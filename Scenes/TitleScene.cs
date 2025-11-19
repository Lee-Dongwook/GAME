using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

public class TitleScene : Scene
{
    private SpriteFont font;

    public TitleScene(ContentManager content) : base(content) { }

    public override void Load()
    {
        font = Content.Load<SpriteFont>("DefaultFont");
    }

    public override void Unload() { }

    public override void Update(GameTime time)
    {
        if (Input.Pressed(Keys.Enter))
        {
            SceneManager.ChangeScene(new GameScene(Content));
        }
    }

    public override void Draw(SpriteBatch batch)
    {
        batch.DrawString(font, "Press ENTER to Start", new Vector2(450, 350), Color.White);
    }
}
