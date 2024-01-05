using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.CodedWorkflows;
using UiPath.Activities.Contracts;

namespace practiceGameChallenges
{
    public class WorkflowRunnerService
    {
        private readonly Func<string, IDictionary<string, object>, TimeSpan?, bool, InvokeTargetSession, IDictionary<string, object>> _runWorkflowHandler;
        public WorkflowRunnerService(Func<string, IDictionary<string, object>, TimeSpan?, bool, InvokeTargetSession, IDictionary<string, object>> runWorkflowHandler)
        {
            _runWorkflowHandler = runWorkflowHandler;
        }

        /// <summary>
        /// Invokes the 1_tic_tac_toe_win/solution1.xaml
        /// </summary>
        public void solution1()
        {
            var result = _runWorkflowHandler(@"1_tic_tac_toe_win\solution1.xaml", new Dictionary<string, object>{}, default, default, default);
        }

        /// <summary>
        /// Invokes the Main.xaml
        /// </summary>
        public void Main()
        {
            var result = _runWorkflowHandler(@"Main.xaml", new Dictionary<string, object>{}, default, default, default);
        }

        /// <summary>
        /// Invokes the testingTicTacToe.xaml
        /// </summary>
        public void testingTicTacToe()
        {
            var result = _runWorkflowHandler(@"testingTicTacToe.xaml", new Dictionary<string, object>{}, default, default, default);
        }
    }
}