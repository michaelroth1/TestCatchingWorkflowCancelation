using mitoSoft.Workflows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using mitoSoft.Workflows.EventArgs;

namespace TestCatchingWorkflowCancelation
{
    internal class CatchCancelEventStateMachine : StateMachine
    {
        public Invoker Invoker { get; set; } = null!;

        public List<string> Logger { get; } = new List<string>();

        public CatchCancelEventStateMachine()
        {
            this
              .AddNode(new State("Start", () => Debug.WriteLine("Start")))
              .AddNode(new State("State1", () => Debug.WriteLine("State1")))
              .AddNode(new State("State2", () =>
              {
                  Thread.Sleep(1000);
                  Debug.WriteLine("State2");
              }))
              .AddNode(new State("End", () => Debug.WriteLine("End")))
              .AddEdge("Start", "State1", () => { return true; })
              .AddEdge("State1", "State2", () => { return true; })
              .AddEdge("State2", "End", () => { return true; });
        }

        public override void Invoke(CancellationToken cancellationToken, DateTime timeout)
        {
            if (this.Invoker != null)
            {
                this.Invoker.Faulted += Invoker_Faulted!;
            }

            base.Invoke(cancellationToken, timeout);
        }

        private void Invoker_Faulted(object sender, StateMachineFaultedEventArgs e)
        {
            //Do something here
            //...

            this.Logger.Add("CancelCatched");
        }
    }
}