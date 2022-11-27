using System;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Aptacode.AppFramework.Views;

public class SceneViewBase : ComponentBase
{
    [Parameter, EditorRequired]
    public Scene Scene { get; set; }

    #region Lifecycle

    [JSInvokable]
    public void GameLoop(float timeStamp)
    {
        SceneRenderController.Tick(timeStamp);
    }

    private bool _isSetup;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isSetup && Canvas != null)
        {
            _isSetup = true;

            SceneInteractionController.Scene = Scene;
            SceneRenderController.Setup(Scene, Canvas);
            await JsRuntime.InvokeAsync<object>("initGame", DotNetObjectReference.Create(this));
            await JsRuntime.InvokeVoidAsync("SetFocusToElement", Container);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    #region Events

    public void MouseDown(MouseEventArgs e)
    {
        SceneInteractionController.MouseDown(e.FromScale());
    }

    public void MouseUp(MouseEventArgs e)
    {
        SceneInteractionController.MouseUp(e.FromScale());
    }

    public void MouseOut(MouseEventArgs e)
    {
    }

    public void MouseMove(MouseEventArgs e)
    {
        SceneInteractionController.MouseMove(e.FromScale());
    }

    public void KeyDown(KeyboardEventArgs e)
    {
        SceneInteractionController.KeyDown(e.Key);
    }

    public void KeyUp(KeyboardEventArgs e)
    {
        SceneInteractionController.KeyUp(e.Key);
    }

    #endregion

    #region Dependencies

    [Inject] private IJSRuntime JsRuntime { get; set; }
    [Inject] public SceneInteractionController SceneInteractionController { get; set; }

    #endregion

    #region Properties

    protected ElementReference Container;
    protected BlazorCanvas.BlazorCanvas Canvas { get; set; }
    protected SceneRenderController SceneRenderController { get; set; } = new SceneRenderController();

    public string Style { get; set; } = "position: absolute; ";

    #endregion
}