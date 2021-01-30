using System.Numerics;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Controls
{
    public class Button : PolygonViewModel
    {
        #region Ctor

        public Button(Polygon polygon) : base( polygon)
        {
            
        }

        public static Button FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new( Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static Button FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new( Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
        }

        #endregion

    }
}