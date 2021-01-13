using Aptacode.AppFramework.Scene;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;

namespace Aptacode.AppFramework.Demo.Pages
{
    public class DemoSceneController : SceneController
    {
        private readonly ComponentBuilder _componentBuilder = new();

        public DemoSceneController(Scene.Scene scene) : base(scene)
        {
        }
    }
}