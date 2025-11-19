using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RpgGame.Systems
{
    public static class TextureHelper
    {
        private static Dictionary<GraphicsDevice, Texture2D> _pixels = new Dictionary<GraphicsDevice, Texture2D>();
        
        public static Texture2D GetPixel(GraphicsDevice device)
        {
            if (device == null)
                return null;
                
            if (!_pixels.ContainsKey(device))
            {
                var pixel = new Texture2D(device, 1, 1);
                pixel.SetData(new[] { Color.White });
                _pixels[device] = pixel;
            }
            return _pixels[device];
        }
        
        public static void DrawRectangle(SpriteBatch spriteBatch, GraphicsDevice device, Rectangle rect, Color color)
        {
            if (device == null || spriteBatch == null)
                return;
                
            var pixel = GetPixel(device);
            if (pixel != null)
            {
                spriteBatch.Draw(pixel, rect, null, color);
            }
        }
        
        public static void DrawRectangleOutline(SpriteBatch spriteBatch, GraphicsDevice device, Rectangle rect, Color color, int thickness = 1)
        {
            if (device == null || spriteBatch == null)
                return;
                
            var pixel = GetPixel(device);
            if (pixel == null)
                return;
            
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

