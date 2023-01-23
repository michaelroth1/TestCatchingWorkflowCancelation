// See https://aka.ms/new-console-template for more information
using mitoSoft.Workflows;
using TestCatchingWorkflowCancelation;

Console.WriteLine("Start");

var stateMachine = new CatchCancelEventStateMachine();

var invoker = new Invoker(stateMachine);
invoker.Completed += (sender, args) =>
{
    stateMachine.Logger.Add("Ended");
};
invoker.Faulted += (sender, args) =>
{
    stateMachine.Logger.Add("Canceled");
};

stateMachine.Invoker = invoker;

var t = invoker.Invoke();

stateMachine.Logger.Add("Started");

Thread.Sleep(100);

stateMachine.Logger.Add("BeforeCancelRequested");
invoker.Cancel();

t.Wait();

Console.WriteLine(string.Join("->", stateMachine.Logger));

Console.ReadLine();