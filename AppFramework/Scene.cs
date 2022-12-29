using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Events;
using Aptacode.AppFramework.Plugins;

namespace Aptacode.AppFramework;

public abstract class Scene
{
    public virtual Task Setup()
    {
        return Task.CompletedTask;
    }

    public virtual Task Loop(float deltaT)
    {
        //Execute scene behaviours
        Plugins.Handle(deltaT);

        //Execute tick behaviours
        foreach (var component in Components)
        {
            component.Plugins?.Handle(deltaT);
        }

        return Task.CompletedTask;
    }

    public virtual Task Reset()
    {
        Plugins.Clear();
        _components.Clear();
        return Task.CompletedTask;
    }

    public virtual Task Teardown()
    {
        return Task.CompletedTask;
    }

    #region Properties
    public PluginCollection Plugins { get; set; } = new();

    protected readonly List<Component> _components = new();

    public int Width { get; set; } = 200;
    public int Height { get; set; } = 200;

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
        Plugins.Handle(e);

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
}