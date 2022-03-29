using System;
using System.Collections.Generic;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Behaviours.Scene;
using Aptacode.AppFramework.Components.States.Scene;
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

    #region TickBehaviours

    private readonly List<SceneTickBehaviour> _tickBehaviours = new();

    public Scene Add(SceneTickBehaviour tickBehaviour)
    {
        _tickBehaviours.Add(tickBehaviour);
        return this;
    }

    public Scene Remove(SceneTickBehaviour tickBehaviour)
    {
        _tickBehaviours.Remove(tickBehaviour);
        return this;
    }

    public void Handle(float deltaT)
    {
        //Execute scene behaviours
        foreach (var sceneBehavior in _tickBehaviours)
        {
            sceneBehavior.Handle(deltaT);
        }

        //Execute tick behaviours
        foreach (var component in Components)
        {
            component.HandleTick(deltaT);
        }
    }

    #endregion

    #region Behaviours

    private readonly List<SceneUiBehaviour> _uiBehaviours = new();

    public Scene Add(SceneUiBehaviour uiBehaviour)
    {
        _uiBehaviours.Add(uiBehaviour);
        return this;
    }

    public Scene Remove(SceneUiBehaviour uiBehaviour)
    {
        _uiBehaviours.Remove(uiBehaviour);
        return this;
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
        //Execute scene behaviours
        foreach (var sceneBehavior in _uiBehaviours)
        {
            sceneBehavior.Handle(e);
        }

        //Execute tick behaviours
        foreach (var component in Components)
        {
            if (component.Handle(e))
            {
                return;
            }
        }
    }

    #endregion

    public void AddRange(IEnumerable<Component> components)
    {
        foreach (var component in components)
        {
            Add(component);
        }
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
        if (!_components.Remove(componentViewModel))
        {
            return;
        }

        _components.Add(componentViewModel);
    }

    public void SendToBack(Component componentViewModel)
    {
        if (!_components.Remove(componentViewModel))
        {
            return;
        }

        _components.Insert(0, componentViewModel);
    }

    public void BringForward(Component componentViewModel)
    {
        var index = _components.IndexOf(componentViewModel);
        if (index == _components.Count - 1)
        {
            return;
        }

        _components.RemoveAt(index);
        _components.Insert(index + 1, componentViewModel);
    }

    public void SendBackward(Component componentViewModel)
    {
        var index = _components.IndexOf(componentViewModel);
        if (index == 0)
        {
            return;
        }

        _components.RemoveAt(index);
        _components.Insert(index - 1, componentViewModel);
    }

    #endregion

    #region States

    public void AddState<T>(T state) where T : SceneState
    {
        _states[typeof(T).Name] = state;
    }

    public bool HasState<T>() where T : SceneState
    {
        return _states.ContainsKey(typeof(T).Name);
    }

    public T? GetState<T>() where T : SceneState
    {
        return _states.TryGetValue(typeof(T).Name, out var value) ? value as T : null;
    }

    private readonly Dictionary<string, SceneState> _states = new();

    #endregion
}