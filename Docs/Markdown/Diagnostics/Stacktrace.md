# MW.Diagnostics.Stacktrace
Provides custom stacktracing for code.

```cs
// Stacktrace up to this Point.
string Standalone = Stacktrace.Here();

// Prints the Stacktrace to the Unity Editor Console with Warning verbosity.
string Verbosity = Stacktrace.Here(EVerbosity.Warning);

// Prints the Stacktrace to the Unity Editor Console with a Message.
string Message = Stacktrace.Here("Something went wrong here, and not there!");
```