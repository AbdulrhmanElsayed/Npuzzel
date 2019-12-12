using System;
using System.Collections.Generic;
using System.Text;

namespace NPuzzle
{
    class Solvability
    {
        public List<int> findEmptySquare(int size, int[,] puzzleArray)
        {
            List<int> index = new List<int>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (puzzleArray[i, j] == -1)
                    {

                        index.Add(size - i);
                        index.Add(j);
                        return index;
                    }
                }
            }
            return index;
        }
        public bool solvabitlity(int size, int[] puzzleArray, int row, int column)
        {
            int NumOfInversion = 0;
            for (int i = 0; i < size - 1; i++)
            {
                if (puzzleArray[i] == -1 && i + 1 == size - 1)
                {
                    break;
                }
                if (puzzleArray[i] == -1)
                    continue;
                for (int j = i + 1; j < size; j++)
                {
                    if (puzzleArray[i] > puzzleArray[j] && puzzleArray[j] != -1)
                    {
                        NumOfInversion++;
                    }
                }

            }
            if (size % 2 != 0)
            {
                if (NumOfInversion % 2 == 0)
                    return true;

            }
            else if (size % 2 == 0)
            {
                if (row % 2 == 0)
                {
                    if (NumOfInversion % 2 != 0)
                        return true;
                }
                else if (row % 2 != 0)
                {
                    if (NumOfInversion % 2 == 0)
                        return true;
                }

            }
            return false;

        }
    }
}
