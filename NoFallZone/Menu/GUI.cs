namespace NoFallZone.Menu;
public class GUI
{
    public static void DrawWindow(string header, int fromLeft, int fromTop, List<string> graphic, int maxLineLength = 50)
    {
        List<string> wrappedLines = new List<string>();
        foreach (string line in graphic)
        {
            if (line == null || line.Length <= maxLineLength)
            {
                wrappedLines.Add(line ?? "");
            }
            else
            {
                for (int i = 0; i < line.Length; i += maxLineLength)
                {
                    wrappedLines.Add(line.Substring(i, Math.Min(maxLineLength, line.Length - i)));
                }
            }
        }

        string[] graphics = wrappedLines.ToArray();

        int width = graphics.Max(g => g?.Length ?? 0);
        if (width < header.Length + 4) width = header.Length + 4;

        Console.SetCursorPosition(fromLeft, fromTop);
        if (header != "")
        {
            Console.Write('┌' + " ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(header);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + new String('─', width - header.Length) + '┐');
        }
        else
        {
            Console.Write('┌' + new String('─', width + 2) + '┐');
        }
        Console.WriteLine();

        for (int j = 0; j < graphics.Length; j++)
        {
            Console.SetCursorPosition(fromLeft, fromTop + j + 1);
            Console.WriteLine('│' + " " + graphics[j] + new String(' ', width - graphics[j].Length + 1) + '│');
        }

        Console.SetCursorPosition(fromLeft, fromTop + graphics.Length + 1);
        Console.Write('└' + new String('─', width + 2) + '┘');
    }
}
