﻿using System.Collections.Generic;
using System.Collections;

namespace PolyOne.Components
{
    public class Coroutine : Component
    {
        public bool Finished { get; private set; }

        public bool RemoveOnComplete { get; set; } = true;

        private Stack<IEnumerator> enumerators;
        private float waitTimer;
        private bool ended;

        public Coroutine(IEnumerator functionCall)
            : base(true, false)
        {
            enumerators = new Stack<IEnumerator>();
            enumerators.Push(functionCall);
        }

        public Coroutine()
            : base(false, false)
        {
            enumerators = new Stack<IEnumerator>();
        }

        public override void Update()
        {
            ended = false;
            IEnumerator now = enumerators.Peek();

            if (waitTimer > 0)
            {
                waitTimer -= Engine.Engine.DeltaTime;
            }
            else if (now.MoveNext() == true)
            {
                if (now.Current is int)
                {
                    waitTimer = (int)now.Current;
                }

                if (now.Current is float)
                {
                    waitTimer = (float)now.Current;
                }
                else if (now.Current is IEnumerator)
                {
                    enumerators.Push(now.Current as IEnumerator);
                }
            }
            else if (ended == false)
            {
                enumerators.Pop();
                if (enumerators.Count == 0)
                {
                    Finished = true;
                    Active = false;
                    if (RemoveOnComplete == true)
                    {
                        RemoveSelf();
                    }
                }
            }
        }

        public void Cancel()
        {
            Active = false;
            Finished = true;
            waitTimer = 0;
            enumerators.Clear();

            ended = true;
        }

        public void Replace(IEnumerator functionCall)
        {
            Active = true;
            Finished = false;
            waitTimer = 0;
            enumerators.Clear();
            enumerators.Push(functionCall);

            ended = true;
        }

    }
}
