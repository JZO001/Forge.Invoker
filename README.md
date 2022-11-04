# Forge.Invoker
Event delegate execution helper library

Have you ever felt that, you got an exception, even if the issue is not on your side? Typical situation, when you are a library developer and raise an event,
and one or more subscribers throw an exception. The issue get back to you, you should to handle it. You got the exception, but who cares? 
One more problem with this: if I have five subscribers on my event,
and the third one throws an exception, the last two subscriber's event handlers will not run, because of the issue. That is not expected behaviour.

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
