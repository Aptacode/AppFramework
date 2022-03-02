using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Aptacode.AppFramework.Behaviours.Transformation;
using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Scene;

public static class SceneExtensions
{
    #region Movement

    public static void Translated(
        this Scene scene,
        ComponentViewModel component,
        Vector2 delta,
        List<ComponentViewModel> movingComponents,
        CancellationTokenSource cancellationToken)
    {
        var unselectedItems = scene.Components.Except(movingComponents)
            .Where(c => c.HasTransformationBehaviour<CollisionBehaviour>());

        var collidingItems = unselectedItems
            .Where(i => i.CollidesWith(component)).ToList();

        movingComponents.AddRange(collidingItems);

        foreach (var collidingItem in collidingItems)
            scene.Translate(collidingItem, delta, movingComponents, cancellationToken);
    }

    public static void Translate(
        this Scene scene,
        ComponentViewModel component,
        Vector2 delta,
        List<ComponentViewModel> movingComponents,
        CancellationTokenSource cancellationToken)
    {
        var unselectedItems = scene.Components.Except(movingComponents)
            .Where(c => c.HasTransformationBehaviour<CollisionBehaviour>());

        component.Translate(delta, false);

        var collidingItems = unselectedItems
            .Where(i => i.CollidesWith(component)).ToList();

        movingComponents.AddRange(collidingItems);

        foreach (var collidingItem in collidingItems)
            scene.Translate(collidingItem, delta, movingComponents, cancellationToken);

        if (cancellationToken.IsCancellationRequested) component.Translate(-delta, false);
    }

    #endregion
}