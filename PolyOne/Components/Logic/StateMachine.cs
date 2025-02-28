﻿using System;
using System.Collections;

using PolyOne.Utility;

namespace PolyOne.Components
{
    public class StateMachine : Component
    {
        private int state;
        private Action[] begins;
        private Func<int>[] updates;
        private Action[] ends;
        private Func<IEnumerator>[] coroutines;
        private Coroutine currentCoroutine;


        public bool ChangedStates { get; set; }

        public bool Log { get; set; }

        public int PreviousState { get; set; }

        public StateMachine(int maxStates = 10) :
            base(true, false)
        {
            PreviousState = state = -1;

            begins = new Action[maxStates];
            updates = new Func<int>[maxStates];
            ends = new Action[maxStates];
            coroutines = new Func<IEnumerator>[maxStates];

            currentCoroutine = new Coroutine();
            currentCoroutine.RemoveOnComplete = false;
        }

        public override void Added(Entity entity)
        {
            base.Added(entity);

            if (state == -1)
            {
                State = 0;
            }
        }

        public int State
        {
            get { return state; }
            set
            {
#if DEBUG
                if (value >= updates.Length || value < 0) {
                    throw new Exception("StateMachine state out of range");
                }
#endif

                if (state != value)
                {
                    if (Log == true) {
                        PolyDebug.Log("Enter State " + value + " (leaving " + state + ")");
                    }
                    ChangedStates = true;

                    if (state != -1 && ends[state] != null) {
                        ends[state]();
                    }
                    PreviousState = state;
                    state = value;

                    if (begins[state] != null) {
                        begins[state]();
                    }

                    if (coroutines[state] != null)
                    {
                        if (Log == true) {
                            PolyDebug.Log("Starting coroutine " + state);
                        }
                        currentCoroutine.Replace(coroutines[state]());
                    }
                    else {
                        currentCoroutine.Cancel();
                    }
                }
            }
        }

        public void SetCallbacks(int state, Func<int> onUpdate, Func<IEnumerator> coroutine = null, Action onEnterState = null, Action onLeaveState = null)
        {
            updates[state] = onUpdate;
            begins[state] = onEnterState;
            ends[state] = onLeaveState;
            coroutines[state] = coroutine;
        }

        public void ReflectState(Entity from, int index, string name)
        {
            updates[index] = (Func<int>)PolyDebug.GetMethod<Func<int>>(from, name + "Update");
            begins[index] = (Action)PolyDebug.GetMethod<Action>(from, name + "Begin");
            ends[index] = (Action)PolyDebug.GetMethod<Action>(from, name + "End");
            coroutines[index] = (Func<IEnumerator>)PolyDebug.GetMethod<Func<IEnumerator>>(from, name + "Coroutine");
        }

        public override void Update()
        {
            ChangedStates = false;

            if (updates[state] != null)
            {
                State = updates[state]();
            }
            if (currentCoroutine.Active == true)
            {
                currentCoroutine.Update();
                if (ChangedStates != false && currentCoroutine.Finished == true)
                {
                    PolyDebug.Log("Coroutine " + state + " finished");
                }
            }
        }

        static public implicit operator int(StateMachine s)
        {
            return s.state;
        }

        public void LogAllStates()
        {
            for (int i = 0; i < updates.Length; i++)
            {
                LogState(i);
            }
        }

        public void LogState(int index)
        {
            PolyDebug.Log("State " + index + ": "
                + (updates[index] != null ? "U" : "")
                + (begins[index] != null ? "B" : "")
                + (ends[index] != null ? "E" : "")
                + (coroutines[index] != null ? "C" : ""));
        }
    }
}
