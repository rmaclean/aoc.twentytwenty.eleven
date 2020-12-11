using System;
using System.IO;
using System.Linq;

var room = (await File.ReadAllLinesAsync("data.txt")).Select(c => c.ToArray()).ToArray();
var lines = room.Length;
var chars = room[0].Length;
var printableRoom = room.ToPrintableString();
var part2Logic = new Func<int, int, int, LocationState>[] {
    CheckNW, CheckN, CheckNE,
    CheckW,          CheckE,
    CheckSW, CheckS, CheckSE,
};

while (true)
{
    Console.WriteLine();
    Console.WriteLine(printableRoom);
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

    var range = 1;
    var positionStates = new[] {
        LocationState.Unknown, LocationState.Unknown, LocationState.Unknown,
        LocationState.Unknown,                        LocationState.Unknown,
        LocationState.Unknown, LocationState.Unknown, LocationState.Unknown,
    };

    while (true)
    {
        if (positionStates.All(p => p == LocationState.EmptySeat || p == LocationState.OccupiedSeat || p == LocationState.StopInstruction))
        {
            break;
        }

        for (var index = 0; index < 8; index++)
        {
            if (positionStates[index] == LocationState.Floor || positionStates[index] == LocationState.Unknown) positionStates[index] = part2Logic[index](lineIndex, charIndex, range);
        }
        
        range++;
    }

    var occupiedSeatsAroundIt = positionStates.Where(p => p == LocationState.OccupiedSeat).Count();

    if (current == 'L' && occupiedSeatsAroundIt == 0)
    {
        return '#';
    }

    if (current == '#' && occupiedSeatsAroundIt >= 5)
    {
        return 'L';
    }

    return current;
}

LocationState CheckNW(int lineIndex, int charIndex, int range)
{
    if (charIndex - range < 0 || lineIndex - range < 0)
    {
        return LocationState.StopInstruction;
    }

    return room[lineIndex - range][charIndex - range].ToLocationState();
}

LocationState CheckN(int lineIndex, int charIndex, int range)
{
    if (lineIndex - range < 0)
    {
        return LocationState.StopInstruction;
    }

    return room[lineIndex - range][charIndex].ToLocationState();
}

LocationState CheckNE(int lineIndex, int charIndex, int range)
{
    if (charIndex + range >= chars || lineIndex - range < 0)
    {
        return LocationState.StopInstruction;
    }

    return room[lineIndex - range][charIndex + range].ToLocationState();
}

LocationState CheckW(int lineIndex, int charIndex, int range)
{
    if (charIndex - range < 0)
    {
        return LocationState.StopInstruction;
    }

    return room[lineIndex][charIndex - range].ToLocationState();
}

LocationState CheckE(int lineIndex, int charIndex, int range)
{
    if (charIndex + range >= chars)
    {
        return LocationState.StopInstruction;
    }

    return room[lineIndex][charIndex + range].ToLocationState();
}

LocationState CheckSW(int lineIndex, int charIndex, int range)
{
    if (charIndex - range < 0 || lineIndex + range >= lines)
    {
        return LocationState.StopInstruction;
    }

    return room[lineIndex + range][charIndex - range].ToLocationState();
}

LocationState CheckS(int lineIndex, int charIndex, int range)
{
    if (lineIndex + range >= lines)
    {
        return LocationState.StopInstruction;
    }

    return room[lineIndex + range][charIndex].ToLocationState();
}

LocationState CheckSE(int lineIndex, int charIndex, int range)
{
    if (charIndex + range >= chars || lineIndex + range >= lines)
    {
        return LocationState.StopInstruction;
    }

    return room[lineIndex + range][charIndex + range].ToLocationState();
}