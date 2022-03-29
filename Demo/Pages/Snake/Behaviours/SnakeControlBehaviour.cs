using Aptacode.AppFramework.Demo.Pages.Snake.States;
using Aptacode.AppFramework.Plugins.Behaviours;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Demo.Pages.Snake.Behaviours;

public class SnakeControlBehaviour : BehaviourPlugin<UiEvent>
{
    public static string BehaviourName = "SnakeControl";

    public SnakeControlBehaviour(Scene.Scene scene) : base(scene)
    {
    }

    public override bool Handle(UiEvent uiEvent)
    {
        if (uiEvent is not KeyDownEvent keyEvent)
        {
            return false;
        }

        var state = Scene.Plugins.State.Get<SnakeState>(SnakeState.StateName);
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

    public override string Name()
    {
        return BehaviourName;
    }
}