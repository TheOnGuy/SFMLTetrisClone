using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLTetrisKlon.src
{
    public static class BlockInformation
    {
        public static Vector2f Size { get; } = new Vector2f(10, 10);

        public static Dictionary<char, Color> BlockColors { get; } = new Dictionary<char, Color>()
        {
            //{ 'J', new Color(64, 26, 235) },
            { 'I', new Color(0, 240, 240) },
            { 'J', new Color(0, 0, 240) },
            { 'L', new Color(240, 160, 0) },
            { 'O', new Color(240, 240, 0) },
            { 'S', new Color(0, 240, 0) },
            { 'T', new Color(160, 0, 240) },
            { 'Z', new Color(240, 0, 0) },
        };

        public static Dictionary<char, int[,]> BlockArrays { get; } = new Dictionary<char, int[,]>()
        {
            {
                'I',
                new int[1, 4]
                {
                    { 1, 1, 1, 1 }
                }
            },
            {
                'J',
                new int[2, 3]
                {
                    { 1, 0, 0 },
                    { 1, 1, 1 }
                }
            },
            {
                'L',
                new int[2, 3]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 }
                }
            },
            {
                'O',
                new int[2, 2]
                {
                    { 1, 1 },
                    { 1, 1 }
                }
            },
            {
                'S',
                new int[2, 3]
                {
                    { 0, 1, 1 },
                    { 1, 1, 0 }
                }
            },
            {
                'T',
                new int[2, 3]
                {
                    { 0, 1, 0 },
                    { 1, 1, 1 }
                }
            },
            {
                'Z',
                new int[2, 3]
                {
                    { 1, 1, 0 },
                    { 0, 1, 1 }
                }
            },
        };
    }

    public struct Block
    {
        public Vector2f Position;
        public char BlockType;
        public int[,] BlockArray;

        public int[,] Rotate()
        {
            if (BlockArray == null) { return null; }
            
            int width = BlockArray.GetUpperBound(0) + 1;
            int height = BlockArray.GetUpperBound(1) + 1;
            int[,] ret = new int[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int newY = x;
                    int newX = height - (y + 1);

                    ret[newX, newY] = BlockArray[x, y];
                }
            }

            return ret;
        }
    }
}
