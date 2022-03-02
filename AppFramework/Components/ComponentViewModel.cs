using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;
using Point = Aptacode.Geometry.Primitives.Point;

namespace Aptacode.AppFramework.Components;

public abstract class ComponentViewModel : IDisposable
{
    #region Ctor

    protected ComponentViewModel()
    {
        Id = Guid.NewGuid();
        Margin = DefaultMargin;
        IsShown = true;
        BorderThickness = DefaultBorderThickness;
        BorderColor = Color.Black;
        FillColor = Color.White;
        Invalidated = true;
        OldBoundingRectangle = BoundingRectangle = BoundingRectangle.Zero;
    }

    #endregion

    public virtual void Dispose()
    {
    }

    #region Canvas

    public virtual async Task CustomDraw(BlazorCanvasInterop ctx)
    {
    }

    public virtual async Task DrawText(BlazorCanvasInterop ctx)
    {
        if (!string.IsNullOrEmpty(Text))
        {
            ctx.TextAlign("center");
            ctx.FillStyle("black");
            ctx.Font("10pt Calibri");
            ctx.WrapText(Text, BoundingRectangle.TopLeft.X * SceneScale.Value,
                BoundingRectangle.TopLeft.Y * SceneScale.Value, BoundingRectangle.Size.X * SceneScale.Value,
                BoundingRectangle.Size.Y * SceneScale.Value, 16);
        }
    }

    public virtual async Task Draw(Scene.Scene scene, BlazorCanvasInterop ctx)
    {
        OldBoundingRectangle = BoundingRectangle;
        Invalidated = false;

        if (!IsShown) return;

        ctx.FillStyle(FillColorName);

        ctx.StrokeStyle(BorderColorName);

        ctx.LineWidth(BorderThickness * SceneScale.Value);

        await CustomDraw(ctx);

        foreach (var child in _children) await child.Draw(scene, ctx);

        await DrawText(ctx);
    }

    #endregion

    #region Children

    public IEnumerable<ComponentViewModel> Children => _children;

    public abstract void UpdateBounds();

    public BoundingRectangle GetChildrenBoundingRectangle()
    {
        return _children.ToBoundingRectangle();
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

    #endregion

    #region Properties

    public Guid Id { get; init; }
    protected readonly List<ComponentViewModel> _children = new();
    public bool Invalidated { get; set; }
    public BoundingRectangle OldBoundingRectangle { get; protected set; }
    public BoundingRectangle BoundingRectangle { get; protected set; }
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
        return BoundingRectangle.CollidesWith(point) && Children.Any(child => child.CollidesWith(point));
    }
    public virtual bool CollidesWith(Primitive primitive)
    {
        return primitive.BoundingRectangle.CollidesWith(BoundingRectangle) && Children.Any(child => child.CollidesWith(primitive));
    }
    public virtual bool CollidesWith(ComponentViewModel component)
    {
        return component.BoundingRectangle.CollidesWith(BoundingRectangle) && Children.Any(child => child.CollidesWith(component));
    }

    #endregion

    #region Transformation

    public virtual void Translate(Scene.Scene scene, Vector2 delta, bool source)
    {
        foreach (var child in Children) child.Translate(scene, delta, false);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(scene, new TranslateEvent(){Source = source, Delta = delta});
    }

    public virtual void Rotate(Scene.Scene scene, float theta)
    {
        foreach (var child in Children) child.Rotate(scene, theta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(scene, new RotateEvent());
    }

    public virtual void Rotate(Scene.Scene scene, Vector2 rotationCenter, float theta)
    {
        foreach (var child in Children) child.Rotate(scene, rotationCenter, theta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(scene, new RotateEvent());
    }

    public virtual void ScaleAboutCenter(Scene.Scene scene, Vector2 delta)
    {
        foreach (var child in Children) child.Scale(scene, BoundingRectangle.Center, delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(scene, new ScaleEvent());
    }

    public virtual void ScaleAboutTopLeft(Scene.Scene scene, Vector2 delta)
    {
        foreach (var child in Children) child.Scale(scene, BoundingRectangle.TopLeft, delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(scene, new ScaleEvent());
    }

    public virtual void Scale(Scene.Scene scene, Vector2 scaleCenter, Vector2 delta)
    {
        foreach (var child in Children) child.Scale(scene, scaleCenter, delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(scene, new ScaleEvent());
    }

    public virtual void Skew(Scene.Scene scene, Vector2 delta)
    {
        foreach (var child in Children) child.Skew(scene, delta);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(scene, new SkewEvent());
    }

    public virtual void SetPosition(Scene.Scene scene, Vector2 position, bool source)
    {
        var delta = position - BoundingRectangle.TopLeft;
        foreach (var child in Children) child.Translate(scene, delta, source);

        UpdateBounds();

        Invalidated = true;
        HandleTransformationEvent(scene, new TranslateEvent());

    }

    public virtual void SetSize(Scene.Scene scene, Vector2 size)
    {
        var scaleFactor = size / BoundingRectangle.Size;
        foreach (var child in Children) child.Scale(scene, BoundingRectangle.TopLeft, scaleFactor);

        UpdateBounds();

        Invalidated = true;

        HandleTransformationEvent(scene, new ScaleEvent());
    }

    private void HandleTransformationEvent(Scene.Scene scene, TransformationEvent transformationEvent)
    {
        _transformationBehaviours.All(t => t.HandleEvent(scene, this, transformationEvent));
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
        if (!HandleEvent(scene, uiEvent))
            return false;

        OnUiEventTunneled?.Invoke(this, uiEvent);

        //If the event 
        if (!isBubbleHandled) OnUiEvent?.Invoke(this, uiEvent);

        return true;
    }

    public bool HandleEvent(Scene.Scene scene, UiEvent uiEvent)
    {
        var eventHandled = false;
        foreach (var behaviour in _uiBehaviours)
            if (behaviour.HandleEvent(scene, this, uiEvent))
                eventHandled = true;

        return eventHandled;
    }

    private readonly List<UiBehaviour> _uiBehaviours = new() { new DragBehaviour() };
    private readonly List<TransformationBehaviour> _transformationBehaviours = new() { new CollisionBehaviour() };

    public bool HasCollisionBehaviour() => true;

    #endregion
}