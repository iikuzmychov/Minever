namespace Minever.Client.ConsoleApplication;

internal static class ThreadSafeConsole
{
    internal static ConsoleColor ForegroundColor = ConsoleColor.White;
    internal static ConsoleColor BackgroundColor = ConsoleColor.Black;

    internal static void Write(object? value, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
    {
        lock (Console.Out)
        {
            var oldForegroundColor = Console.ForegroundColor;
            var oldBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            Console.Write(value);

            Console.ForegroundColor = oldForegroundColor;
            Console.BackgroundColor = oldBackgroundColor;
        }
    }

    internal static void WriteLine(object? value, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
    {
        if (value is null)
            Write(Environment.NewLine, backgroundColor, foregroundColor);
        else
            Write(value + Environment.NewLine, backgroundColor, foregroundColor);
    }

    internal static void Write(object? value) => Write(value, BackgroundColor, ForegroundColor);

    internal static void WriteLine(object? value) => WriteLine(value, BackgroundColor, ForegroundColor);
}
