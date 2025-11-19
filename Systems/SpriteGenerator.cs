using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Systems
{
    public static class SpriteGenerator
    {
        // 플레이어 스프라이트 생성 (간단한 캐릭터 모양)
        public static Texture2D GeneratePlayerSprite(GraphicsDevice device, int size = 32)
        {
            var texture = new Texture2D(device, size, size);
            Color[] data = new Color[size * size];
            
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int index = y * size + x;
                    
                    // 몸통 (파란색 원)
                    float centerX = size / 2f;
                    float centerY = size / 2f;
                    float dist = System.MathF.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    
                    if (dist < size / 3f)
                    {
                        data[index] = Color.Blue;
                    }
                    // 머리 (위쪽 작은 원)
                    else if (y < size / 3f && dist < size / 4f)
                    {
                        data[index] = Color.LightBlue;
                    }
                    // 외곽선
                    else if (dist < size / 2.5f && dist > size / 3f - 1)
                    {
                        data[index] = Color.DarkBlue;
                    }
                    else
                    {
                        data[index] = Color.Transparent;
                    }
                }
            }
            
            texture.SetData(data);
            return texture;
        }
        
        // 몬스터 스프라이트 생성 (빨간색 원형)
        public static Texture2D GenerateMonsterSprite(GraphicsDevice device, int size = 24, Color color = default)
        {
            if (color == default)
                color = Color.Red;
                
            var texture = new Texture2D(device, size, size);
            Color[] data = new Color[size * size];
            
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int index = y * size + x;
                    
                    float centerX = size / 2f;
                    float centerY = size / 2f;
                    float dist = System.MathF.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    
                    if (dist < size / 2.5f)
                    {
                        data[index] = color;
                    }
                    else if (dist < size / 2f && dist > size / 2.5f - 1)
                    {
                        data[index] = new Color(color.R / 2, color.G / 2, color.B / 2);
                    }
                    else
                    {
                        data[index] = Color.Transparent;
                    }
                }
            }
            
            texture.SetData(data);
            return texture;
        }
        
        // 맵 타일 스프라이트 생성
        public static Texture2D GenerateTileSprite(GraphicsDevice device, int tileSize, Color baseColor, bool hasPattern = false)
        {
            var texture = new Texture2D(device, tileSize, tileSize);
            Color[] data = new Color[tileSize * tileSize];
            
            for (int y = 0; y < tileSize; y++)
            {
                for (int x = 0; x < tileSize; x++)
                {
                    int index = y * tileSize + x;
                    
                    if (hasPattern)
                    {
                        // 체크 패턴
                        bool isEven = ((x / 4) + (y / 4)) % 2 == 0;
                        data[index] = isEven ? baseColor : new Color(baseColor.R * 0.9f, baseColor.G * 0.9f, baseColor.B * 0.9f);
                    }
                    else
                    {
                        data[index] = baseColor;
                    }
                    
                    // 가장자리 어둡게
                    if (x == 0 || x == tileSize - 1 || y == 0 || y == tileSize - 1)
                    {
                        data[index] = new Color(baseColor.R * 0.7f, baseColor.G * 0.7f, baseColor.B * 0.7f);
                    }
                }
            }
            
            texture.SetData(data);
            return texture;
        }
        
        // 벽 스프라이트 생성
        public static Texture2D GenerateWallSprite(GraphicsDevice device, int tileSize)
        {
            var texture = new Texture2D(device, tileSize, tileSize);
            Color[] data = new Color[tileSize * tileSize];
            
            Color wallColor = Color.Gray;
            Color darkColor = new Color(wallColor.R * 0.6f, wallColor.G * 0.6f, wallColor.B * 0.6f);
            
            for (int y = 0; y < tileSize; y++)
            {
                for (int x = 0; x < tileSize; x++)
                {
                    int index = y * tileSize + x;
                    
                    // 벽돌 패턴
                    bool isBrick = (x / 8 + y / 8) % 2 == 0;
                    data[index] = isBrick ? wallColor : darkColor;
                    
                    // 벽돌 사이 틈
                    if (y % 8 == 7 || x % 8 == 7)
                    {
                        data[index] = Color.Black;
                    }
                }
            }
            
            texture.SetData(data);
            return texture;
        }
    }
}

