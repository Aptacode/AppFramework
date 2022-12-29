using System;
using System.Collections.Generic;
using System.Numerics;
using Aptacode.AppFramework.Events;
using Aptacode.AppFramework.Plugins;

namespace Aptacode.AppFramework.Components;
public abstract class Component
{
    #region Children
    public List<Component> Children { get; private set; }
    public Component? Parent
    {
        get => parent; protected set
        {
            parent = value;
        }
    }
    public bool ParentMatrixChanged { get; set; }

    public void AddChild(Component child)
    {
        if (Children == null)
        {
            Children = new List<Component>();
        }

        child.Parent = this;
        Children.Add(child);
    }

    public void RemoveChild(Component child)
    {
        if (Children == null)
        {
            return;
        }

        child.Parent = null;
        Children.Remove(child);
    }

    #endregion

    #region Plugins
    public PluginCollection Plugins { get; private set; }

    public void AddPlugin(Plugin plugin)
    {
        if (Plugins == null)
        {
            Plugins = new PluginCollection();
        }

        Plugins.Add(plugin);
    }

    public void RemovePlugin(Plugin plugin)
    {
        if (Plugins == null)
        {
            return;
        }

        Plugins.Remove(plugin);
    }

    #endregion

    #region Transformations
    private Matrix3x2 scaleMatrix = Matrix3x2.Identity;
    private Matrix3x2 rotationMatrix = Matrix3x2.Identity;
    private Matrix3x2 translationMatrix = Matrix3x2.Identity;
    private Component parent;
    private Matrix3x2 matrix = Matrix3x2.Identity;
    private Matrix3x2 transformedMatrix = Matrix3x2.Identity;
    private bool matrixChanged = true;
    public bool HasMatrixChanged => matrixChanged || ParentMatrixChanged;
    public Matrix3x2 TransformedMatrix
    {
        get
        {
            if (!HasMatrixChanged)
            {
                return transformedMatrix;
            }

            if (matrixChanged)
            {
                matrixChanged = false;
                matrix = TranslationMatrix * RotationMatrix * ScaleMatrix;
            }

            transformedMatrixChanged = true;
            if(Parent == null)
            {
                transformedMatrix = matrix;
            }
            else
            {
                transformedMatrix = Parent.TransformedMatrix * matrix;
            }

            ParentMatrixChanged = false;
            if (Children != null)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].ParentMatrixChanged = true;
                }
            }

            return transformedMatrix;
        }
    }

    protected bool transformedMatrixChanged = true;

    public Matrix3x2 TranslationMatrix
    {
        get => translationMatrix; set
        {
            translationMatrix = value;
            matrixChanged = true;
        }
    }
    public Matrix3x2 RotationMatrix
    {
        get => rotationMatrix; set
        {
            rotationMatrix = value;
            matrixChanged = true;
        }
    }
    public Matrix3x2 ScaleMatrix
    {
        get => scaleMatrix; set
        {
            scaleMatrix = value;
            matrixChanged = true;
        }
    }

    public void AddTranslation(Vector2 position)
    {
        TranslationMatrix *= Matrix3x2.CreateTranslation(position);
    }
    public void SetTranslation(Vector2 position)
    {
        TranslationMatrix = Matrix3x2.CreateTranslation(position);
    }
    public void AddRotation(float radians)
    {
        RotationMatrix *= Matrix3x2.CreateRotation(radians);
    }
    public void SetRotation(float radians)
    {
        RotationMatrix = Matrix3x2.CreateRotation(radians);
    }

    #endregion

    #region Render
    public virtual void Draw(BlazorCanvas.BlazorCanvas x)
    {
        Render(x);

        if (Children != null)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Draw(x);
            }
        }
    }

    public abstract void Render(BlazorCanvas.BlazorCanvas ctx);

    #endregion

    #region Events

    //Events
    public event EventHandler<UiEvent> OnUiEvent;
    public event EventHandler<UiEvent> OnUiEventTunneled;
    public event EventHandler<TransformationEvent> OnTransformationEvent;

    public virtual bool Handle(UiEvent uiEvent)
    {
        //Firstly try and handle the event with the most nested child component
        var isBubbleHandled = false;
        if (Children != null)
        {
            foreach (var child in Children)
            {
                if (child.Handle(uiEvent))
                {
                    isBubbleHandled = true; //The child handles the event
                }
            }
        }

        //Try and handle the event with this component
        if (Plugins?.Handle(uiEvent) != true)
        {
            return false;
        }

        OnUiEventTunneled?.Invoke(this, uiEvent);

        if (!isBubbleHandled)
        {
            OnUiEvent?.Invoke(this, uiEvent);
        }

        return true;
    }

    #endregion
}