using Aptacode.AppFramework.Components;
using Xunit;

namespace Tests.Component.Collision;

public class CollisionTests
{
    [Theory]
    [ClassData(typeof(ComponentComponentCollisionTestDataGenerator))]
    public void ComponentComponentCollision(ComponentViewModel a, ComponentViewModel b, bool shouldCollide)
    {
        //Arrange
        //Act
        var sut = a.CollidesWith(b);

        //Assert
        Assert.Equal(shouldCollide, sut);
    }
}