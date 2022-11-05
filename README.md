# Forge.Invoker
Event delegate execution helper library

Have you ever felt that, you got an exception, even if the issue is not on your side? 
Typical situation, when you are a library developer, raise an event, and one or more subscribers throw an exception. The issue get back to you, you should handle it. Thanks...

One more problem with this: if I have five subscribers on my event, and the third one throws an exception, the last two subscriber's event handlers will not run, because of the issue. That is not expected behaviour.

What is the solution? Add a protected stack, a try/catch block around the subscribers, catch the exception, write a log entry, than continue to send the event
to the others.

This is a very small and generic library to achieve that.


## Installing

To install the package add the following line to you csproj file replacing x.x.x with the latest version number:

```
<PackageReference Include="Forge.Invoker" Version="x.x.x" />
```

You can also install via the .NET CLI with the following command:

```
dotnet add package Forge.Invoker
```

If you're using Visual Studio you can also install via the built in NuGet package manager.

## Usage / Example

In the following example, we have five subscribers on an event. In the middle, there is a subscriber, which simulates an issue,
and throws an exception. The Invoker library handle this properly, protect the event sender and allow the last two subscribers
to get the event also.

```c#
using Forge.Invoker;

namespace InvokerExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EventOwner owner = new EventOwner();

            Apple subscriber1 = new Apple(owner);
            Apple subscriber2 = new Apple(owner);
            Worm subscriber3 = new Worm(owner);
            Apple subscriber4 = new Apple(owner);
            Apple subscriber5 = new Apple(owner);

            owner.RaiseEvent();
            Console.ReadKey();
        }
    }

    class EventOwner
    {
        public event EventHandler<EventArgs> OnEvent;

        public void RaiseEvent()
        {
            // The event, and the parameters (object? sender, EventArgs e)
            Executor.Invoke(OnEvent, this, EventArgs.Empty);
        }
    }

    internal abstract class Base
    {
        private static int _counter = 0;

        private readonly int _instanceCounter = 0;
        private readonly EventOwner _eventOwner;

        public Base(EventOwner eventOwner)
        {
            if (eventOwner == null) throw new ArgumentNullException(nameof(eventOwner));
            _eventOwner = eventOwner;
            _instanceCounter = _counter++;
            eventOwner.OnEvent += OnEventHandler;
        }

        protected virtual void OnEventHandler(object? sender, EventArgs e)
        {
            Console.WriteLine($"OnEventHandler, instance: {_instanceCounter}");
        }
    }

    internal class Apple : Base
    {
        public Apple(EventOwner eventOwner) : base(eventOwner)
        {
        }
    }

    internal class Worm : Base
    {
        public Worm(EventOwner eventOwner) : base(eventOwner)
        {
        }

        protected override void OnEventHandler(object? sender, EventArgs e)
        {
            throw new Exception("Simulating, something went wrong");
        }
    }

}
```

The output on the console will be:

```
OnEventHandler, instance: 0
OnEventHandler, instance: 1
OnEventHandler, instance: 3
OnEventHandler, instance: 4
```

The third subscriber is missing from the output logging, because it has failed. The others are fine.

The Executor.Invoke method also can receive an ILogger in that case of an exception, and log the details.
