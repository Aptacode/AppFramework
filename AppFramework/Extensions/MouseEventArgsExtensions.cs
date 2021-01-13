using System.Numerics;
using Aptacode.AppFramework.Utilities;
using Microsoft.AspNetCore.Components.Web;

namespace Aptacode.AppFramework.Extensions
{
    public static class MouseEventArgsExtensions
    {
        public static Vector2 FromScale(this MouseEventArgs args)
        {
            return new(
                (int) (args.OffsetX / SceneScale.Value), (int) (args.OffsetY / SceneScale.Value));
        }
    }
}