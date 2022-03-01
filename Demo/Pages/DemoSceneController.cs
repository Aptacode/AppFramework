using System.Numerics;
using Aptacode.AppFramework.Scene;

namespace Aptacode.AppFramework.Demo.Pages;

public class DemoSceneController : SceneController
{
    public DemoSceneController(Vector2 size) : base(size)
    {
        ShowGrid = true;
    }
}