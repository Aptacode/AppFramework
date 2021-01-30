using System.Numerics;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Controls
{
    public class TextBox : PolygonViewModel
    {
        #region Ctor

        public TextBox(Polygon polygon) : base( polygon)
        {
            OnHasFocusChanged += Handle_OnHasFocusChanged;
            OnKeyboardEventTunneled += Handle_OnKeyboardEventTunneled;
        }

        public static TextBox FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new( Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static TextBox FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new( Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
        }

        #endregion

        #region Properties

        public bool IsTyping { get; set; }

        #endregion

        #region Events
        
        private void Handle_OnHasFocusChanged(object? sender, bool e)
        {
            IsTyping = e;
        }

        private void Handle_OnKeyboardEventTunneled(object? sender, KeyboardEvent e)
        {
            if (e is KeyDownEvent keyDown)
            {
                switch (e.Key.ToLower())
                {
                    case "backspace":
                        if (Text.Length >= 1)
                        {
                            Text = Text[0..^1];
                        }
                        break;
                    default:
                        if (e.Key.Length == 1)
                        {
                            Text += e.Key;
                        }
                        break;
                }
            }

        }

        #endregion

    }
}