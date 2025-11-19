using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Entities
{
    public class Player
    {
        // 위치
        public Vector2 Position { get; set; }
        public float Speed { get; set; } = 100f; // 픽셀/초
        
        // 스탯
        public int Level { get; set; } = 1;
        public int MaxHP { get; set; } = 100;
        public int CurrentHP { get; set; } = 100;
        public int MaxMP { get; set; } = 50;
        public int CurrentMP { get; set; } = 50;
        public int Attack { get; set; } = 10;
        public int Defense { get; set; } = 5;
        public int Experience { get; set; } = 0;
        public int ExperienceToNextLevel { get; set; } = 100;
        
        // 시각적 표현 (임시로 사각형)
        public Rectangle Bounds => new Rectangle(
            (int)Position.X - 16, 
            (int)Position.Y - 16, 
            32, 
            32
        );
        
        public Player(Vector2 startPosition)
        {
            Position = startPosition;
        }
        
        public void Update(GameTime gameTime, Vector2 movement)
        {
            // 이동
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += movement * Speed * deltaTime;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            // 임시로 사각형으로 표시 (나중에 스프라이트로 교체)
            var rect = Bounds;
            // MonoGame에는 기본 사각형 그리기가 없으므로 텍스처 필요
            // 일단 빈 공간으로 두고 나중에 스프라이트 추가
        }
        
        public void TakeDamage(int damage)
        {
            int actualDamage = Math.Max(1, damage - Defense);
            CurrentHP = Math.Max(0, CurrentHP - actualDamage);
        }
        
        public void Heal(int amount)
        {
            CurrentHP = Math.Min(MaxHP, CurrentHP + amount);
        }
        
        public void RestoreMP(int amount)
        {
            CurrentMP = Math.Min(MaxMP, CurrentMP + amount);
        }
        
        public void GainExperience(int exp)
        {
            Experience += exp;
            while (Experience >= ExperienceToNextLevel)
            {
                LevelUp();
            }
        }
        
        private void LevelUp()
        {
            Experience -= ExperienceToNextLevel;
            Level++;
            MaxHP += 20;
            CurrentHP = MaxHP; // 레벨업 시 체력 회복
            MaxMP += 10;
            CurrentMP = MaxMP; // 마나도 회복
            Attack += 3;
            Defense += 2;
            ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.5f);
        }
        
        public bool IsAlive => CurrentHP > 0;
    }
}

