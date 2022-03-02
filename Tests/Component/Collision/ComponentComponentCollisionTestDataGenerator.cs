using System.Collections;
using System.Collections.Generic;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.Geometry.Primitives;

namespace Tests.Component.Collision;

public class ComponentComponentCollisionTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new()
    {
        new object[] { Point.Zero.ToViewModel(), Point.Zero.ToViewModel(), true },
        new object[] { Point.Unit.ToViewModel(), Point.Zero.ToViewModel(), false },
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