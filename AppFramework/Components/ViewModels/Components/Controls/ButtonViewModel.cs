using Aptacode.AppFramework.Components.ViewModels.Components.Events;
using Aptacode.AppFramework.Components.ViewModels.Components.Primitives;
using Aptacode.Geometry.Primitives.Polygons;

namespace Aptacode.AppFramework.Components.ViewModels.Components.Controls
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