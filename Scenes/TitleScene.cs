using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RpgGame.Systems;

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

    public override void Draw(SpriteBatch batch, GraphicsDevice graphicsDevice)
    {
        if (batch == null || graphicsDevice == null)
            return;
            
        batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        
        // 배경을 검은색으로
        TextureHelper.DrawRectangle(batch, graphicsDevice, new Rectangle(0, 0, 1280, 720), Color.Black);
        
        if (font != null)
        {
            batch.DrawString(font, "Press ENTER to Start", new Vector2(450, 350), Color.White);
        }
        else
        {
            // 폰트가 없을 경우 흰색 사각형으로 표시
            TextureHelper.DrawRectangle(batch, graphicsDevice, new Rectangle(450, 350, 400, 50), Color.White);
        }
        
        batch.End();
    }
}
