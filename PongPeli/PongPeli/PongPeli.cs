using System.Runtime.InteropServices;
using System.Threading;
using Jypeli;

namespace PongPeli;

/// @author Joosua Virtanen
/// @version 14.11.2024
/// <summary>
/// Klassinen pong peli.
/// </summary>
public class PongPeli : PhysicsGame
{
    public override void Begin()
    {
        PhysicsObject pallo = LuoPallo(this, -200, 0);

        LuoKentta();
        AloitaPeli(pallo);

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    private void LuoKentta()
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(20.0, 100.0, Shape.Rectangle);
        maila.Color = Color.Black;
        maila.X = Level.Left + 20.0;
        maila.Y = 0.0;
        maila.Restitution = 1.0;
        Add(maila);

        Level.Background.Color = Color.Lime;
        Level.CreateBorders(1.0, false);
        
        Camera.ZoomToLevel();
    }

    private static PhysicsObject LuoPallo(PhysicsGame peli, double x, double y)
    {
        PhysicsObject pallo = new PhysicsObject(40.0, 40.0, Shape.Circle);
        pallo.Color = Color.Black;
        pallo.X = -200.0;
        pallo.Y = 0.0;
        pallo.Restitution = 1.0;
        pallo.KineticFriction = 0.0;
        pallo.MomentOfInertia = double.PositiveInfinity;
        peli.Add(pallo);
        return pallo;
    }

    private static void AloitaPeli(PhysicsObject pallo)
    {
        Vector impulssi = new Vector(500.0, 0.0);
        pallo.Hit(impulssi * pallo.Mass);
    }
}