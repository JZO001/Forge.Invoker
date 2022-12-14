using Forge.Logging.Abstraction;
using System;
using System.Collections.Generic;

namespace Forge.Invoker
{

    /// <summary>Event delegate execution</summary>
    public static class Executor
    {

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(Executor));

        /// <summary>Invokes the specified event delegate.</summary>
        /// <param name="eventDelegate">The event delegate.</param>
        /// <param name="eventArgs">The event arguments.</param>
        /// <returns>List of result data of the subscribers</returns>
        public static List<object
#if NETCOREAPP3_1_OR_GREATER
            ?
#endif
            > Invoke(Delegate
#if NETCOREAPP3_1_OR_GREATER
            ?
#endif
            eventDelegate, params object[]
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            eventArgs)
        {
            List<object
#if NETCOREAPP3_1_OR_GREATER
            ?
#endif
                > result = new List<object
#if NETCOREAPP3_1_OR_GREATER
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
                        LOGGER.Error(ex.Message, ex);
                        index++;
                    }
                }
            }

            return result;
        }

    }

}