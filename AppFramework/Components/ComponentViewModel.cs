using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Behaviours.Tick;
using Aptacode.AppFramework.Behaviours.Transformation;
using Aptacode.AppFramework.Behaviours.Ui;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Vertices;

namespace Aptacode.AppFramework.Components;

public abstract class ComponentViewModel : IDisposable
{
    #region Ctor

    protected ComponentViewModel(Primitive primitive)
    {
        Primitive = primitive;

        Id = Guid.NewGuid();
        Margin = DefaultMargin;
        IsShown = true;
        BorderThickness = DefaultBorderThickness;
        BorderColor = Color.Black;
        FillColor = Color.White;
        UpdateBounds();
        OldBoundingRectangle = BoundingPrimitive.BoundingRectangle;
        Invalidated = true;
    }

    #endregion

    public virtual void Dispose()
    {
    }

    #region Canvas

    public virtual void CustomDraw(BlazorCanvasInterop ctx)
    {
    }

    public virtual void DrawText(BlazorCanvasInterop ctx)
    {
        if (string.IsNullOrEmpty(Text))
            return;

        ctx.TextAlign("center");
        ctx.FillStyle("black");
        ctx.Font("10pt Calibri");
        ctx.WrapText(Text, BoundingPrimitive.BoundingRectangle.TopLeft.X * SceneScale.Value,
            BoundingPrimitive.BoundingRectangle.TopLeft.Y * SceneScale.Value,
            BoundingPrimitive.BoundingRectangle.Size.X * SceneScale.Value,
            BoundingPrimitive.BoundingRectangle.Size.Y * SceneScale.Value, 16);
    }

    public virtual void Draw(Scene.Scene scene, BlazorCanvasInterop ctx)
    {
        OldBoundingRectangle = BoundingPrimitive.BoundingRectangle;
        Invalidated = false;

        if (!IsShown) return;

        ctx.FillStyle(FillColorName);

        ctx.StrokeStyle(BorderColorName);

        ctx.LineWidth(BorderThickness * SceneScale.Value);

        CustomDraw(ctx);

        foreach (var child in _children) child.Draw(scene, ctx);

        DrawText(ctx);
    }

    #endregion

    #region Children

    public IEnumerable<ComponentViewModel> Children => _children;

    protected void UpdateBounds()
    {
        var vertices = AllVertices().ToArray();

        if (Margin > Constants.Tolerance) vertices = new VertexArray(vertices).ToConvexHull(Margin).Vertices;

        BoundingPrimitive = Polygon.Create(vertices);
    }

    protected IEnumerable<Vector2> AllVertices()
    {
        var vertexArrays = Primitive.Vertices.Vertices.ToList();
        vertexArrays.AddRange(_children.SelectMany(v => v.AllVertices()));
        return vertexArrays;
    }

    public virtual void Add(ComponentViewModel child)
    {
        if (!Children.Contains(child))
        {
            child.Parent = this;
            _children.Add(child);
            UpdateBounds();
            Invalidated = true;
        }
    }

    public virtual void AddRange(IEnumerable<ComponentViewModel> children)
    {
        foreach (var child in children) Add(child);
    }

    public virtual void Remove(ComponentViewModel child)
    {
        if (_children.Remove(child))
        {
            child.Parent = null;
            UpdateBounds();
            Invalidated = true;
        }
    }

    #endregion

    #region Defaults

    public static readonly string DefaultBorderColor = Color.Black.ToKnownColor().ToString();
    public static readonly string DefaultFillColor = Color.Black.ToKnownColor().ToString();
    public static readonly float DefaultBorderThickness = 0.1f;
    public static readonly float DefaultMargin = 0.0f;
    public Primitive Primitive { get; }

    #endregion

    #region Properties

    public Guid Id { get; init; }
    protected readonly List<ComponentViewModel> _children = new();
    public bool Invalidated { get; set; }
    public BoundingRectangle OldBoundingRectangle { get; protected set; }
    public Primitive BoundingPrimitive { get; set; }

