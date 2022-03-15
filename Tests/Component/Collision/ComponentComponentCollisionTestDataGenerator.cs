using System.Collections;
using System.Collections.Generic;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.Geometry.Primitives;

namespace Tests.Component.Collision;

public class ComponentComponentCollisionTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new()
    {
        new object[] { Point.Zero.ToComponent(), Point.Zero.ToComponent(), true },
        new object[] { Point.Unit.ToComponent(), Point.Zero.ToComponent(), false }
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