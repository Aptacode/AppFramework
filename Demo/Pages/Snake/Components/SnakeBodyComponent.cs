using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Demo.Pages.Snake.States;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Demo.Pages.Snake.Components;

public sealed class SnakeBodyComponent : PolygonComponent
{
    public SnakeBodyComponent(Polygon primitive) : base(primitive)
    {
    }

    public Direction Direction { get; set; }
}