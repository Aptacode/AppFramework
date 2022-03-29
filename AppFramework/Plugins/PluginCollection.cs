using System.Collections.Generic;

namespace Aptacode.AppFramework.Plugins;

public class PluginCollection<T> where T : Plugin
{
    private readonly Dictionary<string, T> _plugins = new();

    public T? this[string name] => _plugins.TryGetValue(name, out var value) ? value : null;

    public IEnumerable<T> All => _plugins.Values;

    public void Add(T behaviour)
    {
        _plugins[behaviour.Name()] = behaviour;
    }

    public T1? Get<T1>(string name) where T1 : class, T
    {
        return _plugins.TryGetValue(name, out var value) ? value as T1 : null;
    }
}