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
        try
        {
            font = Content.Load<SpriteFont>("DefaultFont");
        }
        catch
        {
            // Content Pipeline이 없을 경우 null로 설정 (텍스트 없이 실행)
            font = null;
        }
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
        if (font != null)
        {
            batch.DrawString(font, "Press ENTER to Start", new Vector2(450, 350), Color.White);
        }
        // 폰트가 없을 경우 텍스트 없이 실행 (게임은 작동함)
    }
}
