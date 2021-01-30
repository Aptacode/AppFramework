using System.Collections.Generic;
using System.Numerics;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Controls
{
    public class Dropdown : PolygonViewModel
    {
        #region Properties

        public List<ComponentViewModel> Options { get; set; } = new();

        #endregion

        #region Ctor

        public Dropdown(Polygon polygon) : base(polygon)
        {
        }

        public static Dropdown FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new(Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static Dropdown FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new(Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
        }

        #endregion

        #region Methods

        public override void Add(ComponentViewModel option)
        {
            Options.Add(option);
            base.Add(option);
        }

        public override void Remove(ComponentViewModel option)
        {
            Options.Remove(option);
            base.Remove(option);
        }

        #endregion

        #region Events

        #endregion
    }
}