    #region Parent

    public ComponentViewModel Parent { get; set; }

    #endregion

    private float _margin;

    public float Margin
    {
        get => _margin;
        set
        {
            _margin = value;
            UpdateBounds();
            Invalidated = true;
        }
    }

    private bool _isShown;

    public bool IsShown
    {
        get => _isShown;
        set
        {
            _isShown = value;
            Invalidated = true;
        }
    }

    private string _text;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            Invalidated = true;
        }
    }

    private Color _borderColor;

    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            BorderColorName = $"rgba({value.R},{value.G},{value.B},{value.A})";
            Invalidated = true;
        }
    }

    public string? BorderColorName { get; private set; }

    private Color _fillColor;

    public Color FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            FillColorName = $"rgba({value.R},{value.G},{value.B},{value.A / 255.0})";
            Invalidated = true;
        }
    }

    public string FillColorName { get; private set; }

    private float _borderThickness;

    public float BorderThickness
    {
        get => _borderThickness;
        set
        {
            _borderThickness = value;
            Invalidated = true;
        }
    }

    #endregion

    #region CollisionDetection

    public virtual bool CollidesWith(Vector2 point)
    {
        return Primitive.CollidesWith(point) || Children.Any(child => child.CollidesWith(point));
    }

    public virtual bool CollidesWith(Primitive primitive)
    {
        return Primitive.CollidesWithPrimitive(primitive) || Children.Any(child => child.CollidesWith(primitive));
    }

    public virtual bool CollidesWith(ComponentViewModel component)
    {
        return Primitive.CollidesWithPrimitive(component.Primitive) ||
               Children.Any(child => child.CollidesWith(component));
    }

    #endregion

    #region Transformation

    public virtual void Translate(Vector2 delta)
    {
        //Translate the components primitive
        Primitive.Translate(delta);

        //Translate each child component
        foreach (var child in Children)
            child.Translate(delta);

        //Update the components bounds
        UpdateBounds();

        //Invalidate the component
        Invalidated = true;
        
        //Handle the transformation event
        HandleTransformationEvent(new TranslateEvent(delta));
    }

    public virtual void Rotate(float theta)
    {
        Primitive.Rotate(theta);

        foreach (var child in Children) child.Rotate(theta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(new RotateEvent(Primitive.BoundingRectangle.Center, theta));
    }

    public virtual void Rotate(Vector2 rotationCenter, float theta)
    {
        Primitive.Rotate(rotationCenter, theta);

        foreach (var child in Children) child.Rotate(rotationCenter, theta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(new RotateEvent(rotationCenter, theta));
    }

    public virtual void ScaleAboutCenter(Vector2 delta)
    {
        Primitive.ScaleAboutCenter(delta);

        foreach (var child in Children) child.Scale(BoundingPrimitive.BoundingRectangle.Center, delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(new ScaleEvent(BoundingPrimitive.BoundingRectangle.Center, delta));
    }

    public virtual void ScaleAboutTopLeft(Vector2 delta)
    {
        Primitive.Scale(delta, BoundingPrimitive.BoundingRectangle.TopLeft);

        foreach (var child in Children) child.Scale(BoundingPrimitive.BoundingRectangle.TopLeft, delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(new ScaleEvent(BoundingPrimitive.BoundingRectangle.TopLeft, delta));
    }

    public virtual void Scale(Vector2 scaleCenter, Vector2 delta)
    {
        Primitive.Scale(scaleCenter, delta);

        foreach (var child in Children) child.Scale(scaleCenter, delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(new ScaleEvent(scaleCenter, delta));
    }

    public virtual void Skew(Vector2 delta)
    {
        Primitive.Skew(delta);

        foreach (var child in Children) child.Skew(delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(new SkewEvent(delta));
    }

    public virtual void SetPosition(Vector2 position, bool source)
    {
        Primitive.SetPosition(position);

        var delta = position - BoundingPrimitive.BoundingRectangle.TopLeft;
        foreach (var child in Children) child.Translate(delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(new TranslateEvent(delta));
    }

    public virtual void SetSize(Vector2 size)
    {
        Primitive.SetSize(size);

        var scaleFactor = size / BoundingPrimitive.BoundingRectangle.Size;
        foreach (var child in Children) child.Scale(BoundingPrimitive.BoundingRectangle.TopLeft, scaleFactor);

        UpdateBounds();

        Invalidated = true;

        HandleTransformationEvent(new ScaleEvent(BoundingPrimitive.BoundingRectangle.TopLeft, scaleFactor));
    }

    private void HandleTransformationEvent(TransformationEvent transformationEvent)
    {
        _transformationBehaviours.Values.All(t => t.HandleEvent(transformationEvent));
        OnTransformationEvent?.Invoke(this, transformationEvent);
    }

    #endregion

    #region Events

    //Events
    public event EventHandler<UiEvent> OnUiEvent;
    public event EventHandler<UiEvent> OnUiEventTunneled;
    public event EventHandler<TransformationEvent> OnTransformationEvent;

    public bool Handle(Scene.Scene scene, UiEvent uiEvent)
    {
        //Firstly try and handle the event with the most nested child component
        var isBubbleHandled = false;
        foreach (var child in Children)
            if (child.Handle(scene, uiEvent))
                isBubbleHandled = true; //The child handles the event

        //Try and handle the event with this component
        var eventHandled = false;
        foreach (var behaviour in _uiBehaviours.Values)
            if (behaviour.HandleEvent(uiEvent))
                eventHandled = true;

        if (!eventHandled)
            return false;

        OnUiEventTunneled?.Invoke(this, uiEvent);

        //If the event 
        if (!isBubbleHandled) OnUiEvent?.Invoke(this, uiEvent);

        return true;
    }

    #region Ui

    public void AddUiBehaviour<T>(T behaviour) where T : UiBehaviour
    {
        _uiBehaviours[typeof(T).Name] = behaviour;
    }

    public bool HasUiBehaviour<T>() where T : UiBehaviour
    {
        return _uiBehaviours.ContainsKey(typeof(T).Name);
    }

    private readonly Dictionary<string, UiBehaviour> _uiBehaviours = new();

    #endregion

    #region Transformations

    public void AddTransformationBehaviour<T>(T behaviour) where T : TransformationBehaviour
    {
        _transformationBehaviours[typeof(T).Name] = behaviour;
    }

    public bool HasTransformationBehaviour<T>() where T : TransformationBehaviour
    {
        return _transformationBehaviours.ContainsKey(typeof(T).Name);
    }

    public T? GetTransformationBehaviour<T>() where T : TransformationBehaviour
    {
        return _transformationBehaviours.TryGetValue(typeof(T).Name, out var value) ? value as T : null;
    }

    private readonly Dictionary<string, TransformationBehaviour> _transformationBehaviours = new();

    #endregion

    #region Tick

    public void AddTickBehaviour<T>(T behaviour) where T : TickBehaviour
    {
        _tickBehaviours[typeof(T).Name] = behaviour;
    }

    public bool HasTickBehaviour<T>() where T : TickBehaviour
    {
        return _tickBehaviours.ContainsKey(typeof(T).Name);
    }

    public T? GetTickBehaviour<T>() where T : TickBehaviour
    {
        return _tickBehaviours.TryGetValue(typeof(T).Name, out var value) ? value as T : null;
    }

    private readonly Dictionary<string, TickBehaviour> _tickBehaviours = new();

    public void HandleTick(float timestamp)
    {
        foreach (var tickBehaviour in _tickBehaviours.Values) tickBehaviour.HandleEvent(timestamp);
    }

    #endregion

    #endregion
}