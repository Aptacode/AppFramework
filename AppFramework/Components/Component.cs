using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Plugins;
using Aptacode.AppFramework.Plugins.Behaviours;
using Aptacode.AppFramework.Plugins.States;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components;

public abstract class Component : IDisposable
{
    #region Ctor

    protected Component(Primitive primitive)
    {
        Primitive = primitive;
        OldBoundingRectangle = Primitive.BoundingRectangle;

        Id = Guid.NewGuid();
        IsShown = true;
        BorderThickness = DefaultBorderThickness;
        BorderColor = Color.Black;
        FillColor = Color.White;
        Invalidated = true;
    }

    #endregion

    public ComponentPlugins Plugins { get; set; } = new();

    public virtual void Dispose()
    {
    }

    public void Handle(float delta)
    {
        Plugins.Tick.All.All(p => p.Handle(delta));
    }

    public class ComponentPlugins
    {
        public PluginCollection<ComponentBehaviour<UiEvent>> Ui { get; } = new();
        public PluginCollection<ComponentBehaviour<TransformationEvent>> Transformation { get; } = new();
        public PluginCollection<ComponentBehaviour<float>> Tick { get; } = new();
        public PluginCollection<ComponentState> State { get; } = new();
    }

    #region Canvas

    public virtual void CustomDraw(BlazorCanvasInterop ctx)
    {
    }

    public virtual void DrawText(BlazorCanvasInterop ctx)
    {
        if (string.IsNullOrEmpty(Text))
        {
            return;
        }

        ctx.TextAlign("center");
        ctx.FillStyle("black");
        ctx.Font("10pt Calibri");
        ctx.WrapText(Text, Primitive.BoundingRectangle.BottomLeft.X,
            Primitive.BoundingRectangle.BottomLeft.Y,
            Primitive.BoundingRectangle.Size.X,
            Primitive.BoundingRectangle.Size.Y, 16);
    }

    public virtual void Draw(Scene.Scene scene, BlazorCanvasInterop ctx)
    {
        OldBoundingRectangle = Primitive.BoundingRectangle;
        Invalidated = false;

        if (!IsShown)
        {
            return;
        }

        ctx.FillStyle(FillColorName);

        ctx.StrokeStyle(BorderColorName);

        ctx.LineWidth(BorderThickness);

        CustomDraw(ctx);

        foreach (var child in _children)
        {
            child.Draw(scene, ctx);
        }

        DrawText(ctx);
    }

    #endregion

    #region Children

    public IEnumerable<Component> Children => _children;

    public virtual void Add(Component child)
    {
        if (!Children.Contains(child))
        {
            child.Parent = this;
            _children.Add(child);
            Invalidated = true;
        }
    }

    public virtual void AddRange(IEnumerable<Component> children)
    {
        foreach (var child in children)
        {
            Add(child);
        }
    }

    public virtual void Remove(Component child)
    {
        if (_children.Remove(child))
        {
            child.Parent = null;
            Invalidated = true;
        }
    }

    #endregion

    #region Defaults

    public static readonly string DefaultBorderColor = Color.Black.ToKnownColor().ToString();
    public static readonly string DefaultFillColor = Color.Black.ToKnownColor().ToString();
    public static readonly float DefaultBorderThickness = 0.1f;
    public Primitive Primitive { get; protected set; }

    #endregion

    #region Properties

    public Guid Id { get; init; }
    protected readonly List<Component> _children = new();
    public bool Invalidated { get; set; }
    public BoundingRectangle OldBoundingRectangle { get; protected set; }

    #region Parent

    public Component Parent { get; set; }

    #endregion

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

    public virtual bool CollidesWith(Component component)
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
        {
            child.Translate(delta);
        }

        //Invalidate the component
        Invalidated = true;

