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
        PhysicsObject pallo = new PhysicsObject(40.0, 40.0, Shape.Circle);
        pallo.Color = Color.Black;
        Add(pallo);
        
        Vector impulssi = new Vector(500.0, 0.0);
        pallo.Hit(impulssi * pallo.Mass);
        
        pallo.X = -200.0;
        pallo.Y = 0.0;
        pallo.Restitution = 1.0;
        
        Level.Background.Color = Color.Lime;
        Level.CreateBorders(1.0, false);
        
        Camera.ZoomToLevel();
        
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
}