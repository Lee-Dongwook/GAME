using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Entities
{
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; protected set; }
        
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}

