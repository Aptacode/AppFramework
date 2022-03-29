using Aptacode.AppFramework.Components.Behaviours.Scene;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Demo.Pages.Snake;

public class SnakeControlBehaviour : SceneUiBehaviour
{
    public SnakeControlBehaviour(Scene.Scene scene) : base(scene)
    {
    }

    public override bool Handle(UiEvent uiEvent)
    {
        if (uiEvent is not KeyDownEvent keyEvent)
        {
            return false;
        }

        var state = Scene.GetState<SnakeState>();
        if (state == null)
        {
            return false;
        }

        state.SnakeHead.Direction = keyEvent.Key switch
        {
            "ArrowUp" => Direction.Up,
            "ArrowDown" => Direction.Down,
            "ArrowLeft" => Direction.Left,
            "ArrowRight" => Direction.Right,
            _ => state.SnakeHead.Direction
        };

        return false;
    }

    #region Properties

    #endregion
}