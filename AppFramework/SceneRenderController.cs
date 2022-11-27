using System.Linq;
using Aptacode.AppFramework.Components;
using Aptacode.BlazorCanvas;

namespace Aptacode.AppFramework;

public class SceneRenderController
{

    #region Events

    private float _lastTimeStamp = -1;

    public virtual void Tick(float timestamp)
    {
        if (Scene == null || _canvas == null)
        {
            return;
        }

        if (_lastTimeStamp == -1)
        {
            _lastTimeStamp = timestamp;

            return;
        }

        var delta = timestamp - _lastTimeStamp;
        _lastTimeStamp = timestamp;

        //Execute behaviors
        Scene.Handle(delta);

        //Reset canvas
        _canvas.FillStyle(Component.DefaultFillColor);
        _canvas.StrokeStyle(Component.DefaultBorderColor);
        _canvas.LineWidth(Component.DefaultBorderThickness);
        _canvas.ClearRect(0, 0, Scene.Size.X, Scene.Size.Y);
        _canvas.Transform(1, 0, 0, -1, 0, Scene.Size.Y);

        //Draw each element
        for (var i = 0; i < Scene.Components.Count(); i++)
        {
            Scene.Components[i].Draw(Scene, _canvas);
        }

        //Flip canvas
        _canvas.Transform(1, 0, 0, -1, 0, Scene.Size.Y);
    }

    #endregion

    #region Properties

    public Scene Scene { get; set; } = null;
    public string Cursor { get; set; }
    public bool ShowGrid { get; set; }

    private BlazorCanvas.BlazorCanvas _canvas = null;


    public void Setup(Scene scene, BlazorCanvas.BlazorCanvas canvas)
    {
        Scene = scene;
        _canvas = canvas;
    }
    #endregion
}