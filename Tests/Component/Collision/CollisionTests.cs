using Xunit;

namespace Tests.Component.Collision;

public class CollisionTests
{
    [Theory]
    [ClassData(typeof(ComponentComponentCollisionTestDataGenerator))]
    public void ComponentComponentCollision(Aptacode.AppFramework.Components.Component a,
        Aptacode.AppFramework.Components.Component b, bool shouldCollide)
    {
        //Arrange
        //Act
        var sut = a.CollidesWith(b);

        //Assert
        Assert.Equal(shouldCollide, sut);
    }
}