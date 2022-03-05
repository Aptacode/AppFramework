using System;
using System.Numerics;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.Geometry;

namespace Aptacode.AppFramework.Scene;

public class SceneInteractionController
{
    private readonly Scene _scene;

    public SceneInteractionController(Scene scene)
    {
        _scene = scene;
    }

    #region State

    public Vector2 LastMousePosition { get; set; }
    public Vector2 MouseDownPosition { get; set; }
    public DateTime FirstMouseDownTime { get; set; }
    public DateTime SecondMouseDownTime { get; set; }
    public bool IsMouseDown { get; private set; }

    #endregion

    #region Interaction

    #region Mouse

    private Vector2 Transform(Vector2 p)
    {
        return new(p.X, _scene.Size.Y - p.Y);
    }

    public void MouseClickDown()
    {
        if (DateTime.Now - FirstMouseDownTime > TimeSpan.FromMilliseconds(300))
            FirstMouseDownTime = DateTime.Now;
        else
            SecondMouseDownTime = DateTime.Now;
    }

    public void MouseClickRelease(Vector2 position)
    {
        position = Transform(position);

        if (DateTime.Now - SecondMouseDownTime < TimeSpan.FromMilliseconds(150))
            _scene.Handle(new MouseDoubleClickEvent(position));
        else if (DateTime.Now - FirstMouseDownTime < TimeSpan.FromMilliseconds(150))
            _scene.Handle(new MouseClickEvent(position));
    }

    public void MouseDown(Vector2 position)
    {
        if (IsMouseDown) return;
        position = Transform(position);

        IsMouseDown = true;

        MouseDownPosition = position;
        MouseClickDown();
        _scene.Handle(new MouseDownEvent(position));
        LastMousePosition = position;
    }

    public void MouseUp(Vector2 position)
    {
        position = Transform(position);

        IsMouseDown = false;
        _scene.Handle(new MouseUpEvent(position));
        LastMousePosition = position;

        MouseClickRelease(position);
    }

    public void MouseMove(Vector2 position)
    {
        position = Transform(position);

        if (Math.Abs(LastMousePosition.X - position.X) <= Constants.Tolerance &&
            Math.Abs(LastMousePosition.Y - position.Y) <= Constants.Tolerance)
            return;

        _scene.Handle(new MouseMoveEvent(position));
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
        _scene.Handle(new KeyDownEvent(key));
    }

    public void KeyUp(string key)
    {
        if (ControlPressed) CurrentKey = null;

        CurrentKey = null;
        _scene.Handle(new KeyUpEvent(key));
    }

    #endregion

    #endregion
}