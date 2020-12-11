
using System;
using System.Linq;

public static class ExtensionMethods
{
    public static string ToPrintableString(this char[][] input)
    {
        return input.Aggregate("", (currLine, nextLine) => currLine += Environment.NewLine + new string(nextLine));
    }

    public static LocationState ToLocationState(this char c)
    {
        switch (c)
        {
            case 'L': return LocationState.EmptySeat;
            case '.': return LocationState.Floor;
            case '#': return LocationState.OccupiedSeat;
        }

        throw new Exception("impossible");
    }
}