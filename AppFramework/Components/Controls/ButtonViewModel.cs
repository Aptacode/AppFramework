using Aptacode.AppFramework.Components.Events;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.Geometry.Primitives.Polygons;

namespace Aptacode.AppFramework.Components.Controls
{
    public class ButtonViewModel : RectangleViewModel
    {
        #region Ctor

        public ButtonViewModel(Rectangle rectangle) : base(rectangle)
        {
        }

        #endregion

        #region Events

        public override bool HandleMouseEvent(BaseMouseEvent mouseEvent)
        {
            return base.HandleMouseEvent(mouseEvent);
        }

        #endregion

        #region Props

        #endregion
    }
}