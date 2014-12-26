// The MIT License (MIT)
// 
// Copyright (c) 2014 Werner Strydom
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Grammatika.StateMachines
{
    using System;
    using System.Collections.Generic;

    public class Nfa
    {
        public Nfa()
        {
            StartState = new State();
        }

        public class State
        {
            private readonly Dictionary<char, State> _transitions;

            public State()
            {
                _transitions = new Dictionary<char, State>();
            }

            public bool IsFinal
            {
                get { return _transitions.Count == 0; }
            }

            public void AddTransition(char c, State next = null)
            {
                _transitions.Add(c, next);
            }
        }

        public static Nfa FromString(string text)
        {
            var result = new Nfa();
            var previous = result.StartState;
            foreach (var c in text)
            {
                var next = new State();
                previous.AddTransition(c, next);
                previous = next;
            }
            return result;
        }

        public State StartState { get; set; }
    }
}
