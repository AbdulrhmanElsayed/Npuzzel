using System;
using System.Collections.Generic;
using System.Text;

namespace NPuzzle
{
    class MinPQ<T> where T : IComparable
    {
        /// <summary>
        /// this intialize attributes of class
        /// </summary>
        public int Counter;
        public T[] PuzzleArray;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="recievedSize"></param>

        public MinPQ(int recievedSize)
        {
            PuzzleArray = new T[recievedSize + 1];
            Counter = 0;
        }
        /// <summary>
        /// intialize Enqueue Method that add node of class
        /// </summary>
        /// <param name="node"></param>
        public void Enqueue(T node)
        {
            if (Counter == PuzzleArray.Length - 1)
            {
                IncreaseSize(PuzzleArray.Length * 3);
            }

            PuzzleArray[++Counter] = node;
            heapify_UP(Counter);
        }
        /// <summary>
        /// extend size of array by 3 times
        /// </summary>
        /// <param name="recievedSize"></param>
        public void IncreaseSize(int recievedSize)
        {
            T[] temp = new T[recievedSize + 1];
            int i = 0;
            while (++i <= Counter)
            {
                temp[i] = PuzzleArray[i];
                PuzzleArray[i] = default(T);
            }

            PuzzleArray = temp;
        }

        /// <summary>
        /// put current node in the right position
        /// </summary>
        /// <param name="node"></param>
        public void heapify_UP(int node)
        {
            int var;

            while (node / 2 > 0)
            {
                var = node / 2;

                if (!FindLess(node, var))
                {
                    break;
                }
                SwapNodes(node, var);
                node = var;
            }
        }
        /// <summary>
        /// check if current node is less than parent.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public bool FindLess(int i, int j)
        {


            return PuzzleArray[i].CompareTo(PuzzleArray[j]) < 0;
        }
        /// <summary>
        /// swap current node and parent.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        public void SwapNodes(int node, int parent)
        {
            T temp = PuzzleArray[parent];
            PuzzleArray[parent] = PuzzleArray[node];
            PuzzleArray[node] = temp;
        }
        /// <summary>
        /// make large number in bottom
        /// </summary>
        /// <param name="node"></param>

        /// <summary>
        /// check is Empty or not
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Counter == 0;
        }

        /// <summary>
        /// get root of tree and decrease 1 from size
        /// </summary>
        /// <returns></returns>
        public T ExtractMin()
        {
            if (!IsEmpty())
            {
                T var = PuzzleArray[1];
                PuzzleArray[1] = PuzzleArray[Counter];
                PuzzleArray[Counter--] = default(T);
                heapify_down(1);
                return var;
            }

            return default(T);
        }
        /// <summary>
        /// put new root of tree in the right position
        /// </summary>
        /// <param name="node"></param>
        public void heapify_down(int node)
        {
            int variable;
            while (node * 2 <= Counter)
            {
                variable = node * 2;

                if (variable + 1 <= Counter && FindLess(variable + 1, variable))
                {
                    variable = variable + 1;
                }

                if (!FindLess(variable, node))
                {
                    break;
                }

                SwapNodes(node, variable);
                node = variable;
            }
        }



        /// <summary>
        /// return node and value in node
        /// </summary>
        /// <param name="var"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public T FindNode(T var, out int node)
        {
            node = -1;
            if (!IsEmpty())
            {
                int i = 0;

                while (++i <= Counter)
                {
                    if (PuzzleArray[i].Equals(var))
                    {
                        node = i;
                        return PuzzleArray[i];
                    }
                }
            }

            return default(T);
        }
        /// <summary>
        /// remove node from tree then put the last element in right position
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(int node)
        {
            if (node > 0 && node <= Counter)
            {
                PuzzleArray[node] = PuzzleArray[Counter];
                PuzzleArray[Counter--] = default(T);
                heapify_down(node);
            }
        }
    }
}
