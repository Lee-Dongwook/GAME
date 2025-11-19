using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RpgGame.Systems
{
    public class Map
    {
        private int[,] _tiles;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileSize { get; set; } = 32;
        
        // 타일 타입
        public const int TILE_GRASS = 0;
        public const int TILE_WALL = 1;
        public const int TILE_WATER = 2;
        public const int TILE_STONE = 3;
        
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new int[width, height];
            GenerateMap();
        }
        
        private void GenerateMap()
        {
            // 간단한 맵 생성 (나중에 더 복잡하게)
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    // 가장자리는 벽
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        _tiles[x, y] = TILE_WALL;
                    }
                    else
                    {
                        _tiles[x, y] = TILE_GRASS;
                    }
                }
            }
        }
        
        public int GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return TILE_WALL;
            return _tiles[x, y];
        }
        
        public bool IsWalkable(int x, int y)
        {
            int tile = GetTile(x, y);
            return tile == TILE_GRASS || tile == TILE_STONE;
        }
        
        public bool IsWalkable(Vector2 position)
        {
            int tileX = (int)(position.X / TileSize);
            int tileY = (int)(position.Y / TileSize);
            return IsWalkable(tileX, tileY);
        }
        
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            // 이 메서드는 더 이상 사용되지 않음 (GameScene에서 직접 그리기)
            // 하지만 호환성을 위해 남겨둠
        }
        
        private Color GetTileColor(int tile)
        {
            return tile switch
            {
                TILE_GRASS => Color.Green,
                TILE_WALL => Color.Gray,
                TILE_WATER => Color.Blue,
                TILE_STONE => Color.DarkGray,
                _ => Color.Black
            };
        }
        
        private void DrawTile(SpriteBatch spriteBatch, GraphicsDevice device, Rectangle rect, Color color)
        {
            TextureHelper.DrawRectangle(spriteBatch, device, rect, color);
        }
    }
}

