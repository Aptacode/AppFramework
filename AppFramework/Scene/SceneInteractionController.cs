﻿using System;
using System.Numerics;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.Geometry;

namespace Aptacode.AppFramework.Scene
{
    public class SceneInteractionController
    {
        #region State

        public Vector2 LastMousePosition { get; set; }
        public Vector2 MouseDownPosition { get; set; }
        public DateTime FirstMouseDownTime { get; set; }
        public DateTime SecondMouseDownTime { get; set; }
        public bool IsMouseDown { get; private set; }

        #endregion

        #region Interaction

        #region Mouse

        public void MouseClickDown()
        {
            if (DateTime.Now - FirstMouseDownTime > TimeSpan.FromMilliseconds(300))
            {
                FirstMouseDownTime = DateTime.Now;
            }
            else
            {
                SecondMouseDownTime = DateTime.Now;
            }
        }

        public void MouseClickRelease(Vector2 position)
        {
            if (DateTime.Now - SecondMouseDownTime < TimeSpan.FromMilliseconds(150))
            {
                OnMouseEvent?.Invoke(this, new MouseDoubleClickEvent(position));
            }
            else if (DateTime.Now - FirstMouseDownTime < TimeSpan.FromMilliseconds(150))
            {
                OnMouseEvent?.Invoke(this, new MouseClickEvent(position));
            }
        }

        public void MouseDown(Vector2 position)
        {
            if (IsMouseDown)
            {
                return;
            }

            IsMouseDown = true;

            MouseDownPosition = position;
            MouseClickDown();
            OnMouseEvent?.Invoke(this, new MouseDownEvent(position));
            LastMousePosition = position;
        }

        public void MouseUp(Vector2 position)
        {
            IsMouseDown = false;
            OnMouseEvent?.Invoke(this, new MouseUpEvent(position));
            LastMousePosition = position;

            MouseClickRelease(position);
        }

        public void MouseMove(Vector2 position)
        {
            if (Math.Abs(LastMousePosition.X - position.X) <= Constants.Tolerance &&
                Math.Abs(LastMousePosition.Y - position.Y) <= Constants.Tolerance)
            {
                return;
            }

            OnMouseEvent?.Invoke(this, new MouseMoveEvent(position));
            LastMousePosition = position;
        }

        #endregion

        #region Keyboard

        public string? CurrentKey;
        public bool ControlPressed => CurrentKey == "Control";

        public bool IsPressed(string key)
        {
            return string.Equals(CurrentKey, key, StringComparison.OrdinalIgnoreCase);
        }

        public bool NothingPressed => string.IsNullOrEmpty(CurrentKey);

        public void KeyDown(string key)
        {
            CurrentKey = key;
            OnKeyboardEvent?.Invoke(this, new KeyDownEvent(key));
        }

        public void KeyUp(string key)
        {
            if (ControlPressed)
            {
                CurrentKey = null;
            }

            CurrentKey = null;
            OnKeyboardEvent?.Invoke(this, new KeyUpEvent(key));
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler<MouseEvent> OnMouseEvent;
        public event EventHandler<KeyboardEvent> OnKeyboardEvent;

        #endregion
    }
}