        //Handle the transformation event
        Handle(new TranslateEvent(delta));
    }

    public virtual void Rotate(float theta)
    {
        Primitive.Rotate(theta);

        foreach (var child in Children)
        {
            child.Rotate(theta);
        }

        Invalidated = true;
        Handle(new RotateEvent(Primitive.BoundingRectangle.Center, theta));
    }

    public virtual void Rotate(Vector2 rotationCenter, float theta)
    {
        Primitive.Rotate(rotationCenter, theta);

        foreach (var child in Children)
        {
            child.Rotate(rotationCenter, theta);
        }

        Invalidated = true;
        Handle(new RotateEvent(rotationCenter, theta));
    }

    public virtual void ScaleAboutCenter(Vector2 delta)
    {
        Primitive.ScaleAboutCenter(delta);

        foreach (var child in Children)
        {
            child.Scale(Primitive.BoundingRectangle.Center, delta);
        }

        Invalidated = true;
        Handle(new ScaleEvent(Primitive.BoundingRectangle.Center, delta));
    }

    public virtual void ScaleAboutTopLeft(Vector2 delta)
    {
        Primitive.Scale(delta, Primitive.BoundingRectangle.BottomLeft);

        foreach (var child in Children)
        {
            child.Scale(Primitive.BoundingRectangle.BottomLeft, delta);
        }

        Invalidated = true;
        Handle(new ScaleEvent(Primitive.BoundingRectangle.BottomLeft, delta));
    }

    public virtual void Scale(Vector2 scaleCenter, Vector2 delta)
    {
        Primitive.Scale(scaleCenter, delta);

        foreach (var child in Children)
        {
            child.Scale(scaleCenter, delta);
        }

        Invalidated = true;
        Handle(new ScaleEvent(scaleCenter, delta));
    }

    public virtual void Skew(Vector2 delta)
    {
        Primitive.Skew(delta);

        foreach (var child in Children)
        {
            child.Skew(delta);
        }

        Invalidated = true;
        Handle(new SkewEvent(delta));
    }

    public virtual void SetPosition(Vector2 position, bool source)
    {
        Primitive.SetPosition(position);

        var delta = position - Primitive.BoundingRectangle.BottomLeft;
        foreach (var child in Children)
        {
            child.Translate(delta);
        }

        Invalidated = true;
        Handle(new TranslateEvent(delta));
    }

    public virtual void SetSize(Vector2 size)
    {
        Primitive.SetSize(size);

        var scaleFactor = size / Primitive.BoundingRectangle.Size;
        foreach (var child in Children)
        {
            child.Scale(Primitive.BoundingRectangle.BottomLeft, scaleFactor);
        }

        Invalidated = true;

        Handle(new ScaleEvent(Primitive.BoundingRectangle.BottomLeft, scaleFactor));
    }

    private void Handle(TransformationEvent e)
    {
        Plugins.Transformation.All.All(t => t.Handle(e));
        OnTransformationEvent?.Invoke(this, e);
    }

    #endregion

    #region Events

    //Events
    public event EventHandler<UiEvent> OnUiEvent;
    public event EventHandler<UiEvent> OnUiEventTunneled;
    public event EventHandler<TransformationEvent> OnTransformationEvent;

    public bool Handle(UiEvent uiEvent)
    {
        //Firstly try and handle the event with the most nested child component
        var isBubbleHandled = false;
        foreach (var child in Children)
        {
            if (child.Handle(uiEvent))
            {
                isBubbleHandled = true; //The child handles the event
            }
        }

        //Try and handle the event with this component
        var eventHandled = false;
        foreach (var behaviour in Plugins.Ui.All)
        {
            if (behaviour.Handle(uiEvent))
            {
                eventHandled = true;
            }
        }

        if (!eventHandled)
        {
            return false;
        }

        OnUiEventTunneled?.Invoke(this, uiEvent);

        //If the event 
        if (!isBubbleHandled)
        {
            OnUiEvent?.Invoke(this, uiEvent);
        }

        return true;
    }

    #endregion
}