using System;
using System.Collections.Generic;
using System.Text;

namespace NPuzzle
{
   public class State : IComparable
    {
        public int[] mNodes;
        public int mSpaceIndex;
        public string mStateCode;
        public int mCostf;
        public int mCosth;
        public int mCostg;
        public Heuristic mHeuristic;
        public State mParent;

        public State(State parent, int[] nodes, Heuristic heuristic)
        {
            mNodes = nodes;
            mParent = parent;
            mHeuristic = heuristic;
            CalculateCost();
            mStateCode = GenerateStateCode();
        }

        public State(State parent, int[] nodes)
        {
            mNodes = nodes;
            mParent = parent;
            mHeuristic = parent.mHeuristic;
            CalculateCost();
            mStateCode = GenerateStateCode();
        }

        public override bool Equals(object obj)
        {
            State that = obj as State;

            return that != null && this.mStateCode.Equals(that.mStateCode);
        }

        public int CompareTo(object obj)
        {
            State that = obj as State;

            if (that != null)
            {
                return (this.mCostf).CompareTo(that.mCostf);
            }

            return 0;
        }

        public bool IsCostlierThan(State thatState)
        {
            return this.mCostg > thatState.mCostg;
        }

        public String GetStateCode()
        {
            return mStateCode;
        }

        private void CalculateCost()
        {
            if (mParent == null)
            {
                // We are at the first state - we assume we have been asked to be at this state, so no cost.
                mCostg = 0;
            }
            else
            {
                // Here, state transition cost is 1 unit. Since transition from one state to another is by moving he tile one step.
                mCostg = mParent.mCostg + 1;
            }

            // Heuristic cost
            mCosth = GetHeuristicCost();

            mCostf = mCosth + mCostg;
        }

        public int GetHeuristicCost()
        {
            if (mHeuristic == Heuristic.ManhattanDistance)
            {
                return GetManhattanDistanceCost();
            }
            else
            {
                return GetMisplacedTilesCost();
            }
        }

        /// <summary>
        /// Heuristic - No of misplaced tiles
        /// </summary>
        public int GetMisplacedTilesCost()
        {
            int heuristicCost = 0;

            for (int i = 0; i < mNodes.Length; i++)
            {
                int value = mNodes[i] - 1;

                // Space tile's value is -1
                if (value == -2)
                {
                    value = mNodes.Length - 1;
                    mSpaceIndex = i;
                }

                if (value != i)
                {
                    heuristicCost++;
                }
            }

            return heuristicCost;
        }

        /// <summary>
        /// Heuristic - Manhattan distance
        /// </summary>
        public int GetManhattanDistanceCost()
        {
            int heuristicCost = 0;
            int gridX = (int)Math.Sqrt(mNodes.Length);
            int idealX;
            int idealY;
            int currentX;
            int currentY;
            int value;

            for (int i = 0; i < mNodes.Length; i++)
            {
                value = mNodes[i] - 1;
                if (value == -2)
                {
                    value = mNodes.Length - 1;
                    mSpaceIndex = i;
                }

                if (value != i)
                {
                    // Misplaced tile
                    idealX = value % gridX;
                    idealY = value / gridX;

                    currentX = i % gridX;
                    currentY = i / gridX;

                    heuristicCost += (Math.Abs(idealY - currentY) + Math.Abs(idealX - currentX));
                }
            }

            return heuristicCost;
        }

        public String GenerateStateCode()
        {
            StringBuilder code = new StringBuilder();

            for (int i = 0; i < mNodes.Length; i++)
            {
                code.Append(mNodes[i] + "*");
            }

            return code.ToString().Trim(new char[] { '*' });
        }

        public int[] GetState()
        {
            int[] state = new int[mNodes.Length];
            Array.Copy(mNodes, state, mNodes.Length);

            return state;
        }

        public bool IsFinalState()
        {
            // If all tiles are at correct position, we are into final state.
            return mCosth == 0;
        }

        public State GetParent()
        {
            return mParent;
        }

        public List<State> GetNextStates(ref List<State> nextStates)
        {
            nextStates.Clear();
            State state;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                state = GetNextState(direction);

                if (state != null)
                {
                    nextStates.Add(state);
                }
            }

            return nextStates;
        }

        public State GetNextState(Direction direction)
        {
            int position;

            if (CanMove(direction, out position))
            {
                int[] nodes = new int[mNodes.Length];
                Array.Copy(mNodes, nodes, mNodes.Length);

                // Get new state nodes
                Swap(nodes, mSpaceIndex, position);

                return new State(this, nodes);
            }

            return null;
        }

        public void Swap(int[] nodes, int i, int j)
        {
            int t = nodes[i];
            nodes[i] = nodes[j];
            nodes[j] = t;
        }

        public bool CanMove(Direction direction, out int newPosition)
        {
            int newX = -1;
            int newY = -1;
            int gridX = (int)Math.Sqrt(mNodes.Length);
            int currentX = mSpaceIndex % gridX;
            int currentY = mSpaceIndex / gridX;
            newPosition = -1;

            switch (direction)
            {
                case Direction.Up:
                    {
                        // Can not move up if we are at the top
                        if (currentY != 0)
                        {
                            newX = currentX;
                            newY = currentY - 1;
                        }
                    }
                    break;

                case Direction.Down:
                    {
                        // Can not move down if we are the lowest level
                        if (currentY < (gridX - 1))
                        {
                            newX = currentX;
                            newY = currentY + 1;
                        }
                    }
                    break;

                case Direction.Left:
                    {
                        // Can not move left if we are at the left most position
                        if (currentX != 0)
                        {
                            newX = currentX - 1;
                            newY = currentY;
                        }
                    }
                    break;

                case Direction.Right:
                    {
                        // Can not move right if we are at the right most position
                        if (currentX < (gridX - 1))
                        {
                            newX = currentX + 1;
                            newY = currentY;
                        }
                    }
                    break;
            }

            if (newX != -1 && newY != -1)
            {
                newPosition = newY * gridX + newX;
            }

            return newPosition != -1;
        }

        public override string ToString()
        {
            return "State:" + mStateCode + ", g:" + mCostg + ", h:" + mCosth + ", f:" + mCostf;
        }
    }
    public enum Heuristic
    {
        MisplacedTiles,
        ManhattanDistance
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }
}
