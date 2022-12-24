using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Aptacode.AppFramework.Views;

public class SceneViewBase : ComponentBase, IDisposable
{
    [Parameter, EditorRequired]
    public Scene Scene { get; set; }

    #region Lifecycle

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    protected override async Task OnInitializedAsync()
    {
        while (Canvas is not { Ready: true })
        {
            await Task.Delay(10);
        }

        SceneInteractionController.Scene = Scene;
        SceneRenderController.Setup(Scene, Canvas);

        try
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(10));
            var watch = new Stopwatch();
            float timeStamp = 0.0f;
            while (await timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
            {
                watch.Restart();
                timeStamp += 0.1f;
                SceneRenderController.Tick(timeStamp);

                watch.Stop();

                await InvokeAsync(StateHasChanged);
            }
        }
        catch (OperationCanceledException)
        {
            // Ignored
        }
        finally
        {

        }
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

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
    }

    #endregion

    #region Dependencies

    [Inject] public SceneInteractionController SceneInteractionController { get; set; }

    #endregion

    #region Properties

    protected ElementReference Container;
    protected BlazorCanvas.BlazorCanvas Canvas { get; set; }
    protected SceneRenderController SceneRenderController { get; set; } = new SceneRenderController();

    public string Style { get; set; } = "position: absolute; ";

    #endregion
}