using System;
using System.Collections.Generic;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.CSharp.Common.Utilities.Mvvm;

namespace Aptacode.AppFramework.Scene
{
    public class Scene : BindableBase
    {
        #region Ctor

        public Scene(Vector2 size)
        {
            Size = size;
        }

        #endregion

        #region Properties

        private readonly List<ComponentViewModel> _components = new();

        public Vector2 Size { get; init; }
        public Guid Id { get; init; } = Guid.NewGuid();

        #endregion

        #region Components

        public void Add(ComponentViewModel component)
        {
            _components.Add(component);
            component.OnDrag += Component_OnDrag;
            component.OnDrop += Component_OnDrop;
            OnComponentAdded?.Invoke(this, component);
        }

        #region Events

        public void Handle(KeyboardEvent e)
        {
            foreach (var component in Components)
            {
                component.HandleKeyboardEvent(e);
            }
        }

        public void Handle(MouseEvent e)
        {
            foreach (var component in Components)
            {
                component.HandleMouseEvent(e);
            }

            if(IsDragging)
            {
                DragEvent.Component.Handle(e);
            }
        }

        #endregion

        #region Drag Drop
        public bool IsDragging { get; set; }
        public DragEvent DragEvent { get; set; }
        public ComponentViewModel DragComponentSource { get; set; }
        private void Component_OnDrag(object sender, Events.DragEvent e)
        {
            Console.WriteLine($"OnDrag: {e.Component.Id}");
            IsDragging = true;
            DragEvent = e;
            DragComponentSource = e.Component.Parent;
        }

        private void Component_OnDrop(object sender, Events.DropEvent e)
        {
            Console.WriteLine($"OnDrop: {e.Component.Id}");

            IsDragging = false;

            foreach (var component in Components)
            {
                if (component.Accepts(e))
                {
                    return;
                }
            }

            DragComponentSource.Accepts(new DropFailedEvent(DragEvent.Component, DragEvent.Position, e.Position));
        }

        #endregion

        public void AddRange(IEnumerable<ComponentViewModel> components)
        {
            foreach (var component in components)
            {
                Add(component);
            }
        }

        public bool Remove(ComponentViewModel component)
        {
            var success = _components.Remove(component);
            component.OnDrag -= Component_OnDrag;
            component.OnDrop -= Component_OnDrop;
            OnComponentRemoved?.Invoke(this, component);
            return success;
        }

        public IEnumerable<ComponentViewModel> Components => _components;

        #endregion

        #region Events

        public event EventHandler<ComponentViewModel> OnComponentAdded;
        public event EventHandler<ComponentViewModel> OnComponentRemoved;

        #endregion

        #region Layering

        public void BringToFront(ComponentViewModel componentViewModel)
        {
            if (!_components.Remove(componentViewModel))
            {
                return;
            }

            _components.Add(componentViewModel);
        }

        public void SendToBack(ComponentViewModel componentViewModel)
        {
            if (!_components.Remove(componentViewModel))
            {
                return;
            }

            _components.Insert(0, componentViewModel);
        }

        public void BringForward(ComponentViewModel componentViewModel)
        {
            var index = _components.IndexOf(componentViewModel);
            if (index == _components.Count - 1)
            {
                return;
            }

            _components.RemoveAt(index);
            _components.Insert(index + 1, componentViewModel);
        }

        public void SendBackward(ComponentViewModel componentViewModel)
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
}