using System;
using System.Collections.Generic;
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
    private List<Monster> monsters;
    private Random random;
    
    // 스프라이트 텍스처
    private Texture2D playerSprite;
    private Texture2D monsterSprite;
    private Texture2D grassTileSprite;
    private Texture2D wallTileSprite;

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
        
        // 카메라 초기화
        camera = new Camera(1280, 720);
        
        // 몬스터 초기화
        random = new Random();
        monsters = new List<Monster>();
        SpawnMonsters();
        
        // 스프라이트 생성 (GraphicsDevice가 필요하므로 Load에서 생성 불가, Draw에서 생성)
    }
    
    private void InitializeSprites(GraphicsDevice device)
    {
        if (playerSprite == null)
        {
            playerSprite = SpriteGenerator.GeneratePlayerSprite(device, 32);
            monsterSprite = SpriteGenerator.GenerateMonsterSprite(device, 24, Color.Red);
            grassTileSprite = SpriteGenerator.GenerateTileSprite(device, map.TileSize, Color.Green, true);
            wallTileSprite = SpriteGenerator.GenerateWallSprite(device, map.TileSize);
        }
    }
    
    private void SpawnMonsters()
    {
        // 맵에 몬스터 배치 (벽이 아닌 곳에만)
        for (int i = 0; i < 10; i++)
        {
            int x = random.Next(2, map.Width - 2);
            int y = random.Next(2, map.Height - 2);
            Vector2 position = new Vector2(x * map.TileSize + map.TileSize / 2, y * map.TileSize + map.TileSize / 2);
            
            if (map.IsWalkable(position))
            {
                int level = random.Next(1, 5);
                string[] monsterNames = { "슬라임", "고블린", "오크", "늑대" };
                string name = monsterNames[random.Next(monsterNames.Length)];
                monsters.Add(new Monster(name, level, position));
            }
        }
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
            
            // 몬스터와 충돌 체크
            CheckMonsterCollision();
        }
        
        // 카메라가 플레이어를 따라감
        camera.Follow(player.Position);
    }
    
    private void CheckMonsterCollision()
    {
        Rectangle playerBounds = new Rectangle(
            (int)player.Position.X - 16,
            (int)player.Position.Y - 16,
            32,
            32
        );
        
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            var monster = monsters[i];
            Rectangle monsterBounds = new Rectangle(
                (int)monster.Position.X - 16,
                (int)monster.Position.Y - 16,
                32,
                32
            );
            
            if (playerBounds.Intersects(monsterBounds))
            {
                // 전투 씬으로 전환
                SceneManager.ChangeScene(new BattleScene(Content, player, monster));
                return;
            }
        }
    }

    public override void Draw(SpriteBatch batch, GraphicsDevice graphicsDevice)
    {
        // 간단한 테스트: 카메라 변환 없이 직접 화면 좌표로 그리기
        if (batch == null || graphicsDevice == null)
            return;
        
        // 스프라이트 초기화 (첫 번째 Draw 호출 시)
        InitializeSprites(graphicsDevice);
            
        batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        
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
                
                // 타일 타입에 따라 다른 스프라이트 사용
                Texture2D tileSprite = (tile == Map.TILE_WALL) ? wallTileSprite : grassTileSprite;
                if (tileSprite != null)
                {
                    batch.Draw(tileSprite, destRect, Color.White);
                }
                else
                {
                    TextureHelper.DrawRectangle(batch, graphicsDevice, destRect, color);
                }
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
        // 플레이어 스프라이트 그리기
        if (playerSprite != null)
        {
            batch.Draw(playerSprite, playerRect, Color.White);
        }
        else
        {
            TextureHelper.DrawRectangle(batch, graphicsDevice, playerRect, Color.Blue);
            TextureHelper.DrawRectangleOutline(batch, graphicsDevice, playerRect, Color.DarkBlue, 2);
        }
        
        // 몬스터 그리기
        foreach (var monster in monsters)
        {
            int monsterScreenX = (int)((monster.Position.X / map.TileSize - offsetX) * map.TileSize);
            int monsterScreenY = (int)((monster.Position.Y / map.TileSize - offsetY) * map.TileSize);
            
            var monsterRect = new Rectangle(
                monsterScreenX - 12,
                monsterScreenY - 12,
                24,
                24
            );
            
            if (monsterSprite != null)
            {
                batch.Draw(monsterSprite, monsterRect, Color.White);
            }
            else
            {
                TextureHelper.DrawRectangle(batch, graphicsDevice, monsterRect, Color.Red);
                TextureHelper.DrawRectangleOutline(batch, graphicsDevice, monsterRect, Color.DarkRed, 1);
            }
        }
        
        // UI 그리기
        SimpleFont.DrawString(batch, graphicsDevice, font, $"Level: {player.Level}", new Vector2(10, 10), Color.White);
        SimpleFont.DrawString(batch, graphicsDevice, font, $"HP: {player.CurrentHP}/{player.MaxHP}", new Vector2(10, 30), Color.Red);
        SimpleFont.DrawString(batch, graphicsDevice, font, $"MP: {player.CurrentMP}/{player.MaxMP}", new Vector2(10, 50), Color.Blue);
        SimpleFont.DrawString(batch, graphicsDevice, font, $"EXP: {player.Experience}/{player.ExperienceToNextLevel}", new Vector2(10, 70), Color.Yellow);
        
        // 체력/마나 바도 함께 표시
        DrawHealthBar(batch, graphicsDevice, new Rectangle(10, 90, 200, 20), player.CurrentHP, player.MaxHP, Color.Red);
        DrawHealthBar(batch, graphicsDevice, new Rectangle(10, 115, 200, 20), player.CurrentMP, player.MaxMP, Color.Blue);
        
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
