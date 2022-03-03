using System.Linq;
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
    public void ComponentTranslateBoundsTest(ComponentViewModel a, Vector2 delta)
    {
        //Arrange
        var expectedBoundingRectangle = a.BoundingPrimitive.BoundingRectangle.Translate(delta);

        //Act
        a.Translate(delta);

        //Assert
        Assert.Equal(expectedBoundingRectangle, a.BoundingPrimitive.BoundingRectangle);
    }

    [Theory]
    [ClassData(typeof(ComponentTransformationTestDataGenerator))]
    public void ComponentTranslateChildrenTest(ComponentViewModel a, Vector2 delta)
    {
        //Arrange
        var expectedBoundingRectangles = a.Children.Select(c => c.Primitive.BoundingRectangle.Translate(delta));

        //Act
        a.Translate(delta);

        //Assert
        Assert.Equal(a.Children.Select(c => c.Primitive.BoundingRectangle), expectedBoundingRectangles);
        Assert.True(a.Children.All(c => c.Invalidated));
    }

    [Theory]
    [ClassData(typeof(ComponentTransformationTestDataGenerator))]
    public void ComponentTranslateInvalidate(ComponentViewModel a, Vector2 delta)
    {
        //Arrange
        var expectedBoundingRectangle = a.BoundingPrimitive.BoundingRectangle.Translate(delta);

        //Act
        a.Translate(delta);

        //Assert
        Assert.Equal(expectedBoundingRectangle, a.BoundingPrimitive.BoundingRectangle);
        Assert.True(a.Invalidated);
    }
}