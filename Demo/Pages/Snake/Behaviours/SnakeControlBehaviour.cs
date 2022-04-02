using Aptacode.AppFramework.Demo.Pages.Snake.States;
using Aptacode.AppFramework.Events;
using Aptacode.AppFramework.Plugins;

namespace Aptacode.AppFramework.Demo.Pages.Snake.Behaviours;

public class SnakeControlBehaviour : Plugin
{
    public static string BehaviourName = "SnakeControl";

    public SnakeControlBehaviour(Scene scene) : base(scene)
    {
    }

    public override bool Handle(UiEvent uiEvent)
    {
        if (uiEvent is not KeyDownEvent keyEvent)
        {
            return false;
        }

        var state = Scene.Plugins.Get<SnakeState>(SnakeState.StateName);
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