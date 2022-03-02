using System;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Scene;
using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Aptacode.AppFramework.Views;

public class SceneControllerViewBase : ComponentBase
{
    #region Lifecycle

    [JSInvokable]
    public void GameLoop(float timeStamp)
    {
        ViewModel.Tick(timeStamp);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Console.WriteLine($"Register canvas for scene: {ViewModel.Scene.Id}");
            await BlazorCanvas.Register(ViewModel.Scene.Id.ToString(), Canvas);
            await JsRuntime.InvokeAsync<object>("initGame", DotNetObjectReference.Create(this));
            await JsRuntime.InvokeVoidAsync("SetFocusToElement", Container);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    #region Events

    public void MouseDown(MouseEventArgs e)
    {
        ViewModel?.UserInteractionController.MouseDown(e.FromScale());
    }

    public void MouseUp(MouseEventArgs e)
    {
        ViewModel?.UserInteractionController.MouseUp(e.FromScale());
    }

    public void MouseOut(MouseEventArgs e)
    {
    }

    public void MouseMove(MouseEventArgs e)
    {
        ViewModel?.UserInteractionController.MouseMove(e.FromScale());
    }

    public void KeyDown(KeyboardEventArgs e)
    {
        ViewModel?.UserInteractionController.KeyDown(e.Key);
    }

    public void KeyUp(KeyboardEventArgs e)
    {
        ViewModel?.UserInteractionController.KeyUp(e.Key);
    }

    #endregion

    #region Dependencies

    [Inject] private IJSRuntime JsRuntime { get; set; }
    [Inject] public BlazorCanvasInterop BlazorCanvas { get; set; }

    #endregion

    #region Properties

    private SceneController _viewModel;

    [Parameter]
    public SceneController ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            StateHasChanged();
        }
    }

    protected ElementReference Container;

    public ElementReference Canvas { get; set; }

    public string Style { get; set; } =
        "position: absolute; "; //-moz-transform: scale({SceneScale.Value}); -moz-transform-origin: 0 0; zoom: {SceneScale.Value};";

    #endregion
}