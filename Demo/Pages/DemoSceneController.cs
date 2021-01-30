using System.Numerics;
using Aptacode.AppFramework.Scene;
using Aptacode.AppFramework.Utilities;

namespace Aptacode.AppFramework.Demo.Pages
{
    public class DemoSceneController : SceneController
    {
        private readonly ComponentBuilder _componentBuilder = new();

        public DemoSceneController(Vector2 size) : base(size)
        {
            ShowGrid = true;
        }
    }
}