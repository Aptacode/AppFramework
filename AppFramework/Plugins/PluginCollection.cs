using System.Collections.Generic;

namespace Aptacode.AppFramework.Plugins;

public class PluginCollection
{
    private readonly Dictionary<string, Plugin> _plugins = new();

    public Plugin? this[string name] => _plugins.TryGetValue(name, out var value) ? value : null;

    public IEnumerable<Plugin> All => _plugins.Values;

    public void Add(Plugin behaviour)
    {
        _plugins[behaviour.Name()] = behaviour;
    }

    public T1? Get<T1>(string name) where T1 : Plugin
    {
        return _plugins.TryGetValue(name, out var value) ? value as T1 : null;
    }
}