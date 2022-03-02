using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Collision.Rectangles;

namespace Aptacode.AppFramework.Scene;

public class SceneRenderer
{
    #region Ctor

    public SceneRenderer(BlazorCanvasInterop canvas, SceneController scene)
    {
        _canvas = canvas;
        _sceneController = scene;
    }

    #endregion

    #region Props

    private readonly BlazorCanvasInterop _canvas;
    private readonly SceneController _sceneController;

    #endregion

    #region Redraw

    public void Redraw()
    {
        foreach (var scene in _sceneController.Scenes)
        {
            _canvas.SelectCanvas(scene.Id.ToString());
            _canvas.FillStyle(ComponentViewModel.DefaultFillColor);
            _canvas.StrokeStyle(ComponentViewModel.DefaultBorderColor);
            _canvas.LineWidth(ComponentViewModel.DefaultBorderThickness);
            _canvas.ClearRect(0, 0, scene.Size.X * SceneScale.Value, scene.Size.Y * SceneScale.Value);

            for (var i = 0; i < scene.Components.Count(); i++) scene.Components.ElementAt(i).Draw(scene, _canvas);

            //var invalidatedItems = await InvalidateItems();

            //for (var i = 0; i < invalidatedItems.Count; i++)
            //{
            //    var component = invalidatedItems[i];
            //    await component.Draw(_canvas);
            //}
        }
    }

    public async Task<List<ComponentViewModel>> InvalidateItems(Scene scene)
    {
        var validItems = new List<ComponentViewModel>();
        var invalidItems = new List<ComponentViewModel>();

        for (var i = 0; i < scene.Components.Count(); i++)
        {
            var component = scene.Components.ElementAt(i);
            if (component.Invalidated)
                invalidItems.Add(component);
            else
                validItems.Add(component);
        }


        for (var invalidItemIndex = 0; invalidItemIndex < invalidItems.Count; invalidItemIndex++)
        {
            var invalidItem = invalidItems[invalidItemIndex];
            var thickness = invalidItem.BorderThickness;

            var invalidItemBorder = new Vector2(thickness);
            var oldBoundingRecWithBorder = new BoundingRectangle(
                invalidItem.OldBoundingRectangle.TopLeft - 4 * invalidItemBorder,
                invalidItem.OldBoundingRectangle.BottomRight + 8 * invalidItemBorder);
            var newBoundingRecWithBorder = new BoundingRectangle(
                invalidItem.BoundingPrimitive.BoundingRectangle.TopLeft - 4 * invalidItemBorder,
                invalidItem.BoundingPrimitive.BoundingRectangle.BottomRight + 8 * invalidItemBorder);


            for (var validItemIndex = 0; validItemIndex < validItems.Count;)
            {
                var validComponent = validItems[validItemIndex];
                var validThickness = validComponent.BorderThickness;

                var validItemBorder = new Vector2(validThickness);

                var newValidBoundingRect = new BoundingRectangle(
                    validComponent.BoundingPrimitive.BoundingRectangle.TopLeft - 4 * validItemBorder,
                    validComponent.BoundingPrimitive.BoundingRectangle.BottomRight + 8 * validItemBorder);

                if (oldBoundingRecWithBorder.CollidesWith(newValidBoundingRect) ||
                    newBoundingRecWithBorder.CollidesWith(newValidBoundingRect)
                   )
                {
                    validComponent.Invalidated = true;
                    validItems.RemoveAt(validItemIndex);
                    invalidItems.Add(validComponent);
                }
                else
                {
                    validItemIndex++;
                }
            }

            await Invalidate(invalidItem.OldBoundingRectangle, thickness);
            await Invalidate(invalidItem.BoundingPrimitive.BoundingRectangle, thickness);
        }

        return invalidItems;
    }

    public async Task Invalidate(BoundingRectangle rectangle, float border)
    {
        _canvas.ClearRect((rectangle.TopLeft.X - 4 * border) * SceneScale.Value,
            (rectangle.TopLeft.Y - 4 * border) * SceneScale.Value,
            (rectangle.Size.X + 8 * border) * SceneScale.Value, (rectangle.Size.Y + 8 * border) * SceneScale.Value);
    }

    #endregion
}