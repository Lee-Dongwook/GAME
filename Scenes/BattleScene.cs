using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RpgGame.Entities;
using RpgGame.Systems;

public class BattleScene : Scene
{
    private Player player;
    private Monster monster;
    private SpriteFont font;
    
    // 전투 상태
    private BattleState state;
    private int selectedActionIndex = 0;
    private string battleMessage = "";
    private float messageTimer = 0f;
    
    // 액션 메뉴
    private readonly string[] actions = { "공격", "스킬", "아이템", "도망" };
    
    // 스프라이트 텍스처
    private Texture2D playerSprite;
    private Texture2D monsterSprite;
    
    private enum BattleState
    {
        PlayerTurn,      // 플레이어 턴
        EnemyTurn,       // 적 턴
        Processing,      // 처리 중
        Victory,         // 승리
        Defeat           // 패배
    }
    
    public BattleScene(ContentManager content, Player player, Monster monster) : base(content)
    {
        this.player = player;
        this.monster = monster;
        state = BattleState.PlayerTurn;
        battleMessage = $"{monster.Name}이(가) 나타났다!";
    }
    
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
    }
    
    private void InitializeSprites(GraphicsDevice device)
    {
        if (playerSprite == null)
        {
            playerSprite = SpriteGenerator.GeneratePlayerSprite(device, 64);
            // 몬스터 타입에 따라 다른 색상
            Color monsterColor = monster.Name switch
            {
                "슬라임" => Color.Green,
                "고블린" => Color.Orange,
                "오크" => Color.Brown,
                "늑대" => Color.Gray,
                _ => Color.Red
            };
            monsterSprite = SpriteGenerator.GenerateMonsterSprite(device, 80, monsterColor);
        }
    }
    
    public override void Unload() { }
    
    public override void Update(GameTime time)
    {
        float deltaTime = (float)time.ElapsedGameTime.TotalSeconds;
        
        // 메시지 타이머 업데이트
        if (messageTimer > 0)
        {
            messageTimer -= deltaTime;
            if (messageTimer <= 0 && state == BattleState.Processing)
            {
                // 도망 성공 처리
                if (battleMessage == "도망에 성공했다!")
                {
                    SceneManager.ChangeScene(new GameScene(Content));
                    return;
                }
                
                // 처리 완료 후 적 턴으로
                if (monster.IsAlive)
                {
                    state = BattleState.EnemyTurn;
                    ProcessEnemyTurn();
                }
                else
                {
                    state = BattleState.Victory;
                    battleMessage = "승리!";
                }
            }
        }
        
        // 승리/패배 처리
        if (state == BattleState.Victory)
        {
            if (Input.Pressed(Keys.Enter) || Input.Pressed(Keys.Space))
            {
                // 경험치와 골드 획득
                player.GainExperience(monster.ExperienceReward);
                // 골드는 나중에 추가
                
                // 게임 씬으로 돌아가기
                SceneManager.ChangeScene(new GameScene(Content));
            }
        }
        else if (state == BattleState.Defeat)
        {
            if (Input.Pressed(Keys.Enter) || Input.Pressed(Keys.Space))
            {
                // 타이틀 화면으로 돌아가기
                SceneManager.ChangeScene(new TitleScene(Content));
            }
        }
        else if (state == BattleState.PlayerTurn)
        {
            // 액션 선택
            if (Input.Pressed(Keys.Up) || Input.Pressed(Keys.W))
            {
                selectedActionIndex = Math.Max(0, selectedActionIndex - 1);
            }
            if (Input.Pressed(Keys.Down) || Input.Pressed(Keys.S))
            {
                selectedActionIndex = Math.Min(actions.Length - 1, selectedActionIndex + 1);
            }
            
            // 액션 실행
            if (Input.Pressed(Keys.Enter) || Input.Pressed(Keys.Space))
            {
                ExecutePlayerAction();
            }
        }
    }
    
    private void ExecutePlayerAction()
    {
        switch (selectedActionIndex)
        {
            case 0: // 공격
                PlayerAttack();
                break;
            case 1: // 스킬
                PlayerSkill();
                break;
            case 2: // 아이템
                battleMessage = "아이템 기능은 아직 구현되지 않았습니다.";
                messageTimer = 2f;
                break;
            case 3: // 도망
                TryEscape();
                break;
        }
    }
    
    private void PlayerAttack()
    {
        int damage = player.Attack;
        monster.TakeDamage(damage);
        battleMessage = $"플레이어가 {damage}의 데미지를 입혔다!";
        
        if (!monster.IsAlive)
        {
            state = BattleState.Victory;
            battleMessage = $"{monster.Name}을(를) 쓰러뜨렸다!";
        }
        else
        {
            state = BattleState.Processing;
            messageTimer = 1.5f;
        }
    }
    
    private void PlayerSkill()
    {
        if (player.CurrentMP < 10)
        {
            battleMessage = "MP가 부족합니다!";
            messageTimer = 1.5f;
            return;
        }
        
        player.CurrentMP -= 10;
        int damage = player.Attack * 2; // 스킬은 공격력의 2배
        monster.TakeDamage(damage);
        battleMessage = $"강력한 스킬로 {damage}의 데미지를 입혔다!";
        
        if (!monster.IsAlive)
        {
            state = BattleState.Victory;
            battleMessage = $"{monster.Name}을(를) 쓰러뜨렸다!";
        }
        else
        {
            state = BattleState.Processing;
            messageTimer = 1.5f;
        }
    }
    
    private void TryEscape()
    {
        // 50% 확률로 도망 성공
        Random random = new Random();
        if (random.Next(2) == 0)
        {
            battleMessage = "도망에 성공했다!";
            messageTimer = 1f;
            state = BattleState.Processing;
            // 게임 씬으로 돌아가기 (다음 프레임에 처리)
            // Update에서 messageTimer가 0이 되면 처리하도록 함
        }
        else
        {
            battleMessage = "도망에 실패했다!";
            state = BattleState.Processing;
            messageTimer = 1f;
        }
    }
    
    private void ProcessEnemyTurn()
    {
        int damage = monster.GetAttackDamage();
        player.TakeDamage(damage);
        battleMessage = $"{monster.Name}이(가) {damage}의 데미지를 입혔다!";
        
        if (!player.IsAlive)
        {
            state = BattleState.Defeat;
            battleMessage = "패배했다...";
        }
        else
        {
            state = BattleState.PlayerTurn;
            messageTimer = 1.5f;
        }
    }
    
    public override void Draw(SpriteBatch batch, GraphicsDevice graphicsDevice)
    {
        if (batch == null || graphicsDevice == null)
            return;
        
        // 스프라이트 초기화
        InitializeSprites(graphicsDevice);
            
        batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        
        // 배경
        TextureHelper.DrawRectangle(batch, graphicsDevice, new Rectangle(0, 0, 1280, 720), Color.DarkBlue);
        
        // 플레이어 정보 (왼쪽)
        DrawPlayerInfo(batch, graphicsDevice, new Vector2(50, 50));
        
        // 플레이어 스프라이트 (왼쪽 하단)
        Rectangle playerSpriteRect = new Rectangle(100, 500, 64, 64);
        if (playerSprite != null)
        {
            batch.Draw(playerSprite, playerSpriteRect, Color.White);
        }
        
        // 몬스터 정보 (오른쪽)
        DrawMonsterInfo(batch, graphicsDevice, new Vector2(800, 50));
        
        // 액션 메뉴 (하단)
        DrawActionMenu(batch, graphicsDevice, new Vector2(50, 500));
        
        // 메시지 표시
        DrawMessage(batch, graphicsDevice, new Vector2(50, 400));
        
        batch.End();
    }
    
    private void DrawPlayerInfo(SpriteBatch batch, GraphicsDevice device, Vector2 position)
    {
        SimpleFont.DrawString(batch, device, font, "Player", position, Color.White);
        SimpleFont.DrawString(batch, device, font, $"HP: {player.CurrentHP}/{player.MaxHP}", position + new Vector2(0, 20), Color.Red);
        SimpleFont.DrawString(batch, device, font, $"MP: {player.CurrentMP}/{player.MaxMP}", position + new Vector2(0, 40), Color.Blue);
        SimpleFont.DrawString(batch, device, font, $"Lv: {player.Level}", position + new Vector2(0, 60), Color.Yellow);
        
        // 체력/마나 바도 함께 표시
        DrawHealthBar(batch, device, new Rectangle((int)position.X, (int)position.Y + 80, 200, 20), 
                     player.CurrentHP, player.MaxHP, Color.Red);
        DrawHealthBar(batch, device, new Rectangle((int)position.X, (int)position.Y + 105, 200, 20), 
                     player.CurrentMP, player.MaxMP, Color.Blue);
    }
    
    private void DrawMonsterInfo(SpriteBatch batch, GraphicsDevice device, Vector2 position)
    {
        SimpleFont.DrawString(batch, device, font, monster.Name, position, Color.White);
        SimpleFont.DrawString(batch, device, font, $"HP: {monster.CurrentHP}/{monster.MaxHP}", position + new Vector2(0, 20), Color.Red);
        SimpleFont.DrawString(batch, device, font, $"Lv: {monster.Level}", position + new Vector2(0, 40), Color.Yellow);
        
        // 체력 바도 함께 표시
        DrawHealthBar(batch, device, new Rectangle((int)position.X, (int)position.Y + 60, 200, 20), 
                     monster.CurrentHP, monster.MaxHP, Color.Red);
        
        // 몬스터 시각적 표현
        Rectangle monsterRect = new Rectangle((int)position.X, (int)position.Y + 100, 100, 100);
        if (monsterSprite != null)
        {
            batch.Draw(monsterSprite, monsterRect, Color.White);
        }
        else
        {
            TextureHelper.DrawRectangle(batch, device, monsterRect, Color.Red);
        }
    }
    
    private void DrawActionMenu(SpriteBatch batch, GraphicsDevice device, Vector2 position)
    {
        if (state != BattleState.PlayerTurn)
            return;
            
        for (int i = 0; i < actions.Length; i++)
        {
            Color color = (i == selectedActionIndex) ? Color.Yellow : Color.White;
            
            string prefix = (i == selectedActionIndex) ? "> " : "  ";
            SimpleFont.DrawString(batch, device, font, prefix + actions[i], position + new Vector2(0, i * 30), color);
            
            // 선택된 항목 강조
            if (i == selectedActionIndex)
            {
                Rectangle rect = new Rectangle((int)position.X - 5, (int)(position.Y + i * 30) - 2, 120, 20);
                TextureHelper.DrawRectangleOutline(batch, device, rect, Color.Yellow, 2);
            }
        }
    }
    
    private void DrawMessage(SpriteBatch batch, GraphicsDevice device, Vector2 position)
    {
        if (!string.IsNullOrEmpty(battleMessage))
        {
            // 메시지 박스 배경
            Rectangle msgBox = new Rectangle((int)position.X - 10, (int)position.Y - 10, 600, 40);
            TextureHelper.DrawRectangle(batch, device, msgBox, Color.Black);
            TextureHelper.DrawRectangleOutline(batch, device, msgBox, Color.White, 2);
            
            // 메시지 텍스트
            SimpleFont.DrawString(batch, device, font, battleMessage, position, Color.White);
        }
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

