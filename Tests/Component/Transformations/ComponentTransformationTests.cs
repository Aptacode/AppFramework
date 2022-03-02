using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Scene;
using Aptacode.Geometry.Collision.Rectangles;
using Xunit;

namespace Tests.Component.Transformations;

public class ComponentTransformationTests
{
    [Theory]
    [ClassData(typeof(ComponentTransformationTestDataGenerator))]
    public void ComponentTransformationTest(ComponentViewModel a, Vector2 delta)
    {
        //Arrange
        var scene = new Scene(Vector2.One);
        var expectedBoundingRectangle = a.BoundingPrimitive.BoundingRectangle.Translate(delta);
        //Act
        a.Translate(delta, true);

        //Assert
        Assert.Equal(expectedBoundingRectangle, a.BoundingPrimitive.BoundingRectangle);
    }
}