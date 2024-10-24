using RestoreMonarchy.PlayerStats.Databases;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using System;
using System.Threading;
using Action = System.Action;

namespace RestoreMonarchy.PlayerStats.Helpers
{
    internal static class ThreadHelper
    {
        internal static void RunAsynchronously(Action action, string exceptionMessage = null)
        {
            PlayerStatsPlugin pluginInstance = PlayerStatsPlugin.Instance;
            if (pluginInstance.Database is not MySQLDatabase)
            {
                action.Invoke();
                return;
            }

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
