using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RpgGame.Entities;
using RpgGame.Systems;

public class GameScene : Scene
{
    private SpriteFont font;
    private Player player;
    private Map map;
    private Camera camera;

    public GameScene(ContentManager content) : base(content) { }

    public override void Load()
    {
        try
        {
            font = Content.Load<SpriteFont>("DefaultFont");
        }
        catch
        {
            font = null;
        }
        
        // 맵과 플레이어 초기화
        map = new Map(50, 50);
        player = new Player(new Vector2(400, 300));
        
        // 카메라 초기화 (나중에 GraphicsDevice 필요)
        camera = new Camera(1280, 720);
    }

    public override void Unload() { }

    public override void Update(GameTime time)
    {
        // 입력 처리
        Vector2 movement = Vector2.Zero;
        
        if (Input.Held(Keys.W) || Input.Held(Keys.Up))
            movement.Y -= 1;
        if (Input.Held(Keys.S) || Input.Held(Keys.Down))
            movement.Y += 1;
        if (Input.Held(Keys.A) || Input.Held(Keys.Left))
            movement.X -= 1;
        if (Input.Held(Keys.D) || Input.Held(Keys.Right))
            movement.X += 1;
        
        // 정규화 (대각선 이동 속도 보정)
        if (movement.Length() > 0)
            movement.Normalize();
        
        // 플레이어 이동
        Vector2 newPosition = player.Position + movement * player.Speed * (float)time.ElapsedGameTime.TotalSeconds;
        
        // 맵 충돌 체크
        if (map.IsWalkable(newPosition))
        {
            player.Update(time, movement);
        }
        
        // 카메라가 플레이어를 따라감
        camera.Follow(player.Position);
    }

    public override void Draw(SpriteBatch batch, GraphicsDevice graphicsDevice)
    {
        // 간단한 테스트: 카메라 변환 없이 직접 화면 좌표로 그리기
        batch.Begin();
        
        // 디버깅: 전체 화면을 빨간색으로 채워서 렌더링이 작동하는지 확인
        TextureHelper.DrawRectangle(batch, graphicsDevice, new Rectangle(0, 0, 1280, 720), Color.DarkGreen);
        
        // 맵 그리기 (간단하게 화면에 맞춰서)
        int tilesToShowX = 1280 / map.TileSize;
        int tilesToShowY = 720 / map.TileSize;
        
        int playerTileX = (int)(player.Position.X / map.TileSize);
        int playerTileY = (int)(player.Position.Y / map.TileSize);
        
        int offsetX = Math.Max(0, Math.Min(map.Width - tilesToShowX, playerTileX - tilesToShowX / 2));
        int offsetY = Math.Max(0, Math.Min(map.Height - tilesToShowY, playerTileY - tilesToShowY / 2));
        
        for (int x = 0; x < tilesToShowX && (x + offsetX) < map.Width; x++)
        {
            for (int y = 0; y < tilesToShowY && (y + offsetY) < map.Height; y++)
            {
                int tileX = x + offsetX;
                int tileY = y + offsetY;
                int tile = map.GetTile(tileX, tileY);
                Color color = GetTileColor(tile);
                
                Rectangle destRect = new Rectangle(
                    x * map.TileSize,
                    y * map.TileSize,
                    map.TileSize,
                    map.TileSize
                );
                
                TextureHelper.DrawRectangle(batch, graphicsDevice, destRect, color);
            }
        }
        
        // 플레이어 그리기 (화면 좌표로 변환)
        int screenPlayerX = (int)((player.Position.X / map.TileSize - offsetX) * map.TileSize);
        int screenPlayerY = (int)((player.Position.Y / map.TileSize - offsetY) * map.TileSize);
        
        var playerRect = new Rectangle(
            screenPlayerX - 16,
            screenPlayerY - 16,
            32,
            32
        );
        TextureHelper.DrawRectangle(batch, graphicsDevice, playerRect, Color.Blue);
        TextureHelper.DrawRectangleOutline(batch, graphicsDevice, playerRect, Color.DarkBlue, 2);
        
        // UI 그리기
        if (font != null)
        {
            batch.DrawString(font, $"Level: {player.Level}", new Vector2(10, 10), Color.White);
            batch.DrawString(font, $"HP: {player.CurrentHP}/{player.MaxHP}", new Vector2(10, 40), Color.Red);
            batch.DrawString(font, $"MP: {player.CurrentMP}/{player.MaxMP}", new Vector2(10, 70), Color.Blue);
            batch.DrawString(font, $"EXP: {player.Experience}/{player.ExperienceToNextLevel}", new Vector2(10, 100), Color.Yellow);
        }
        else
        {
            // 폰트가 없을 경우 색상으로 표시
            DrawHealthBar(batch, graphicsDevice, new Rectangle(10, 10, 200, 20), player.CurrentHP, player.MaxHP, Color.Red);
            DrawHealthBar(batch, graphicsDevice, new Rectangle(10, 35, 200, 20), player.CurrentMP, player.MaxMP, Color.Blue);
        }
        
        batch.End();
    }
    
    private Color GetTileColor(int tile)
    {
        return tile switch
        {
            Map.TILE_GRASS => Color.Green,
            Map.TILE_WALL => Color.Gray,
            Map.TILE_WATER => Color.Blue,
            Map.TILE_STONE => Color.DarkGray,
            _ => Color.Black
        };
    }
    
    private void DrawHealthBar(SpriteBatch batch, GraphicsDevice device, Rectangle rect, int current, int max, Color color)
    {
        // 배경
        TextureHelper.DrawRectangle(batch, device, rect, Color.Black);
        TextureHelper.DrawRectangleOutline(batch, device, rect, Color.White, 1);
        
        // 체력/마나 바
        if (max > 0)
        {
            int fillWidth = (int)(rect.Width * ((float)current / max));
            if (fillWidth > 0)
            {
                Rectangle fillRect = new Rectangle(rect.X + 1, rect.Y + 1, fillWidth - 2, rect.Height - 2);
                TextureHelper.DrawRectangle(batch, device, fillRect, color);
            }
        }
    }
}
