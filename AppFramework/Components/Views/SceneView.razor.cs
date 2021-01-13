using System.Threading.Tasks;
using Aptacode.AppFramework.Components.ViewModels;
using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Components.Views
{
    public class SceneViewBase : ComponentBase
    {
        #region Lifecycle

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await BlazorCanvas.Register(Canvas);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        #endregion

        #region Properties

        [Inject] public BlazorCanvasInterop BlazorCanvas { get; set; }

        [Parameter] public Scene ViewModel { get; set; }

        public ElementReference Canvas { get; set; }

        public string Style { get; set; } =
            "position: absolute; "; //-moz-transform: scale({SceneScale.Value}); -moz-transform-origin: 0 0; zoom: {SceneScale.Value};";

        #endregion
    }
}