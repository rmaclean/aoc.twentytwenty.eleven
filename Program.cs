using System;
using System.IO;
using System.Linq;
using System.Threading;

var room = (await File.ReadAllLinesAsync("data.txt")).Select(c => c.ToArray()).ToArray();
var lines = room.Length;
var chars = room[0].Length;
var printableRoom = room.ToPrintableString();

while (true)
{
    // Console.WriteLine();
    // Console.WriteLine(printableRoom);
    var next = ApplyRules();
    var nextPrint = next.ToPrintableString();
    if (nextPrint == printableRoom)
    {
        break;
    }

    printableRoom = nextPrint;
    room = next;
}

var occupiedCount = room.Select(l => l.Where(c => c == '#').Count()).Sum();
Console.WriteLine($"Occupied seats when stable {occupiedCount}");

char[][] ApplyRules()
{
    var nextState = new char[lines][];
    for (var lineIndex = 0; lineIndex < lines; lineIndex++)
    {
        nextState[lineIndex] = new char[chars];
        for (var charIndex = 0; charIndex < chars; charIndex++)
        {
            nextState[lineIndex][charIndex] = GetSeatState(lineIndex, charIndex);
        }
    }

    return nextState;
}

char GetSeatState(int lineIndex, int charIndex)
{
    var current = room[lineIndex][charIndex];
    if (current == '.') return '.';

    var occupiedSeatsAroundIt = 0;
    if (lineIndex > 0)
    {
        occupiedSeatsAroundIt += charIndex > 0 && room[lineIndex - 1][charIndex - 1] == '#' ? 1 : 0;
        occupiedSeatsAroundIt += room[lineIndex - 1][charIndex] == '#' ? 1 : 0;
        occupiedSeatsAroundIt += charIndex < chars - 1 && room[lineIndex - 1][charIndex + 1] == '#' ? 1 : 0;
    }

    occupiedSeatsAroundIt += charIndex > 0 && room[lineIndex][charIndex - 1] == '#' ? 1 : 0;
    occupiedSeatsAroundIt += charIndex < chars - 1 && room[lineIndex][charIndex + 1] == '#' ? 1 : 0;

    if (lineIndex < lines - 1)
    {
        occupiedSeatsAroundIt += charIndex > 0 && room[lineIndex + 1][charIndex - 1] == '#' ? 1 : 0;
        occupiedSeatsAroundIt += room[lineIndex + 1][charIndex] == '#' ? 1 : 0;
        occupiedSeatsAroundIt += charIndex < chars - 1 && room[lineIndex + 1][charIndex + 1] == '#' ? 1 : 0;
    }

    if (current == 'L' && occupiedSeatsAroundIt == 0)
    {
        return '#';
    }

    if (current == '#' && occupiedSeatsAroundIt >= 4)
    {
        return 'L';
    }

    return current;
}

public static class ExtensionMethods
{
    public static string ToPrintableString(this char[][] input)
    {
        return input.Aggregate("", (currLine, nextLine) => currLine += Environment.NewLine + new string(nextLine));
    }
}