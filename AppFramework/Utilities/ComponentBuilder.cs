using System.Collections.Generic;
using System.Drawing;
using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Utilities;

public class ComponentBuilder
{
    public ComponentBuilder SetBorderThickness(float borderThickness)
    {
        _borderThickness = borderThickness;
        return this;
    }

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

    public ComponentBuilder SetMargin(float margin)
    {
        _margin = margin;
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
        component.BorderColor = _borderColor;
        component.FillColor = _fillColor;
        component.BorderThickness = _borderThickness;
        component.Text = _text;
        component.Margin = _margin;

        component.AddRange(_children);

        Reset();
        return component;
    }

    public void Reset()
    {
        _baseComponent = null;
        _children.Clear();
        _borderColor = Color.Black;
        _fillColor = Color.White;
        _borderThickness = Component.DefaultBorderThickness;
        _margin = Component.DefaultMargin;
        _text = "";
    }

    #region Ctor

    #endregion

    #region Properties

    private Color _fillColor = Color.White;
    private Color _borderColor = Color.Black;
    private float _borderThickness = Component.DefaultBorderThickness;
    private float _margin = Component.DefaultMargin;
    private string _text = "";
    private readonly List<Component> _children = new();
    private Component _baseComponent;

    #endregion
}