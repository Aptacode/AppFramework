using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Behaviours.Tick;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.CSharp.Common.Utilities.Mvvm;

namespace Aptacode.AppFramework.Scene;

public class SceneController : BindableBase
{
    #region Ctor
    public SceneController(BlazorCanvasInterop canvas, Scene scene)
    {
        _canvas = canvas;
        Id = Guid.NewGuid();
        Size = scene.Size;
        Scene = scene;
        UserInteractionController = new SceneInteractionController();
        UserInteractionController.OnUiEvent += UserInteractionControllerOnUiEvent;
        _behaviors = new List<GlobalBehavior>(){ new GlobalPhysicsBehaviour(Scene) };
    }

    private void UserInteractionControllerOnUiEvent(object? sender, UiEvent e)
    {
        Scene.Handle(e);
    }

    #endregion

    #region Events

    private float _lastTimeStamp = -1;
    public virtual void Tick(float timestamp)
    {
        if (_lastTimeStamp == -1)
        {
            _lastTimeStamp = timestamp;
            return;
        }

        var delta = timestamp - _lastTimeStamp;
        _lastTimeStamp = timestamp;

        //Execute global behaviors
        foreach (var globalBehavior in _behaviors) globalBehavior.Handle(delta);

        //Execute tick behaviours
        foreach (var component in Scene.Components) component.HandleTick(delta);

        //Reset canvas
        _canvas.SelectCanvas(Scene.Id.ToString());
        _canvas.FillStyle(Component.DefaultFillColor);
        _canvas.StrokeStyle(Component.DefaultBorderColor);
        _canvas.LineWidth(Component.DefaultBorderThickness);
        _canvas.ClearRect(0, 0, Scene.Size.X * SceneScale.Value, Scene.Size.Y * SceneScale.Value);

        //Draw each element
        for (var i = 0; i < Scene.Components.Count(); i++)
            Scene.Components.ElementAt(i).Draw(Scene, _canvas);
    }

    #endregion

    #region Properties
    public SceneInteractionController UserInteractionController { get; }
    public Scene Scene { get; set; }

    public string Cursor { get; set; }
    public bool ShowGrid { get; set; }
    public Vector2 Size { get; }
    public Guid Id { get; set; }

    private readonly List<GlobalBehavior> _behaviors;
    private readonly BlazorCanvasInterop _canvas;
    #endregion
}