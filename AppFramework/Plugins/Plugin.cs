namespace Aptacode.AppFramework.Plugins;

public abstract class Plugin
{
    public bool Enabled { get; set; } = true;
    public abstract string Name();
}