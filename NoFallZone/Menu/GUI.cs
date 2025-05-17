namespace NoFallZone.Menu;
public class GUI
{

    public static void DrawWindow(string header, int fromLeft, int fromTop, List<string> graphic, int maxLineWidth = 40)
    {
        List<string> lines = new List<string>();

        // Wrappa varje rad i input
        foreach (var line in graphic)
        {
            if (!string.IsNullOrEmpty(line))
                lines.AddRange(WrapText(line, maxLineWidth));
            else
                lines.Add("");
        }

        // Hitta bredd
        int width = lines.Max(l => l.Length);
        if (width < header.Length + 4)
            width = header.Length + 4;

        // Rita övre delen
        Console.SetCursorPosition(fromLeft, fromTop);
        if (!string.IsNullOrEmpty(header))
        {
            Console.Write('┌' + " ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(header);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + new string('─', width - header.Length) + '┐');
        }
        else
        {
            Console.Write('┌' + new string('─', width + 2) + '┐');
        }
        Console.WriteLine();

        // Rita rader
        for (int j = 0; j < lines.Count; j++)
        {
            Console.SetCursorPosition(fromLeft, fromTop + j + 1);
            Console.WriteLine('│' + " " + lines[j] + new string(' ', width - lines[j].Length + 1) + '│');
        }

        // Rita nedre ramen
        Console.SetCursorPosition(fromLeft, fromTop + lines.Count + 1);
        Console.Write('└' + new string('─', width + 2) + '┘');
    }


    private static List<string> WrapText(string text, int maxWidth)
    {
        var wrapped = new List<string>();
        while (text.Length > maxWidth)
        {
            int wrapAt = text.LastIndexOf(' ', maxWidth);
            if (wrapAt <= 0) wrapAt = maxWidth;
            wrapped.Add(text.Substring(0, wrapAt).Trim());
            text = text.Substring(wrapAt).TrimStart();
        }
        if (!string.IsNullOrEmpty(text))
            wrapped.Add(text);
        return wrapped;
    }

}
