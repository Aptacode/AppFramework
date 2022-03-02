using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Behaviours.Transformation;

public class CollisionBehaviour : TransformationBehaviour
{
    public CollisionBehaviour(Scene.Scene scene, ComponentViewModel component) : base(scene, component, nameof(CollisionBehaviour))
    {
    }

    public override bool HandleEvent(TransformationEvent transformationEvent)
    {
        if (!Enabled) return false;

        if (transformationEvent is not TranslateEvent { Source: true } translationEvent) return false;

        Move(new List<ComponentViewModel>() {Component}, translationEvent.Delta);

        return true;
    }

    protected void Move(List<ComponentViewModel> movedComponents, Vector2 delta)
    {
        var componentsToMove = Scene.Components
            .Except(movedComponents)
            .Where(c =>
                c.HasTransformationBehaviour<CollisionBehaviour>() &&
                c.CollidesWith(Component)
            ).ToList();

        movedComponents.AddRange(componentsToMove);

        foreach (var component in componentsToMove)
        {
            component.Translate(delta, false);
            component.GetTransformationBehaviour<CollisionBehaviour>()?.Move(movedComponents, delta);
        }
    }
}