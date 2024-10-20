using Rocket.Core.Logging;
using Rocket.Core.Utils;
using System;
using System.Threading;

namespace RestoreMonarchy.PlayerStats.Helpers
{
    internal static class ThreadHelper
    {
        internal static void RunAsynchronously(Action action, string exceptionMessage = null)
        {
            ThreadPool.QueueUserWorkItem((_) =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    RunSynchronously(() => Logger.LogException(e, exceptionMessage));
                }
            });
        }

        internal static void RunSynchronously(Action action, float delaySeconds = 0)
        {
            TaskDispatcher.QueueOnMainThread(action, delaySeconds);
        }
    }
}
