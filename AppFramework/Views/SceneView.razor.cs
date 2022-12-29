using Aptacode.AppFramework.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode.AppFramework.Views;

public class SceneViewBase : ComponentBase, IDisposable
{
    #region Properties

    [Parameter, EditorRequired]
    public int Width { get; set; } = 400;

    [Parameter, EditorRequired]
    public int Height { get; set; } = 400;

    [Parameter, EditorRequired]
    public Scene Scene { get; set; }

    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    protected BlazorCanvas.BlazorCanvas Canvas { get; set; }

    private readonly SceneInteractionController SceneInteractionController = new();

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    public int FPS { get; private set; } = 0;

    private long _lastRender = -1;

    #endregion
    protected override async Task OnInitializedAsync()
    {
        try
        {
            Width = Height = (int)Math.Floor(Width / 100.0) * 100;

            Scene.Width = Width;
            Scene.Height = Height;

            await InvokeAsync(StateHasChanged);

            // Wait for the canvas to be ready
            while (Canvas is not { Ready: true } && !_cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(10, _cancellationTokenSource.Token);
            }

            // Setup the scene
            SceneInteractionController.Scene = Scene;
            await Scene.Setup();

            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(10));
            var watch = new Stopwatch();
            var watch2 = new Stopwatch();
            watch2.Start();
            long last = 0;
            while (await timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
            {
                watch.Restart();

                var d = watch2.ElapsedMilliseconds - last;
                last = watch2.ElapsedMilliseconds;
                await Scene.Loop(d);

                //Clear canvas
                Canvas.ClearRect(0, 0, Width, Height);
                Canvas.Transform(1, 0, 0, -1, 0, Height);

                //Render each component
                for (var i = 0; i < Scene.Components.Count; i++)
                {
                    Scene.Components[i].Draw(Canvas);
                }

                //Flip canvas
                Canvas.Transform(1, 0, 0, -1, 0, Height);

                watch.Stop();
                if (watch.Elapsed.Milliseconds == 0)
                {
                    FPS = 0;
                }
                else
                {
                    FPS = 1000 / watch.Elapsed.Milliseconds;
                }

                await InvokeAsync(StateHasChanged);
            }
        }
        catch (OperationCanceledException)
        {
            // Ignored
        }
        finally
        {
            await Scene.Teardown();
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
    }

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
}