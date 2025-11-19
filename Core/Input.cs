using Microsoft.Xna.Framework.Input;

public static class Input
{
    private static KeyboardState _current;
    private static KeyboardState _previous;

    public static void Initialize()
    {
        _current = Keyboard.GetState();
        _previous = _current;
    }

    public static void Update()
    {
        _previous = _current;
        _current = Keyboard.GetState();
    }

    public static bool Pressed(Keys key)
        => _previous.IsKeyUp(key) && _current.IsKeyDown(key);

    public static bool Held(Keys key)
        => _current.IsKeyDown(key);
}
