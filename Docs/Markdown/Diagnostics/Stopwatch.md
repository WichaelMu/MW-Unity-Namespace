# MW.Diagnostics.Stopwatch
Track the execution time of code.

```cs
Stopwatch SW = new Stopwatch();
ExecuteUselessFunction();

long ElapsedTime = SW.Stop();

SW.Restart();
ExecuteUselessFunction();

long TimeOnSW = SW.Time();
Log.P(SW.TimeInSeconds());

void ExecuteUselessFunction()
{
	for (int i = 0; i < 1 << 31 - 1; ++i)
		; // Do literally nothing.
}
```