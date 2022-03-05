using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;
using Xunit;

namespace Tests.Component.Transformations;

public class SceneTests
{
    [Fact]
    public void Scene()
    {
        //Arrange
        var scene = new Scene(new Vector2(100));
        var a = Polygon.Rectangle.FromTwoPoints(Vector2.Zero, Vector2.One).ToComponent();
        var b = Polygon.Rectangle.FromTwoPoints(Vector2.Zero, Vector2.One).ToComponent();
        scene.Add(a);
        scene.Add(b);

        //Act

        //Assert
    }
}