using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.Geometry.Primitives;
using Xunit;

namespace Tests.Scene;

public class SceneTests
{
    [Fact]
    public void Scene()
    {
        //Arrange
        var scene = new Aptacode.AppFramework.Scene();
        var a = Polygon.Rectangle.FromTwoPoints(Vector2.Zero, Vector2.One).ToComponent();
        var b = Polygon.Rectangle.FromTwoPoints(Vector2.Zero, Vector2.One).ToComponent();
        scene.Add(a);
        scene.Add(b);

        //Act

        //Assert
    }
}