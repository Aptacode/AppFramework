using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.Geometry.Primitives;

namespace Tests.Component.Collision;

public class ComponentComponentCollisionTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new()
    {
        new object[] { new Point(Vector2.Zero), new Point(Vector2.Zero), true },
        new object[] { new Point(Vector2.One), new Point(Vector2.Zero), false }
    };

    public IEnumerator<object[]> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}