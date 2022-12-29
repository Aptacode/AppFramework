using System;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Demo.Pages.Physics.Behaviours;
using Aptacode.AppFramework.Demo.Pages.Physics.States;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Demo.Pages.Physics;

public class PhysicsScene : Scene
{
    public override Task Setup()
    {
        //Scene
        Plugins.Add(new ScenePhysicsBehaviour(this) { Gravity = true, Friction = false });

        var random = new Random();
        int size = 10;
        for (int i = 0; i < 300; i++)
        {
            var posX = random.Next(0 + size, 1000 - size);
            var posY = random.Next(0 + size, 1000 - size);
            var b = new CircleComponent(new Vector2(posX, posY), size);
            b.AddPhysics(this).SetHorizontalVelocity(-2, 2);//.SetVerticalVelocity(-2, 2);
            //b.AddDragToMove(this);
            Add(b);
        }

        var bottom = new Polygon(100, 100, 900, 100, 1000, 0, 0, 0).ToComponent();

        var right = new Polygon(900, 100, 900, 900, 1000, 1000, 1000, 0).ToComponent();

        var top = new Polygon(900, 900, 100, 900, 0, 1000, 1000, 1000).ToComponent();

        var left = new Polygon(100, 900, 100, 100, 0, 0, 0, 1000).ToComponent();

        top.AddPhysics(this).IsFixed();
        bottom.AddPhysics(this).IsFixed();
        left.AddPhysics(this).IsFixed();
        right.AddPhysics(this).IsFixed();

        Add(top).Add(right).Add(bottom).Add(left);

        return base.Setup();
    }
}
