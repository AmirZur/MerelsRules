using System;

namespace MerelsRules.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            gm = GameManager.GetInstance();

            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine(PrintBoard());
                GameManager.Piece p = i % 2 == 0 ? GameManager.Piece.O : GameManager.Piece.X;
                Console.WriteLine("Where do you want to place a " + p.ToString() + "? ");
                int loc = int.Parse(Console.ReadLine());
                gm.Board[loc] = p;
                gm.PieceLocations[((i % 2) * 3) + (i / 2)] = loc;
            }

            //int[] locations = new[] { 0, 1, 2, 6, 7, 8 };
            //for (int i = 0; i < 6; i++)
            //{
            //    GameManager.Piece p = i % 2 == 0 ? GameManager.Piece.O : GameManager.Piece.X;
            //    int loc = locations[i];
            //    gm.Board[loc] = p;
            //    gm.PieceLocations[((i % 2) * 3) + (i / 2)] = loc;
            //}

            Console.WriteLine("\nBeginning Game\n");

            while(true)
            {
                Console.WriteLine(PrintBoard());
                Console.WriteLine("What piece do you want to move (0-9)? ");
                int initial = int.Parse(Console.ReadLine());
                Console.WriteLine("Where do you want to move the piece (0-9)? ");
                int destination = int.Parse(Console.ReadLine());
                Console.WriteLine();
                //player
                gm.MakeMove(initial, destination);
                Console.WriteLine("Your move:");
                Console.WriteLine(PrintBoard() + "\n");
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("You won!");
                    break;
                }
                //computer
                Tuple<int, int> compMove = gm.MakeMove(initial, destination);
                Console.WriteLine("The computer moved the piece on " + compMove.Item1 + " to " + compMove.Item2);
                Console.WriteLine(PrintBoard());
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("Computer won.");
                    break;
                }
            }

            Console.ReadLine();
        }

        private static GameManager gm;


        private static string PrintBoard()
        {
            string result = "";
            result += PrintRow(SubArray(gm.Board, 0, 3)) + "\n";
            result += "|\\|/|" + "\n";
            result += PrintRow(SubArray(gm.Board, 3, 3)) + "\n";
            result += "|/|\\|" + "\n";
            result += PrintRow(SubArray(gm.Board, 6, 3));
            return result;
        }

        private static string PrintRow(GameManager.Piece[] row)
        {
            string result = (row[0] == GameManager.Piece.Empty ? " " : row[0].ToString());
            for(int i = 1; i < row.Length; i++)
            {
                result += "-" + (row[i]==GameManager.Piece.Empty?" ":row[i].ToString());
            }
            return result;
        }

        private static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
