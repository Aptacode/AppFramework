using System;
using System.Collections.Generic;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Behaviours.Scene;
using Aptacode.AppFramework.Components.Behaviours.Tick;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.CSharp.Common.Utilities.Mvvm;

namespace Aptacode.AppFramework.Scene;

public class Scene : BindableBase
{
    #region Ctor

    public Scene(Vector2 size)
    {
        Size = size;
    }

    #endregion

    #region Behaviours

    private readonly List<SceneBehavior> _behaviors = new();

    public Scene Add(SceneBehavior behavior)
    {
        _behaviors.Add(behavior);
        return this;
    }

    public Scene Remove(SceneBehavior behavior)
    {
        _behaviors.Remove(behavior);
        return this;
    }

    public void Handle(float deltaT)
    {
        //Execute scene behaviours
        foreach (var sceneBehavior in _behaviors)
        {
            sceneBehavior.Handle(deltaT);
        }

        //Execute tick behaviours
        foreach (var component in Components) {component.HandleTick(deltaT);}
    }

    #endregion

    #region Properties

    private readonly List<Component> _components = new();

    public Vector2 Size { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();

    #endregion

    #region Components

    public Scene Add(Component component)
    {
        _components.Add(component);
        OnComponentAdded?.Invoke(this, component);
        return this;
    }

    #region Events

    public void Handle(UiEvent e)
    {
        foreach (var component in Components)
            if (component.Handle(e))
                return;
    }

    #endregion

    public void AddRange(IEnumerable<Component> components)
    {
        foreach (var component in components) Add(component);
    }

    public bool Remove(Component component)
    {
        var success = _components.Remove(component);
        OnComponentRemoved?.Invoke(this, component);
        return success;
    }

    public IReadOnlyList<Component> Components => _components;

    #endregion

    #region Events

    public event EventHandler<Component> OnComponentAdded;
    public event EventHandler<Component> OnComponentRemoved;

    #endregion

    #region Layering

    public void BringToFront(Component componentViewModel)
    {
        if (!_components.Remove(componentViewModel)) return;

        _components.Add(componentViewModel);
    }

    public void SendToBack(Component componentViewModel)
    {
        if (!_components.Remove(componentViewModel)) return;

        _components.Insert(0, componentViewModel);
    }

    public void BringForward(Component componentViewModel)
    {
        var index = _components.IndexOf(componentViewModel);
        if (index == _components.Count - 1) return;

        _components.RemoveAt(index);
        _components.Insert(index + 1, componentViewModel);
    }

    public void SendBackward(Component componentViewModel)
    {
        var index = _components.IndexOf(componentViewModel);
        if (index == 0) return;

        _components.RemoveAt(index);
        _components.Insert(index - 1, componentViewModel);
    }

    #endregion
}