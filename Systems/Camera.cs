using Microsoft.Xna.Framework;

namespace RpgGame.Systems
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Zoom { get; set; } = 1f;
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }
        
        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                       Matrix.CreateScale(Zoom) *
                       Matrix.CreateTranslation(ViewportWidth / 2f, ViewportHeight / 2f, 0);
            }
        }
        
        public Camera(int viewportWidth, int viewportHeight)
        {
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
        }
        
        public void Follow(Vector2 targetPosition)
        {
            Position = targetPosition;
        }
        
        public Rectangle GetVisibleArea()
        {
            return new Rectangle(
                (int)(Position.X - ViewportWidth / 2f / Zoom),
                (int)(Position.Y - ViewportHeight / 2f / Zoom),
                (int)(ViewportWidth / Zoom),
                (int)(ViewportHeight / Zoom)
            );
        }
    }
}

