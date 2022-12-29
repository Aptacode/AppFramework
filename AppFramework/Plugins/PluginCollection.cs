using Aptacode.AppFramework.Events;
using System.Collections.Generic;
using System.Data;

namespace Aptacode.AppFramework.Plugins;

public class PluginCollection
{
    private readonly Dictionary<string, Plugin> _plugins = new();

    public Plugin? this[string name] => _plugins.TryGetValue(name, out Plugin value) ? value : null;

    public IEnumerable<Plugin> All => _plugins.Values;

    public void Add(Plugin behaviour)
    {
        _plugins[behaviour.Name()] = behaviour;
    }
    public void Remove(Plugin behaviour)
    {
        _plugins.Remove(behaviour.Name());
    }

    public T1? Get<T1>(string name) where T1 : Plugin
    {
        return _plugins.TryGetValue(name, out var value) ? value as T1 : null;
    }

    public void Clear()
    {
        _plugins.Clear();
    }

    public void Handle(float delta)
    {
        foreach (var plugin in _plugins.Values)
        {
            plugin.Handle(delta);
        }
    }

    public bool Handle(UiEvent uiEvent)
    {
        var handled = false;
        foreach (var plugin in _plugins.Values)
        {
            if (plugin.Handle(uiEvent))
            {
                handled = true;
            }
        }
        return handled;
    }
}