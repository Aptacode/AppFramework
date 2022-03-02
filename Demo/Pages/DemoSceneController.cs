using Aptacode.AppFramework.Scene;

namespace Aptacode.AppFramework.Demo.Pages;

public class DemoSceneController : SceneController
{
    public DemoSceneController(Scene.Scene scene) : base(scene)
    {
        ShowGrid = true;
    }
}