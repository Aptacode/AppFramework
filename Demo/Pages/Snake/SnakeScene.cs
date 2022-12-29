using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Demo.Pages.Snake.Behaviours;
using Aptacode.AppFramework.Demo.Pages.Snake.Components;
using Aptacode.AppFramework.Demo.Pages.Snake.States;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Demo.Pages.Snake;

public class SnakeScene : Scene
{
    public SnakeState SnakeState { get; set; }
    public override Task Setup()
    {
        SnakeState = new SnakeState(this);
        Plugins.Add(SnakeState);
        Plugins.Add(new SnakeControlBehaviour(this));
        Plugins.Add(new SnakeMovementBehaviour(this));

        SnakeState.SnakeHead =
           new SnakeBodyComponent(Polygon.Rectangle.FromTwoPoints(-SnakeGameConfig.InnerCellSize / 2, SnakeGameConfig.InnerCellSize / 2))
           {
               Direction = Direction.Up
           };
        Add(SnakeState.SnakeHead);

        SnakeState.SnakeFood = new SnakeFoodComponent(Polygon.Rectangle.FromTwoPoints(-SnakeGameConfig.InnerCellSize / 2, SnakeGameConfig.InnerCellSize / 2));
        Add(SnakeState.SnakeFood);

        var thickness = 10.0f;
        var bottom = new Polygon(thickness, thickness, SnakeGameConfig.BoardSize.X - thickness, thickness,
            SnakeGameConfig.BoardSize.X, 0, 0, 0).ToComponent();

        var right = new Polygon(SnakeGameConfig.BoardSize.X - thickness, thickness,
            SnakeGameConfig.BoardSize.X - thickness, SnakeGameConfig.BoardSize.Y - thickness,
            SnakeGameConfig.BoardSize.X, SnakeGameConfig.BoardSize.Y, SnakeGameConfig.BoardSize.X, 0).ToComponent();

        var top = new Polygon(SnakeGameConfig.BoardSize.X - thickness, SnakeGameConfig.BoardSize.X - thickness,
            thickness, SnakeGameConfig.BoardSize.X - thickness, 0, SnakeGameConfig.BoardSize.Y,
            SnakeGameConfig.BoardSize.X, SnakeGameConfig.BoardSize.Y).ToComponent();

        var left = new Polygon(thickness, SnakeGameConfig.BoardSize.X - thickness, thickness, thickness, 0, 0, 0,
            SnakeGameConfig.BoardSize.Y).ToComponent();

        Add(top).Add(right).Add(bottom).Add(left);
        SnakeState.Walls.Add(top);
        SnakeState.Walls.Add(right);
        SnakeState.Walls.Add(bottom);
        SnakeState.Walls.Add(left);
        return Task.CompletedTask;
    }
}