using System.Collections.Generic;
using System.Drawing;
using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Utilities;

public class ComponentBuilder
{
    public ComponentBuilder SetBorderColor(Color borderColor)
    {
        _borderColor = borderColor;
        return this;
    }

    public ComponentBuilder SetText(string text)
    {
        _text = text;
        return this;
    }

    public ComponentBuilder SetFillColor(Color fillColor)
    {
        _fillColor = fillColor;
        return this;
    }

    public ComponentBuilder SetBase(Component component)
    {
        _baseComponent = component;
        return this;
    }

    public ComponentBuilder AddChild(Component child)
    {
        _children.Add(child);
        return this;
    }

    public Component Build()
    {
        var component = _baseComponent;
        component.Children.AddRange(_children);

        Reset();
        return component;
    }

    public void Reset()
    {
        _baseComponent = null;
        _children.Clear();
        _borderColor = Color.Black;
        _fillColor = Color.White;
        _text = "";
    }

    #region Properties

    private Color _fillColor = Color.White;
    private Color _borderColor = Color.Black;
    private string _text = "";
    private readonly List<Component> _children = new();
    private Component _baseComponent;

    #endregion
}