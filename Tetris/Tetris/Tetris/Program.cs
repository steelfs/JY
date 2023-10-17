
using System.Runtime.CompilerServices;
using Tetris;

namespace Testris
{
    public enum CellType
    {
        Void,
        Wall,
        Block
    }

    public class Program
    {
        static Random random = new Random();
        static Board board;
        static void Main(string[] args)
        {
            string message = "시작하려면 아무키나 누르시오";
            int messageLength = message.Length;

            int centerX = (Console.WindowWidth / 2) - (messageLength / 2);
            int centerY = Console.WindowHeight / 2;

            while (!Console.KeyAvailable) // 키 입력이 없을 때까지 루프
            {
                Console.Clear(); // 화면 지우기

                // 커서를 중앙으로 이동
                Console.SetCursorPosition(centerX, centerY);
                Console.Write(message); // 메시지 출력

                Thread.Sleep(500); // 0.5초 대기

                Console.Clear(); // 화면 지우기

                Thread.Sleep(500); // 0.5초 대기
            }

            // 아무 키나 눌렀을 때 나가기
            Console.ReadKey();
            Console.Clear();
            GameStart();
        }
        static void GameStart()
        {
            board = new Board(10, 15);
            board.GenerateBoard();
            board.StartRender();
            GenerateBlock();
        }
        static void GenerateBlock()
        {
            int posX = random.Next(0,board.Width);
            
            Block block = new Block(board.Width / 2, 0,BlockType.I, board);
        }
    }
}

