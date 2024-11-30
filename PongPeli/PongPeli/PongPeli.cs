using Jypeli;

namespace PongPeli;

/// @author Joosua Virtanen
/// @version 14.11.2024
/// <summary>
/// Klassinen pong peli.
/// </summary>
public class PongPeli : PhysicsGame
{
    private readonly Vector nopeusYlos = new Vector(0, 200);
    private readonly Vector nopeusAlas = new Vector(0, -200);

    public override void Begin()
    {
        PhysicsObject pallo = LuoPallo(this, -200, 0);
        PhysicsObject maila1 = LuoMaila(this,Level.Left + 20.0, 0.0);
        PhysicsObject maila2 = LuoMaila(this,Level.Right - 20.0, 0.0);


        LuoKentta();
        AsetaOhjaimet(maila1, maila2);
        AloitaPeli(pallo);
    }

    private void LuoKentta()
    {
        Level.Background.Color = Color.Lime;
        Level.CreateBorders(1.0, false);
        IntMeter pelaajan1Pisteet = LuoPisteLaskuri(this, Screen.Left + 800.0, Screen.Top - 200.0);
        IntMeter pelaajan2Pisteet = LuoPisteLaskuri(this, Screen.Right + 800.0, Screen.Top - 100.0);

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

    public static PhysicsObject LuoMaila(PhysicsGame peli, double x, double y)
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(20.0, 100.0, Shape.Rectangle);
        maila.X = x;
        maila.Y = y;
        maila.Color = Color.Black;
        maila.Restitution = 1.0;
        peli.Add(maila);
        return maila;
    }

    public static IntMeter LuoPisteLaskuri(PhysicsGame peli, double y, double x)
    {
        IntMeter laskuri = new IntMeter(0);
        laskuri.MaxValue = 10;

        Label naytto = new Label();
        naytto.X = x;
        naytto.Y = y;
        naytto.TextColor = Color.Black;
        naytto.BorderColor = peli.Level.BackgroundColor;
        naytto.Color = peli.Level.BackgroundColor;

        naytto.BindTo(laskuri);
        peli.Add(naytto);

        return laskuri;
    }

    private void AsetaOhjaimet(PhysicsObject maila1, PhysicsObject maila2)
    {
        Keyboard.Listen(Key.W, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liiuta mailaa ylös", maila1, nopeusYlos);
        Keyboard.Listen(Key.W, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);
        Keyboard.Listen(Key.S, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa alas", maila1, nopeusAlas);
        Keyboard.Listen(Key.S, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);

        Keyboard.Listen(Key.Up, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liiuta mailaa ylös", maila2, nopeusYlos);
        Keyboard.Listen(Key.Up, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);
        Keyboard.Listen(Key.Down, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa alas", maila2, nopeusAlas);
        Keyboard.Listen(Key.Down, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
    }

    private void AsetaNopeus(PhysicsObject maila, Vector nopeus)
    {
        if ((nopeus.Y < 0) && (maila.Bottom < Level.Bottom))
        {
            maila.Velocity = Vector.Zero;
            return;
        }
        if ((nopeus.Y > 0) && (maila.Top > Level.Top))
        {
            maila.Velocity = Vector.Zero;
            return;
        }

        maila.Velocity = nopeus;
    }

    private static void AloitaPeli(PhysicsObject pallo)
    {
        Vector impulssi = new Vector(500.0, 0.0);
        pallo.Hit(impulssi * pallo.Mass);
    }
}