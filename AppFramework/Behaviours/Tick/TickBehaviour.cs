﻿using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Behaviours.Tick;

public abstract class TickBehaviour
{
    protected TickBehaviour(Scene.Scene scene, ComponentViewModel component)
    {
        Scene = scene;
        Component = component;
    }

    public abstract bool HandleEvent(float deltaT);

    #region Properties

    public Scene.Scene Scene { get; init; }
    public ComponentViewModel Component { get; init; }
    public bool Enabled { get; set; } = true;

    #endregion
}