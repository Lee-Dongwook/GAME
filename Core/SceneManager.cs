using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class SceneManager
{
    private static Scene _current;

    public static void Initialize() {}

    public static void ChangeScene(Scene scene)
    {
        _current?.Unload();
        _current = scene;
        _current?.Load();
    }

    public static void Update(GameTime time)
    {
        _current?.Update(time);
    }
    
    public static void Draw(SpriteBatch batch)
    {
        _current?.Draw(batch);
    }
}
