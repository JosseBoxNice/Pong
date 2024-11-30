using System.Runtime;
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
    private readonly Vector nopeusYlos = new Vector(0, 400);
    private readonly Vector nopeusAlas = new Vector(0, -400);
    private PhysicsObject pallo;
    private PhysicsObject maila1;
    public override void Begin()
    {
        pallo = LuoPallo(this, -200, 0.0);
        maila1 = LuoMaila(this,Level.Left + 20.0, 0.0);
        PhysicsObject maila2 = LuoMaila(this,Level.Right - 20.0, 0.0);

        LuoKenttaJaAsetaTormaykset(pallo);
        AsetaOhjaimet(maila1, maila2);
        AloitaPeli(pallo);
    }

    protected override void Update(Time time)
    {
        maila1.Position = pallo.Position;
    }

    private void LuoKenttaJaAsetaTormaykset(PhysicsObject pallo)
    {
        Level.Background.Color = Color.Lime;
        
        PhysicsObject vasenReuna = Level.CreateLeftBorder();
        vasenReuna.KineticFriction = 0.0;
        vasenReuna.Restitution = 1.0;
        vasenReuna.IsVisible = false;

        PhysicsObject oikeaReuna = Level.CreateRightBorder();
        oikeaReuna.KineticFriction = 0.0;
        oikeaReuna.Restitution = 1.0;
        oikeaReuna.IsVisible = false;

        PhysicsObject alaReuna = Level.CreateBottomBorder();
        alaReuna.KineticFriction = 0.0;
        alaReuna.Restitution = 1.0;
        alaReuna.IsVisible = false;

        PhysicsObject ylaReuna = Level.CreateTopBorder();
        ylaReuna.KineticFriction = 0.0;
        ylaReuna.Restitution = 1.0;
        ylaReuna.IsVisible = false;

        IntMeter pelaajan1Pisteet = LuoPisteLaskuri(this, Screen.Top - 100.0, Screen.Left + 100.0);
        IntMeter pelaajan2Pisteet = LuoPisteLaskuri(this, Screen.Top -100.0, Screen.Right - 100.0);

        AddCollisionHandler(pallo, oikeaReuna, (_, _) => pelaajan1Pisteet.Value += 1);
        AddCollisionHandler(pallo, vasenReuna, (_, _) => pelaajan2Pisteet.Value += 1);

        Camera.ZoomToLevel();
    }

    private static PhysicsObject LuoPallo(PhysicsGame peli, double x, double y)
    {
        PhysicsObject pallo = new PhysicsObject(40.0, 40.0, Shape.Circle);
        pallo.Color = Color.Black;
        pallo.X = x;
        pallo.Y = y;
        pallo.Restitution = 1.01;
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
        Vector impulssi = new Vector(500.0, 200.0);
        pallo.Hit(impulssi * pallo.Mass);
    }
}