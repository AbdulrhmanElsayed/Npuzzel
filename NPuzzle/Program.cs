using System;
using System.Collections.Generic;
using System.IO;

namespace NPuzzle
{

    class Program
    {
        /// <summary>
        /// declare for innput
        /// </summary>
        public static int NumberTestCases;
        public static int[,] PuzzleArray = null;
        public static int[] PuzzleArray1D = null;
        public static string[] PuzzleElement;
        public static StreamReader sr;
        /// <summary>
        /// declare solve puzzle
        /// </summary>
        public static Solve mStrategy;
        public static Heuristic mHeuristic;
        public static Solvability obj;
        public static TextReader origConsole;
        static void Main(string[] args)
        {
             obj = new Solvability();
            
            origConsole = Console.In;
            Console.WriteLine("NPuzzle:\n[1] solve Puzzle A* With Hamming\n[2] solve Puzzle A* with Manhattan\n[3] solve Puzzle with DFS\n[4] solve Puzzle with BFS");
            Console.Write("\nEnter your choice [1-3]: ");
            char choice = (char)Console.ReadLine()[0];
            if (choice == '1')
            {
                #region Hamming Fn
                StartSolve(Heuristic.MisplacedTiles);
               
                #endregion
            }
            else if (choice == '2')
            {
                #region Manhattan FN
                StartSolve(Heuristic.ManhattanDistance);
                #endregion
            }
            else if (choice == '3')
            {
                #region BFS

                #endregion 
            }
            else if (choice == '4')
            {
                #region DFS

                #endregion 
            }
            ////

        }
        public static void StartSolve(Heuristic mHeuristic)
        {
            sr = new StreamReader(@"C:\Users\abdul\OneDrive\Desktop\NPuzzle\NPuzzle\testCases.txt");
            Console.SetIn(sr);
                PuzzleElement = Console.ReadLine().Split(' ');
                int size = int.Parse(PuzzleElement[0]);
                Console.ReadLine();
                PuzzleArray = new int[size, size];
                PuzzleArray1D = new int[size * size];
                int counter = 0;
                for (int j = 0; j < size; j++)
                {
                    PuzzleElement = Console.ReadLine().Split(' ');
                    for (int z = 0; z < size; z++)
                    {
                    if (int.Parse(PuzzleElement[z]) == 0)
                    {
                        PuzzleArray[j, z] = -1;
                        PuzzleArray1D[counter] = -1;
                        
                    }
                    else
                    {
                        PuzzleArray[j, z] = int.Parse(PuzzleElement[z]);
                        PuzzleArray1D[counter] = int.Parse(PuzzleElement[z]);
                        
                    }
                    counter++;
                    }
                }
                Console.WriteLine("TestCase # : ");
                ////////////////////////////////////////////////////////////// send index of raw and col
                List<int> ls = obj.findEmptySquare(size, PuzzleArray);
                int row = ls[0];
                int col = ls[1];
                Console.WriteLine("Size " + (size).ToString() + ":");
                if (obj.solvabitlity(size * size, PuzzleArray1D, row, col))
                {
                    Console.WriteLine("Solvable ");
                    mStrategy = new Solve();
                    Console.WriteLine("Min NUmber of Moves Is");
                    mStrategy.Start(PuzzleArray1D, mHeuristic);

                }
                else if (!obj.solvabitlity(size * size, PuzzleArray1D, row, col))
                {
                    Console.WriteLine("Not Solvable ");
                     
            }
               /* for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        Console.Write(PuzzleArray[y, z] + " ");
                    }
                    Console.WriteLine();
                }*/
                Console.WriteLine("***********************************************************");
            

        }
    }
   
}
