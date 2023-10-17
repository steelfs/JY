using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testris;

namespace Tetris
{
    internal class Board
    {
        List<List<CellType>> cells;
        int width = 0;
        int height = 0;
        public int Width => width;
        public int Height => height;
        public Board(int x, int y)
        {
            this.width = x;
            this.height = y;
            cells = new List<List<CellType>>(height + 2);
        }
        public void GenerateBoard()
        {

            for (int y = 0; y < this.height + 2; y++)
            {
                cells.Add(new List<CellType>(width + 2));
                for(int x = 0; x < this.width + 2; x++)
                {
                    if (y == 0 || x == 0 || y == this.height + 1 || x == this.width + 1)
                    {
                        cells[y].Add(CellType.Wall);
                    }
                    else
                    {
                        cells[y].Add(CellType.Void);
                    }
                }
            }
            //cells.Reverse();
        }
        public void StartRender()
        {
            for (int y = 0; y < cells.Count; y++)
            {
                for (int x = 0; x < cells[y].Count; x++)
                {
                    if (cells[y][x] == CellType.Wall)
                    {
                        Console.Write("■");
                    }
                    else if (cells[y][x] == CellType.Void)
                    {
                        Console.Write("□");
                    }
                    else
                    {
                        Console.Write("▣");
                    }
                    Thread.Sleep(10);  
                }
                Console.WriteLine();
            }
        }
        public void Render()
        {
            for (int y = 0; y < cells.Count; y++)
            {
                for (int x = 0; x < cells[y].Count; x++)
                {
                    if (cells[y][x] == CellType.Wall)
                    {
                        Console.Write("■");
                    }
                    else if (cells[y][x] == CellType.Void)
                    {
                        Console.Write("□");
                    }
                    else
                    {
                        Console.Write("▣");
                    }
                }
                Console.WriteLine();
            }
        }
        //bool IsWall(int x, int y)
        //{

        //}
        public void SetBlock(int x, int y, BlockType blockType)
        {
            //왼쪽 위 기준 빈칸부터 1, 1
            cells[y][x] = CellType.Block;
            switch (blockType)
            {
                case BlockType.I:
                    if (y > 3)
                    {
                        cells[y][x] = CellType.Block;
                        cells[y - 1][x] = CellType.Block;
                        cells[y - 2][x] = CellType.Block;
                        cells[y - 3][x] = CellType.Block;
                    }
                    else if (y > 2)
                    {
                        cells[y][x] = CellType.Block;
                        cells[y - 1][x] = CellType.Block;
                        cells[y - 2][x] = CellType.Block;
                    }
                    else if (y > 1)
                    {
                        cells[y][x] = CellType.Block;
                        cells[y - 1][x] = CellType.Block;
                    }
                    else if (y == 1)
                    {
                        cells[y][x] = CellType.Block;
                    }
                    break;
                case BlockType.L: 
                    break;
                case BlockType.RL:
                    break;
                case BlockType.S:
                    break;
                case BlockType.RS:
                    break;
                case BlockType.RT:
                    break;
                case BlockType.O:
                    break;
            }
        }
    }
}
