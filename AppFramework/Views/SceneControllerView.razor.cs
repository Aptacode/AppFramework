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
    [JSInvokable]
    public void GameLoop(float timeStamp)
    {
        ViewModel.Tick();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeAsync<object>("initGame", DotNetObjectReference.Create(this));
            await JsRuntime.InvokeVoidAsync("SetFocusToElement", Container);
        }
    }

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

    #region Properties

    private SceneController _viewModel;

    [Parameter]
    public SceneController ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            ViewModel?.Setup(BlazorCanvas);
            StateHasChanged();
        }
    }

    [Inject] private IJSRuntime JsRuntime { get; set; }


    protected ElementReference Container;

    [Inject] public BlazorCanvasInterop BlazorCanvas { get; set; }

    #endregion
}