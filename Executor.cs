using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Forge.Invoker
{

    /// <summary>Event delegate execution</summary>
    public static class Executor
    {

        /// <summary>Invokes the specified event delegate.</summary>
        /// <param name="eventDelegate">The event delegate.</param>
        /// <param name="eventArgs">The event arguments.</param>
        /// <returns>List of result data of the subscribers</returns>
        public static List<object
#if NET6_0_OR_GREATER
            ?
#endif
            > Invoke(Delegate eventDelegate, params object[] eventArgs)
        {
            return Invoke(eventDelegate, null, eventArgs);
        }

        /// <summary>Invokes the specified event delegate.</summary>
        /// <param name="eventDelegate">The event delegate.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="eventArgs">The event arguments.</param>
        /// <returns>List of result data of the subscribers</returns>
        public static List<object
#if NET6_0_OR_GREATER
            ?
#endif
            > Invoke(Delegate eventDelegate, ILogger
#if NET6_0_OR_GREATER
            ?
#endif
            logger, params object[] eventArgs)
        {
            List<object
#if NET6_0_OR_GREATER
            ?
#endif
                > result = new List<object
#if NET6_0_OR_GREATER
            ?
#endif
                >();

            if (eventDelegate != null)
            {
                Delegate[] subscribers = eventDelegate.GetInvocationList();
                int index = 0;
                while (index < subscribers.Length)
                {
                    try
                    {
                        while (index < subscribers.Length)
                        {
                            Delegate subscriber = subscribers[index];
                            result.Add(subscriber.Method.Invoke(subscriber.Target, eventArgs));
                            index++;
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (logger != null) logger.LogError(ex, ex.Message);
                        index++;
                    }
                }
            }

            return result;
        }

    }

}