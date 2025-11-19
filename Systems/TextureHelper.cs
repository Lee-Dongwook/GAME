using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Systems
{
    public static class TextureHelper
    {
        private static Texture2D _pixel;
        
        public static Texture2D GetPixel(GraphicsDevice device)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(device, 1, 1);
                _pixel.SetData(new[] { Color.White });
            }
            return _pixel;
        }
        
        public static void DrawRectangle(SpriteBatch spriteBatch, GraphicsDevice device, Rectangle rect, Color color)
        {
            var pixel = GetPixel(device);
            spriteBatch.Draw(pixel, rect, null, color);
        }
        
        public static void DrawRectangleOutline(SpriteBatch spriteBatch, GraphicsDevice device, Rectangle rect, Color color, int thickness = 1)
        {
            var pixel = GetPixel(device);
            
            // 위쪽
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), null, color);
            // 아래쪽
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), null, color);
            // 왼쪽
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), null, color);
            // 오른쪽
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), null, color);
        }
    }
}

