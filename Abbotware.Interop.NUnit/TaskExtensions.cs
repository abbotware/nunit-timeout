// -----------------------------------------------------------------------
// <copyright file="TaskExtensions.cs" company="Abbotware, LLC">
// Copyright © Abbotware, LLC 2012-2018. All rights reserved
// </copyright>
// <author>Anthony Abate</author>
// -----------------------------------------------------------------------

namespace Abbotware.Core.Extensions
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Task Extension methods
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Timeout a task after a timeout
        /// </summary>
        /// <remarks>https://blogs.msdn.microsoft.com/pfxteam/2011/11/10/crafting-a-task-timeoutafter-method/</remarks>
        /// <param name="task">wrapped task</param>
        /// <param name="timeout">timeout value</param>
        /// <returns>awaitable task</returns>
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(timeout)).ConfigureAwait(false))
            {
                await task.ConfigureAwait(false);
            }
            else
            {
                throw new TimeoutException();
            }
        }
    }
}