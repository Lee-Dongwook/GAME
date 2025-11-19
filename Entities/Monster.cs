using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Entities
{
    public class Monster : Entity
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int ExperienceReward { get; set; }
        public int GoldReward { get; set; }
        
        public Monster(string name, int level, Vector2 position)
        {
            Name = name;
            Level = level;
            Position = position;
            
            // 레벨에 따른 스탯 계산
            MaxHP = 50 + (level * 20);
            CurrentHP = MaxHP;
            Attack = 5 + (level * 3);
            Defense = 2 + level;
            ExperienceReward = 20 + (level * 10);
            GoldReward = 10 + (level * 5);
            
            Bounds = new Rectangle((int)Position.X - 16, (int)Position.Y - 16, 32, 32);
        }
        
        public override void Update(GameTime gameTime)
        {
            Bounds = new Rectangle((int)Position.X - 16, (int)Position.Y - 16, 32, 32);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            // 임시로 빈 공간 (나중에 스프라이트로 교체)
        }
        
        public void TakeDamage(int damage)
        {
            int actualDamage = Math.Max(1, damage - Defense);
            CurrentHP = Math.Max(0, CurrentHP - actualDamage);
        }
        
        public bool IsAlive => CurrentHP > 0;
        
        public int GetAttackDamage()
        {
            return Attack;
        }
    }
}

