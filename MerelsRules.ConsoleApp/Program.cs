using System;

namespace MerelsRules.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            gm = GameManager.GetInstance();

            Console.WriteLine("Welcome to Merels Rules!");
            Console.WriteLine("Would you like to move first? (y/n)");
            String response = Console.ReadLine();
            if (response.Equals("y"))
            {
                PlayPlayerFirst();
            }
            else
            {
                PlayComputerFirst();
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

        public static void PlayPlayerFirst()
        {
            //placement -- player goes first
            Console.WriteLine(PrintBoard());
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Where do you want to place your piece (0-9)?");
                int loc = int.Parse(Console.ReadLine());
                Console.WriteLine();
                //player placement
                gm.MakeMove(-1, loc);
                Console.WriteLine("Your placement:");
                Console.WriteLine(PrintBoard());
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("You won!");
                    return;
                }
                //computer placement
                Tuple<int, int> compMove = gm.MakeMove(-1, loc);
                Console.WriteLine("The computer placed a piece on " + compMove.Item2);
                Console.WriteLine(PrintBoard());
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("Computer won.");
                    return;
                }
            }

            Console.WriteLine("\nBeginning Game\n");
            Console.WriteLine(PrintBoard());

            //moves -- player goes first
            while (true)
            {
                Console.WriteLine("What piece do you want to move (0-9)? ");
                int initial = int.Parse(Console.ReadLine());
                Console.WriteLine("Where do you want to move the piece (0-9)? ");
                int destination = int.Parse(Console.ReadLine());
                Console.WriteLine();
                //player move
                gm.MakeMove(initial, destination);
                Console.WriteLine("Your move:");
                Console.WriteLine(PrintBoard() + "\n");
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("You won!");
                    return;
                }
                //computer move
                Tuple<int, int> compMove = gm.MakeMove(initial, destination);
                Console.WriteLine("The computer moved the piece on " + compMove.Item1 + " to " + compMove.Item2);
                Console.WriteLine(PrintBoard());
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("Computer won.");
                    return;
                }
            }
        }

        public static void PlayComputerFirst()
        {
            //placement -- computer goes first
            Console.WriteLine("The computer placed a piece on 4");
            gm.Board[4] = GameManager.Piece.X;
            gm.PieceLocations[3] = 4;
            Console.WriteLine(PrintBoard());
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Where do you want to place your piece?");
                int loc = int.Parse(Console.ReadLine());
                Console.WriteLine();
                //player placement
                gm.MakeMove(-1, loc);
                Console.WriteLine("Your placement:");
                Console.WriteLine(PrintBoard());
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("You won!");
                    Console.ReadLine();
                    return;
                }
                //computer placement
                Tuple<int, int> compMove = gm.MakeMove(-1, loc);
                Console.WriteLine("The computer placed a piece on " + compMove.Item2);
                Console.WriteLine(PrintBoard());
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("Computer won.");
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine("Where do you want to place your piece?");
            int location = int.Parse(Console.ReadLine());
            Console.WriteLine();
            gm.MakeMove(-1, location);
            Console.WriteLine("Your placement:");
            Console.WriteLine(PrintBoard());
            if (gm.CheckGameEnd())
            {
                Console.WriteLine("You won!");
                Console.ReadLine();
                return;
            }

            //moves -- computer goes first
            while (true)
            {
                //computer move
                Tuple<int, int> compMove = gm.MakeMove(0, 0);
                Console.WriteLine("The computer moved the piece on " + compMove.Item1 + " to " + compMove.Item2);
                Console.WriteLine(PrintBoard());
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("Computer won.");
                    Console.ReadLine();
                    return;
                }
                //player move
                Console.WriteLine("What piece do you want to move (0-9)? ");
                int initial = int.Parse(Console.ReadLine());
                Console.WriteLine("Where do you want to move the piece (0-9)? ");
                int destination = int.Parse(Console.ReadLine());
                Console.WriteLine();
                gm.MakeMove(initial, destination);
                Console.WriteLine("Your move:");
                Console.WriteLine(PrintBoard() + "\n");
                if (gm.CheckGameEnd())
                {
                    Console.WriteLine("You won!");
                    Console.ReadLine();
                    return;
                }
            }
        } 
    }
}
