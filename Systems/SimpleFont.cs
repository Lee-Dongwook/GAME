using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Systems
{
    public static class SimpleFont
    {
        private static Dictionary<char, Texture2D> _charTextures = new Dictionary<char, Texture2D>();
        private static Dictionary<GraphicsDevice, SpriteFont> _fallbackFonts = new Dictionary<GraphicsDevice, SpriteFont>();
        
        public static void DrawString(SpriteBatch spriteBatch, GraphicsDevice device, SpriteFont font, string text, Vector2 position, Color color)
        {
            if (font != null)
            {
                spriteBatch.DrawString(font, text, position, color);
            }
            else
            {
                // 폰트가 없을 경우 간단한 비트맵 폰트로 표시
                DrawSimpleString(spriteBatch, device, text, position, color);
            }
        }
        
        private static void DrawSimpleString(SpriteBatch spriteBatch, GraphicsDevice device, string text, Vector2 position, Color color)
        {
            float x = position.X;
            float y = position.Y;
            int charWidth = 8;
            int charHeight = 12;
            
            foreach (char c in text)
            {
                if (c == ' ')
                {
                    x += charWidth;
                    continue;
                }
                
                if (c == '\n')
                {
                    y += charHeight;
                    x = position.X;
                    continue;
                }
                
                var charTexture = GetCharTexture(device, c);
                if (charTexture != null)
                {
                    spriteBatch.Draw(charTexture, new Rectangle((int)x, (int)y, charWidth, charHeight), color);
                }
                x += charWidth;
            }
        }
        
        private static Texture2D GetCharTexture(GraphicsDevice device, char c)
        {
            if (!_charTextures.ContainsKey(c))
            {
                _charTextures[c] = GenerateCharTexture(device, c);
            }
            return _charTextures[c];
        }
        
        private static Texture2D GenerateCharTexture(GraphicsDevice device, char c)
        {
            int width = 8;
            int height = 12;
            var texture = new Texture2D(device, width, height);
            Color[] data = new Color[width * height];
            
            // 간단한 8x12 비트맵 폰트 패턴
            bool[,] pattern = GetCharPattern(c);
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    if (pattern[y, x])
                    {
                        data[index] = Color.White;
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
        
        private static bool[,] GetCharPattern(char c)
        {
            bool[,] pattern = new bool[12, 8];
            
            // 숫자 패턴
            if (c >= '0' && c <= '9')
            {
                return GetNumberPattern(c - '0');
            }
            
            // 영문자 패턴 (대소문자)
            if (c >= 'A' && c <= 'Z')
            {
                return GetLetterPattern(c - 'A', true);
            }
            if (c >= 'a' && c <= 'z')
            {
                return GetLetterPattern(c - 'a', false);
            }
            
            // 특수 문자
            switch (c)
            {
                case ':': return GetColonPattern();
                case '/': return GetSlashPattern();
                case '-': return GetDashPattern();
                case '.': return GetDotPattern();
                case '!': return GetExclamationPattern();
                case '?': return GetQuestionPattern();
                default: return new bool[12, 8];
            }
        }
        
        private static bool[,] GetNumberPattern(int num)
        {
            // 간단한 숫자 패턴 (7-segment 스타일)
            bool[,] pattern = new bool[12, 8];
            
            switch (num)
            {
                case 0:
                    FillRect(pattern, 1, 1, 6, 1); // top
                    FillRect(pattern, 1, 10, 6, 1); // bottom
                    FillRect(pattern, 1, 1, 1, 11); // left top
                    FillRect(pattern, 6, 1, 1, 11); // right top
                    break;
                case 1:
                    FillRect(pattern, 4, 1, 1, 11);
                    break;
                case 2:
                    FillRect(pattern, 1, 1, 6, 1); // top
                    FillRect(pattern, 1, 5, 6, 1); // middle
                    FillRect(pattern, 1, 10, 6, 1); // bottom
                    FillRect(pattern, 6, 1, 1, 5); // right top
                    FillRect(pattern, 1, 6, 1, 5); // left bottom
                    break;
                case 3:
                    FillRect(pattern, 1, 1, 6, 1); // top
                    FillRect(pattern, 1, 5, 6, 1); // middle
                    FillRect(pattern, 1, 10, 6, 1); // bottom
                    FillRect(pattern, 6, 1, 1, 11); // right
                    break;
                case 4:
                    FillRect(pattern, 1, 1, 1, 6); // left top
                    FillRect(pattern, 1, 5, 6, 1); // middle
                    FillRect(pattern, 6, 1, 1, 11); // right
                    break;
                case 5:
                    FillRect(pattern, 1, 1, 6, 1); // top
                    FillRect(pattern, 1, 5, 6, 1); // middle
                    FillRect(pattern, 1, 10, 6, 1); // bottom
                    FillRect(pattern, 1, 1, 1, 5); // left top
                    FillRect(pattern, 6, 6, 1, 5); // right bottom
                    break;
                case 6:
                    FillRect(pattern, 1, 1, 6, 1); // top
                    FillRect(pattern, 1, 5, 6, 1); // middle
                    FillRect(pattern, 1, 10, 6, 1); // bottom
                    FillRect(pattern, 1, 1, 1, 11); // left
                    FillRect(pattern, 6, 6, 1, 5); // right bottom
                    break;
                case 7:
                    FillRect(pattern, 1, 1, 6, 1); // top
                    FillRect(pattern, 6, 1, 1, 11); // right
                    break;
                case 8:
                    FillRect(pattern, 1, 1, 6, 1); // top
                    FillRect(pattern, 1, 5, 6, 1); // middle
                    FillRect(pattern, 1, 10, 6, 1); // bottom
                    FillRect(pattern, 1, 1, 1, 11); // left
                    FillRect(pattern, 6, 1, 1, 11); // right
                    break;
                case 9:
                    FillRect(pattern, 1, 1, 6, 1); // top
                    FillRect(pattern, 1, 5, 6, 1); // middle
                    FillRect(pattern, 1, 10, 6, 1); // bottom
                    FillRect(pattern, 1, 1, 1, 5); // left top
                    FillRect(pattern, 6, 1, 1, 11); // right
                    break;
            }
            
            return pattern;
        }
        
        private static bool[,] GetLetterPattern(int index, bool uppercase)
        {
            // 간단한 영문자 패턴 (A-Z만)
            bool[,] pattern = new bool[12, 8];
            
            // 기본적으로 간단한 박스 형태로 표시
            if (uppercase && index < 26)
            {
                // A-Z는 간단하게 표시
                FillRect(pattern, 1, 1, 6, 1); // top
                FillRect(pattern, 1, 1, 1, 11); // left
                FillRect(pattern, 6, 1, 1, 11); // right
                FillRect(pattern, 1, 5, 6, 1); // middle (A의 경우)
            }
            
            return pattern;
        }
        
        private static bool[,] GetColonPattern()
        {
            bool[,] pattern = new bool[12, 8];
            FillRect(pattern, 3, 3, 2, 2);
            FillRect(pattern, 3, 8, 2, 2);
            return pattern;
        }
        
        private static bool[,] GetSlashPattern()
        {
            bool[,] pattern = new bool[12, 8];
            for (int i = 0; i < 12; i++)
            {
                if (i < 8) pattern[i, 7 - i] = true;
            }
            return pattern;
        }
        
        private static bool[,] GetDashPattern()
        {
            bool[,] pattern = new bool[12, 8];
            FillRect(pattern, 1, 5, 6, 1);
            return pattern;
        }
        
        private static bool[,] GetDotPattern()
        {
            bool[,] pattern = new bool[12, 8];
            FillRect(pattern, 3, 9, 2, 2);
            return pattern;
        }
        
        private static bool[,] GetExclamationPattern()
        {
            bool[,] pattern = new bool[12, 8];
            FillRect(pattern, 3, 1, 2, 8);
            FillRect(pattern, 3, 9, 2, 2);
            return pattern;
        }
        
        private static bool[,] GetQuestionPattern()
        {
            bool[,] pattern = new bool[12, 8];
            FillRect(pattern, 1, 1, 6, 1); // top
            FillRect(pattern, 6, 1, 1, 5); // right top
            FillRect(pattern, 1, 1, 1, 3); // left top
            FillRect(pattern, 1, 5, 3, 1); // middle
            FillRect(pattern, 3, 9, 2, 2); // dot
            return pattern;
        }
        
        private static void FillRect(bool[,] pattern, int x, int y, int width, int height)
        {
            for (int dy = 0; dy < height; dy++)
            {
                for (int dx = 0; dx < width; dx++)
                {
                    if (y + dy < pattern.GetLength(0) && x + dx < pattern.GetLength(1))
                    {
                        pattern[y + dy, x + dx] = true;
                    }
                }
            }
        }
    }
}

