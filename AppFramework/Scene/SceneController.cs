using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.BlazorCanvas;
using Aptacode.CSharp.Common.Utilities.Mvvm;

namespace Aptacode.AppFramework.Scene;

public class SceneController : BindableBase
{
    public void Setup(BlazorCanvasInterop canvas)
    {
        Renderer = new SceneRenderer(canvas, this);
        Id = Guid.NewGuid();
    }

    #region Ctor

    public SceneController(Vector2 size)
    {
        Size = size;
        Scenes = new List<Scene>();
        UserInteractionController = new SceneInteractionController();
        UserInteractionController.OnUiEvent += UserInteractionControllerOnUiEvent;
    }

    private void UserInteractionControllerOnUiEvent(object? sender, UiEvent e)
    {
        foreach (var scene in Scenes.ToList()) scene.Handle(e);
    }

    #endregion

    #region Events

    private DateTime _lastTick = DateTime.Now;

    public virtual void Tick(float timestamp)
    {
        var currentTime = DateTime.Now;
        var delta = currentTime - _lastTick;
        var frameRate = 1.0f / delta.TotalSeconds;
        _lastTick = currentTime;
        //Console.WriteLine($"{frameRate}");

        foreach (var scene in Scenes)
        {
            foreach (var component in scene.Components)        
            {
                component.HandleTick(timestamp);
            }
        }

        Renderer.Redraw();
    }

    #endregion

    #region Properties

    public SceneRenderer Renderer { get; private set; }
    public SceneInteractionController UserInteractionController { get; }
    public List<Scene> Scenes { get; set; }

    public string Cursor { get; set; }
    public bool ShowGrid { get; set; }
    public Vector2 Size { get; }
    public Guid Id { get; set; }

    #endregion

    #region Scene

    public void Add(Scene scene)
    {
        Scenes.Add(scene);
    }

    public bool Remove(Scene scene)
    {
        return Scenes.Remove(scene);
    }

    #endregion
}