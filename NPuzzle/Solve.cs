using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace NPuzzle
{
    class Solve
    {
        #region Fields
        public Stopwatch mStopWatch;

        #endregion Fields

        #region Methods

        public Solve()
        {
            mStopWatch = new Stopwatch();
        }



        public void Start(int[] nodes, Heuristic heuristic)
        {
            int openStateIndex;
            int stateCount = -1;
            State currentState = null;
            List<State> nextStates = new List<State>();
            HashSet<String> openStates = new HashSet<string>();
            MinPQ<State> openStateQueue = new MinPQ<State>(nodes.Length * 3);
            Dictionary<String, State> closedQueue = new Dictionary<string, State>(nodes.Length * 3);

            State state = new State(null, nodes, heuristic);
            openStateQueue.Enqueue(state);
            openStates.Add(state.GetStateCode());

            StartMeasure();

            while (!openStateQueue.IsEmpty())
            {
                currentState = openStateQueue.ExtractMin();
                openStates.Remove(currentState.GetStateCode());

                stateCount++;

                // Is this final state
                if (currentState.IsFinalState())
                {
                    EndMeasure(stateCount);
                    break;
                }

                // Look into next state
                currentState.GetNextStates(ref nextStates);

                if (nextStates.Count > 0)
                {
                    State closedState;
                    State openState;
                    State nextState;

                    for (var i = 0; i < nextStates.Count; i++)
                    {
                        closedState = null;
                        openState = null;
                        nextState = nextStates[i];

                        if (openStates.Contains(nextState.GetStateCode()))
                        {
                            // We already have same state in the open queue. 
                            openState = openStateQueue.FindNode(nextState, out openStateIndex);

                            if (openState.IsCostlierThan(nextState))
                            {
                                // We have found a better way to reach at this state. Discard the costlier one
                                openStateQueue.RemoveNode(openStateIndex);
                                openStateQueue.Enqueue(nextState);
                            }
                        }
                        else
                        {
                            // Check if state is in closed queue
                            String stateCode = nextState.GetStateCode();

                            if (closedQueue.TryGetValue(stateCode, out closedState))
                            {
                                // We have found a better way to reach at this state. Discard the costlier one
                                if (closedState.IsCostlierThan(nextState))
                                {
                                    closedQueue.Remove(stateCode);
                                    closedQueue[stateCode] = nextState;
                                }
                            }
                        }

                        // Either this is a new state, or better than previous one.
                        if (openState == null && closedState == null)
                        {
                            openStateQueue.Enqueue(nextState);
                            openStates.Add(nextState.GetStateCode());
                        }
                    }

                    closedQueue[currentState.GetStateCode()] = currentState;
                }
            }

            if (currentState != null && !currentState.IsFinalState())
            {
                // No solution
                currentState = null;
            }

            PuzzleSolved(currentState,stateCount);
            OnFinalState(currentState);
        }

        public void StartMeasure()
        {
            mStopWatch.Reset();
            mStopWatch.Start();
        }

        public void EndMeasure(int stateCount)
        {
            mStopWatch.Stop();
        }

        public void OnFinalState(State state)
        {
            if (state != null)
            {
                // We have a solution for this puzzle
                // Backtrac to the root of the path in the tree
                Stack<State> path = new Stack<State>();

                while (state != null)
                {
                    path.Push(state);
                    state = state.GetParent();
                }


            }
            else
            {
                // No solution
                
            }
        }

        public void PuzzleSolved(State state,int states)
        {
            Console.WriteLine("Min Movements#: " + state.mCostf.ToString()+"\n"+"costed Time# : "+ (int)mStopWatch.ElapsedMilliseconds+"\n"+"Number of States# : "+ states);



        }

        #endregion Methods
    }
}
