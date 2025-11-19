using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public abstract class Scene
{
    protected ContentManager Content;

    protected Scene(ContentManager content)
    {
        Content = content;
    }

    public abstract void Load();
    public abstract void Unload();
    public abstract void Update(GameTime time);
    public abstract void Draw(SpriteBatch batch);
}
