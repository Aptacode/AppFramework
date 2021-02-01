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

        public virtual async Task Draw(Scene.Scene scene, BlazorCanvasInterop ctx)
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
                await child.Draw(scene, ctx);
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
                child.Parent = this;
                child.OnDrag += Child_OnDrag;
                child.OnDrop += Child_OnDrop;
                _children.Add(child);
                UpdateBounds();
                Invalidated = true;
            }
        }
        private void Child_OnDrag(object sender, DragEvent e)
        {
            OnDrag?.Invoke(sender, e);
        }

        private void Child_OnDrop(object sender, DropEvent e)
        {
            OnDrop?.Invoke(sender, e);
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
                child.Parent = null;
                child.OnDrag -= Child_OnDrag;
                child.OnDrop -= Child_OnDrop;
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
        public bool CollisionDetectionEnabled { get; set; }
        public bool Invalidated { get; set; }
        public BoundingRectangle OldBoundingRectangle { get; protected set; }
        public BoundingRectangle BoundingRectangle { get; protected set; }
        public Primitive BoundingPrimitive { get; set; }
        public bool MouseOver { get; protected set; }
        public bool HasFocus { get; protected set; }

        #region Parent

        private ComponentViewModel _parent;

        public ComponentViewModel Parent
        {
            get => _parent;
            set
            {
                if (_parent != null)
                {
                    _parent.OnScaled -= ParentOnOnScaled;
                }

                _parent = value;
                if (_parent != null)
                {
                    Reposition();
                    _parent.OnScaled += ParentOnOnScaled;
                }
            }
        }

        private void ParentOnOnScaled(object? sender, ScaleEvent e)
        {
            if (_parent != null)
            {
                Reposition();
            }
        }

        protected void Reposition()
        {
            var cellPosition = Parent.BoundingRectangle.TopLeft;
            var cellSize = Parent.BoundingRectangle.Size;

            var xPos = BoundingRectangle.X;
            var yPos = BoundingRectangle.Y;
            var xSize = BoundingRectangle.Width;
            var ySize = BoundingRectangle.Height;

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                    ySize = cellSize.Y;
                    yPos = cellPosition.Y;
                    break;
                case VerticalAlignment.Top:
                    ySize = Math.Min(cellSize.Y, BoundingRectangle.Size.Y);
                    yPos = cellPosition.Y;
                    break;
                case VerticalAlignment.Center:
                    ySize = Math.Min(cellSize.Y, BoundingRectangle.Size.Y);
                    yPos = cellPosition.Y + ySize / 2.0f;
                    break;
                case VerticalAlignment.Bottom:
                    ySize = Math.Min(cellSize.Y, BoundingRectangle.Size.Y);
                    yPos = cellPosition.Y + (cellSize.Y - ySize);
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                    xSize = cellSize.X;
                    xPos = cellPosition.X;
                    break;
                case HorizontalAlignment.Left:
                    xSize = Math.Min(cellSize.X, BoundingRectangle.Size.X);
                    xPos = cellPosition.X;
                    break;
                case HorizontalAlignment.Center:
                    xSize = Math.Min(cellSize.X, BoundingRectangle.Size.X);
                    xPos = cellPosition.X + xSize / 2.0f;
                    break;
                case HorizontalAlignment.Right:
                    xSize = Math.Min(cellSize.X, BoundingRectangle.Size.X);
                    xPos = cellPosition.X + xSize;
                    break;
            }

            SetPosition(new Vector2(xPos + Margin, yPos + Margin));
            SetSize(new Vector2(xSize - 2 * Margin, ySize - 2 * Margin));
        }

        #endregion

        private VerticalAlignment _verticalAlignment = VerticalAlignment.None;

        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                OnVerticalAlignmentChanged?.Invoke(this, value);
            }
        }

        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.None;

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
                case KeyboardEvent keyboardEvent:
                    return HandleKeyboardEvent(keyboardEvent);
                default:
                    return HandleCustomEvent(uiEvent);
            }
        }

        public virtual bool HandleKeyboardEvent(KeyboardEvent keyboardEvent)
        {
            var isBubbleHandled = false;

            foreach (var child in Children)
            {
                if (child.HandleKeyboardEvent(keyboardEvent))
                {
                    isBubbleHandled = true;
                    break;
                }
            }

            OnKeyboardEventTunneled?.Invoke(this, keyboardEvent);

            if (!isBubbleHandled)
            {
                OnKeyboardEvent?.Invoke(this, keyboardEvent);
            }

            return true;
        }


        public virtual bool HandleMouseEvent(MouseEvent mouseEvent)
        {
            var hasPassedEventToChildren = false;
            if (IsDragging && mouseEvent is MouseUpEvent m)
            {
                IsDragging = false;
                OnDrop?.Invoke(this, new DropEvent(this, m.Position));
            }

            if (CollidesWith(mouseEvent.Position))
            {
                if (!HasFocus && mouseEvent is MouseDownEvent)
                {
                    HasFocus = true;
                    OnHasFocusChanged?.Invoke(this, true);
                }


                if (!MouseOver)
                {
                    MouseOver = true;
                    OnMouseEnterEvent?.Invoke(this, new MouseEnterEvent(mouseEvent.Position));
                }

                var isBubbleHandled = false;
                //Already passed to children
                hasPassedEventToChildren = true;

                //Bubbling
                foreach (var child in Children)
                {
                    if (child.HandleMouseEvent(mouseEvent))
                    {
                        isBubbleHandled = true;
                    }
                }

                switch (mouseEvent)
                {
                    case MouseMoveEvent mouseMoveEvent:
                        //if (IsDragging)
                        //{
                        //    var delta = mouseMoveEvent.Position - LastDragPosition;
                        //    Translate(delta);
                        //    LastDragPosition = mouseMoveEvent.Position;
                        //}

                        OnMouseMoveTunneled?.Invoke(this, mouseMoveEvent);
                        if (!isBubbleHandled)
                        {
                            OnMouseMove?.Invoke(this, mouseMoveEvent);
                        }

                        break;
                    case MouseDownEvent mouseDownEvent:
                        if (CanDrag)
                        {
                            LastDragPosition = mouseDownEvent.Position;
                            IsDragging = true;
                            OnDrag?.Invoke(this, new DragEvent(this, mouseDownEvent.Position));
                        }

                        OnMouseDownTunneled?.Invoke(this, mouseDownEvent);
                        if (!isBubbleHandled)
                        {
                            OnMouseDown?.Invoke(this, mouseDownEvent);
                        }

                        break;
                    case MouseUpEvent mouseUpEvent:

                        OnMouseUpTunneled?.Invoke(this, mouseUpEvent);
                        if (!isBubbleHandled)
                        {
                            OnMouseUp?.Invoke(this, mouseUpEvent);
                        }

                        break;
                    case MouseClickEvent mouseClickEvent:
                        OnMouseClickTunneled?.Invoke(this, mouseClickEvent);
                        if (!isBubbleHandled)
                        {
                            OnMouseClick?.Invoke(this, mouseClickEvent);
                        }

                        break;
                    case MouseDoubleClickEvent mouseDoubleClickEvent:
                        OnMouseDoubleClickTunneled?.Invoke(this, mouseDoubleClickEvent);
                        if (!isBubbleHandled)
                        {
                            OnMouseDoubleClick?.Invoke(this, mouseDoubleClickEvent);
                        }

                        break;
                }

                return true;
            }

            if (HasFocus && mouseEvent is MouseDownEvent)
            {
                HasFocus = false;
                if (!hasPassedEventToChildren)
                {
                    foreach (var child in Children)
                    {
                        child.HandleMouseEvent(mouseEvent);
                    }

                    hasPassedEventToChildren = true;
                }

                OnHasFocusChanged?.Invoke(this, false);
            }

            if (MouseOver)
            {
                MouseOver = false;
                if (!hasPassedEventToChildren)
                {
                    foreach (var child in Children)
                    {
                        child.HandleMouseEvent(mouseEvent);
                    }

                    hasPassedEventToChildren = true;
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

        public event EventHandler<MouseDownEvent> OnMouseDownTunneled;
        public event EventHandler<MouseMoveEvent> OnMouseMoveTunneled;
        public event EventHandler<MouseUpEvent> OnMouseUpTunneled;
        public event EventHandler<MouseClickEvent> OnMouseClickTunneled;
        public event EventHandler<MouseDoubleClickEvent> OnMouseDoubleClickTunneled;
        public event EventHandler<MouseEnterEvent> OnMouseEnterEvent;
        public event EventHandler<MouseLeaveEvent> OnMouseLeaveEvent;

        public event EventHandler<bool> OnHasFocusChanged;

        //Keyboard
        public event EventHandler<KeyboardEvent> OnKeyboardEvent;
        public event EventHandler<KeyboardEvent> OnKeyboardEventTunneled;

        //Transformation
        public event EventHandler<TranslateEvent> OnTranslated;
        public event EventHandler<RotateEvent> OnRotated;
        public event EventHandler<ScaleEvent> OnScaled;
        public event EventHandler<SkewEvent> OnSkewed;

        public event EventHandler<VerticalAlignment> OnVerticalAlignmentChanged;
        public event EventHandler<HorizontalAlignment> OnHorizontalAlignmentChanged;

        #endregion

        #region DragDrop

        public bool IsDragging { get; set; }
        public bool CanDrag { get; set; }
        public bool CanDrop { get; set; }
        public Vector2 LastDragPosition { get; set; }

        public event EventHandler<DragEvent> OnDrag;
        public event EventHandler<DropEvent> OnDrop;

        public virtual bool Accepts(DropEvent dropEvent)
        {
            if (CanDrop && CollidesWith(dropEvent.Position) && dropEvent.Component != this)
            {
                var component = dropEvent.Component;
                component.Parent?.Remove(component);
                Add(component);
                return true;
            }
            return false;
        }

        public virtual bool Accepts(DropFailedEvent dragFailedEvent)
        {
            Add(dragFailedEvent.Component);
            return true;
        }

        #endregion
    }
}