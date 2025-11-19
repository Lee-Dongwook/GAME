using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public class GameScene : Scene
{
    private SpriteFont font;

    public GameScene(ContentManager content) : base(content) { }

    public override void Load()
    {
        try
        {
            font = Content.Load<SpriteFont>("DefaultFont");
        }
        catch
        {
            // Content Pipeline이 없을 경우 null로 설정
            font = null;
        }
    }

    public override void Unload() { }

    public override void Update(GameTime time)
    {
        // 앞으로 플레이어, 몬스터, 맵 등이 여기에 추가됨
    }

    public override void Draw(SpriteBatch batch)
    {
        if (font != null)
        {
            batch.DrawString(font, "Game Scene (In Development)", new Vector2(50, 50), Color.White);
        }
    }
}
