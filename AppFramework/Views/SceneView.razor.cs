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
    private Scene _scene;

    [Parameter]
    public Scene Scene
    {
        get => _scene;
        set
        {
            _scene = value;
            SceneRenderController.Scene = _scene;
            SceneInteractionController.Scene = _scene;
        }
    }

    #region Lifecycle

    [JSInvokable]
    public void GameLoop(float timeStamp)
    {
        SceneRenderController.Tick(timeStamp);
    }

    private bool _isSetup;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isSetup && Scene != null)
        {
            _isSetup = true;
            Console.WriteLine($"Register canvas for scene: {Scene.Id}");
            await BlazorCanvas.Register(Scene.Id.ToString(), Canvas);
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
    [Inject] public BlazorCanvasInterop BlazorCanvas { get; set; }
    [Inject] public SceneRenderController SceneRenderController { get; set; }
    [Inject] public SceneInteractionController SceneInteractionController { get; set; }

    #endregion

    #region Properties

    protected ElementReference Container;

    public ElementReference Canvas { get; set; }

    public string Style { get; set; } = "position: absolute; ";

    #endregion
}