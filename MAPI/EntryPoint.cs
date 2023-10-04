#if STANDALONE

using MW.Diagnostics;

Log.P("STANDALONE Testing Environment for MW.\n");

#else

#error RUN MAPI IN A STANDALONE CONFIGURATION.

#endif // STANDALONE
