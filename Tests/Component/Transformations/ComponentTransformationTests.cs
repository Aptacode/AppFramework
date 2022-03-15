using System.Linq;
using System.Numerics;
using Aptacode.Geometry.Collision.Rectangles;
using Xunit;

namespace Tests.Component.Transformations;

public class ComponentTransformationTests
{
    [Theory]
    [ClassData(typeof(ComponentTransformationTestDataGenerator))]
    public void ComponentTranslateBoundsTest(Aptacode.AppFramework.Components.Component a, Vector2 delta)
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
    public void ComponentTranslateChildBoundingRectangleTest(Aptacode.AppFramework.Components.Component a,
        Vector2 delta)
    {
        //Arrange
        var expectedBoundingRectangles = a.Children.Select(c => c.Primitive.BoundingRectangle.Translate(delta));

        //Act
        a.Translate(delta);

        //Assert
        Assert.Equal(a.Children.Select(c => c.Primitive.BoundingRectangle), expectedBoundingRectangles);
    }

    [Theory]
    [ClassData(typeof(ComponentTransformationTestDataGenerator))]
    public void ComponentTranslateInvalidate(Aptacode.AppFramework.Components.Component a, Vector2 delta)
    {
        //Arrange
        //Act
        a.Translate(delta);

        //Assert
        Assert.True(a.Invalidated);
    }
}