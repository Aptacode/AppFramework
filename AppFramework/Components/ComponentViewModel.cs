using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Enums;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;
using Point = Aptacode.Geometry.Primitives.Point;

namespace Aptacode.AppFramework.Components
{
    public abstract class ComponentViewModel : IDisposable
    {
        #region Ctor

        protected ComponentViewModel()
        {
            Id = Guid.NewGuid();
            CollisionDetectionEnabled = true;
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
                ctx.WrapText(Text, BoundingRectangle.TopLeft.X * SceneScale.Value, BoundingRectangle.TopLeft.Y * SceneScale.Value, BoundingRectangle.Size.X * SceneScale.Value, BoundingRectangle.Size.Y * SceneScale.Value, 16);
            }
        }

        public virtual async Task Draw(BlazorCanvasInterop ctx)
        {
            OldBoundingRectangle = BoundingRectangle;
            Invalidated = false;

            if (!IsShown)
            {
                return;
            }

            ctx.FillStyle(FillColorName);

            ctx.StrokeStyle(BorderColorName);

            ctx.LineWidth(BorderThickness * SceneScale.Value);

            await CustomDraw(ctx);

            foreach (var child in _children)
            {
                await child.Draw(ctx);
            }

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
                _children.Add(child);
                UpdateBounds();
                Invalidated = true;
            }
        }

        public virtual void AddRange(IEnumerable<ComponentViewModel> children)
        {
            foreach (var child in children)
            {
                Add(child);
            }
        }

        public virtual void Remove(ComponentViewModel child)
        {
            if (_children.Remove(child))
            {
                UpdateBounds();
                Invalidated = true;
            }
        }

        #endregion

        #region Defaults

        public static readonly string DefaultBorderColor = Color.Black.ToKnownColor().ToString();
        public static readonly string DefaultFillColor = Color.Black.ToKnownColor().ToString();
        public static readonly float DefaultBorderThickness = 0.1f;
        public static readonly float DefaultMargin = 1.0f;

        #endregion

        #region Properties

        public Guid Id { get; init; }
        protected readonly List<ComponentViewModel> _children = new();
        public bool CollisionDetectionEnabled { get; set; }
        public bool Invalidated { get; set; }
        public BoundingRectangle OldBoundingRectangle { get; protected set; }
        public BoundingRectangle BoundingRectangle { get; protected set; }
        public Primitive BoundingPrimitive { get; set; }
        public bool MouseOver { get; protected set; }
        
        
        private VerticalAlignment _verticalAlignment = VerticalAlignment.Stretch;

        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                OnVerticalAlignmentChanged?.Invoke(this, value);
            }
        }

        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Stretch;

        public HorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                OnHorizontalAlignmentChanged?.Invoke(this, value);
            }
        }

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

        public virtual bool CollidesWith(ComponentViewModel component)
        {
            if (!component.BoundingRectangle.CollidesWith(BoundingRectangle))
            {
                return false;
            }

            foreach (var child in Children)
            {
                if (child.CollidesWith(component))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool CollidesWith(Point point)
        {
            if (!BoundingRectangle.CollidesWith(point.BoundingRectangle))
            {
                return false;
            }

            foreach (var child in Children)
            {
                if (child.CollidesWith(point))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool CollidesWith(PolyLine point)
        {
            if (!BoundingRectangle.CollidesWith(point.BoundingRectangle))
            {
                return false;
            }

            foreach (var child in Children)
            {
                if (child.CollidesWith(point))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool CollidesWith(Polygon point)
        {
            if (!BoundingRectangle.CollidesWith(point.BoundingRectangle))
            {
                return false;
            }

            foreach (var child in Children)
            {
                if (child.CollidesWith(point))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool CollidesWith(Ellipse point)
        {
            if (!BoundingRectangle.CollidesWith(point.BoundingRectangle))
            {
                return false;
            }

            foreach (var child in Children)
            {
                if (child.CollidesWith(point))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool CollidesWith(Vector2 point)
        {
            if (!BoundingRectangle.CollidesWith(point))
            {
                return false;
            }

            foreach (var child in Children)
            {
                if (child.CollidesWith(point))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool CollidesWith(BoundingRectangle rectangle)
        {
            return BoundingRectangle.CollidesWith(rectangle);
        }

        #endregion

        #region Transformation

        public virtual void Translate(Vector2 delta)
        {
            foreach (var child in Children)
            {
                child.Translate(delta);
            }

            UpdateBounds();

            Invalidated = true;
            OnTranslated?.Invoke(this, new TranslateEvent());
        }

        public virtual void Rotate(float theta)
        {
            foreach (var child in Children)
            {
                child.Rotate(theta);
            }

            UpdateBounds();

            Invalidated = true;
            OnRotated?.Invoke(this, new RotateEvent());
        }

        public virtual void Rotate(Vector2 rotationCenter, float theta)
        {
            foreach (var child in Children)
            {
                child.Rotate(rotationCenter, theta);
            }

            UpdateBounds();

            Invalidated = true;
            OnRotated?.Invoke(this, new RotateEvent());
        }

        public virtual void ScaleAboutCenter(Vector2 delta)
        {
            foreach (var child in Children)
            {
                child.Scale(BoundingRectangle.Center, delta);
            }

            UpdateBounds();

            Invalidated = true;
            OnScaled?.Invoke(this, new ScaleEvent());
        }

        public virtual void ScaleAboutTopLeft(Vector2 delta)
        {
            foreach (var child in Children)
            {
                child.Scale(BoundingRectangle.TopLeft, delta);
            }

            UpdateBounds();

            Invalidated = true;
            OnScaled?.Invoke(this, new ScaleEvent());
        }

        public virtual void Scale(Vector2 scaleCenter, Vector2 delta)
        {
            foreach (var child in Children)
            {
                child.Scale(scaleCenter, delta);
            }

            UpdateBounds();

            Invalidated = true;
            OnScaled?.Invoke(this, new ScaleEvent());
        }

        public virtual void Skew(Vector2 delta)
        {
            foreach (var child in Children)
            {
                child.Skew(delta);
            }

            UpdateBounds();

            Invalidated = true;
            OnSkewed?.Invoke(this, new SkewEvent());
        }

        public virtual void SetPosition(Vector2 position)
        {
            var delta = position - BoundingRectangle.TopLeft;
            foreach (var child in Children)
            {
                child.Translate(delta);
            }

            UpdateBounds();

            Invalidated = true;
            OnTranslated?.Invoke(this, new TranslateEvent());
        }

        public virtual void SetSize(Vector2 size)
        {
            var scaleFactor = size / BoundingRectangle.Size;
            foreach (var child in Children)
            {
                child.Scale(BoundingRectangle.TopLeft, scaleFactor);
            }

            UpdateBounds();

            Invalidated = true;
            OnScaled?.Invoke(this, new ScaleEvent());
        }

        #endregion

        #region Events

        public bool Handle(UIEvent uiEvent)
        {
            switch (uiEvent)
            {
                case MouseEvent mouseEvent:
                    return HandleMouseEvent(mouseEvent);
                default:
                    return HandleCustomEvent(uiEvent);
            }
        }

        public virtual bool HandleMouseEvent(MouseEvent mouseEvent)
        {
            if (CollidesWith(mouseEvent.Position))
            {
                if (!MouseOver)
                {
                    MouseOver = true;
                    OnMouseEnterEvent?.Invoke(this, new MouseEnterEvent(mouseEvent.Position));
                }
                
                var isTunnelHandled = false;
                //Tunnel
                foreach (var child in Children)
                {
                    if (child.HandleMouseEvent(mouseEvent))
                    {
                        isTunnelHandled = true;
                    }
                }

                switch (mouseEvent)
                {
                    case MouseMoveEvent mouseMoveEvent:
                        OnMouseMoveBubbled?.Invoke(this, mouseMoveEvent);
                        if (!isTunnelHandled)
                        {
                            OnMouseMove?.Invoke(this, mouseMoveEvent);
                        }

                        break;
                    case MouseDownEvent mouseDownEvent:
                        OnMouseDownBubbled?.Invoke(this, mouseDownEvent);
                        if (!isTunnelHandled)
                        {
                            OnMouseDown?.Invoke(this, mouseDownEvent);
                        }

                        break;
                    case MouseUpEvent mouseUpEvent:
                        OnMouseUpBubbled?.Invoke(this, mouseUpEvent);
                        if (!isTunnelHandled)
                        {
                            OnMouseUp?.Invoke(this, mouseUpEvent);
                        }

                        break;
                    case MouseClickEvent mouseClickEvent:
                        OnMouseClickBubbled?.Invoke(this, mouseClickEvent);
                        if (!isTunnelHandled)
                        {
                            OnMouseClick?.Invoke(this, mouseClickEvent);
                        }

                        break;
                    case MouseDoubleClickEvent mouseDoubleClickEvent:
                        OnMouseDoubleClickBubbled?.Invoke(this, mouseDoubleClickEvent);
                        if (!isTunnelHandled)
                        {
                            OnMouseDoubleClick?.Invoke(this, mouseDoubleClickEvent);
                        }

                        break;
                }

                return true;
            }else if (MouseOver)
            {
                MouseOver = false;
                foreach (var child in Children)
                {
                    child.HandleMouseEvent(mouseEvent);
                }
                OnMouseLeaveEvent?.Invoke(this, new MouseLeaveEvent(mouseEvent.Position));
            }

            return false;
        }

        public virtual bool HandleCustomEvent(UIEvent customEvent)
        {
            foreach (var child in Children)
            {
                if (child.HandleCustomEvent(customEvent))
                {
                    return true;
                }
            }

            return false;
        }

        //Mouse
        public event EventHandler<MouseDownEvent> OnMouseDown;
        public event EventHandler<MouseMoveEvent> OnMouseMove;
        public event EventHandler<MouseUpEvent> OnMouseUp;
        public event EventHandler<MouseClickEvent> OnMouseClick;
        public event EventHandler<MouseDoubleClickEvent> OnMouseDoubleClick;

        public event EventHandler<MouseDownEvent> OnMouseDownBubbled;
        public event EventHandler<MouseMoveEvent> OnMouseMoveBubbled;
        public event EventHandler<MouseUpEvent> OnMouseUpBubbled;
        public event EventHandler<MouseClickEvent> OnMouseClickBubbled;
        public event EventHandler<MouseDoubleClickEvent> OnMouseDoubleClickBubbled;
        public event EventHandler<MouseEnterEvent> OnMouseEnterEvent;
        public event EventHandler<MouseLeaveEvent> OnMouseLeaveEvent;

        //Transformation
        public event EventHandler<TranslateEvent> OnTranslated;
        public event EventHandler<RotateEvent> OnRotated;
        public event EventHandler<ScaleEvent> OnScaled;
        public event EventHandler<SkewEvent> OnSkewed;

        public event EventHandler<VerticalAlignment> OnVerticalAlignmentChanged;
        public event EventHandler<HorizontalAlignment> OnHorizontalAlignmentChanged;

        #endregion
    }
}