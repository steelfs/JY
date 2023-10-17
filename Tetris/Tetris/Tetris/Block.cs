using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Testris;

namespace Tetris
{
    internal class Block
    {
        public BlockType blockType;
        public Board board;
        int x;
        int y;
        public Block(int x,int y, BlockType Type, Board board) 
        {
            this.x = x;
            this.y = y;
            this.board = board;
            this.blockType = Type;
            Move();
        }
        
        void Move()
        {
            while(this.y < board.Height)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.A:
                        MoveLeft();
                        board.SetBlock(x, y, BlockType.I);
                        Console.Clear();
                        board.Render();
                        break;
                    case ConsoleKey.D:
                        MoveRight();
                        board.SetBlock(x, y, BlockType.I);
                        Console.Clear();
                        board.Render();
                        break;
                    case ConsoleKey.S:
                        MoveDown();
                        board.SetBlock(x, y, BlockType.I);
                        Console.Clear();
                        board.Render();
                        break;
                    default:
                        break;

                }
                    
            }
        }
        void MoveLeft()
        {
            if(this.x > 1)
            {
                x--;
            }
        }
        void MoveRight()
        {
            if (this.x < board.Width)
            {
                x++;
            }
        }
        void MoveDown()
        {
            if (this.y < board.Height)
            {
                y++;
            }
        }
    }
}
