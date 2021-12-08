// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 8: Seven Segment Search ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

long total = 0;

foreach (string line in lines)
{
    var parts = line.Split('|');

    List<HashSet<char>> signalSets = new List<HashSet<char>>(Array.ConvertAll<string, HashSet<char>>(parts[0].Trim(' ').Split(' '), (s) => new HashSet<char>(s.ToCharArray())));

    HashSet<char>[] digits = new HashSet<char>[signalSets.Count];

    digits[1] = signalSets.FindDigit(d => d.Count == 2);
    digits[7] = signalSets.FindDigit(d => d.Count == 3);
    digits[4] = signalSets.FindDigit(d => d.Count == 4);
    digits[8] = signalSets.FindDigit(d => d.Count == 7);
    digits[3] = signalSets.FindDigit(d => d.Count == 5 && d.IsProperSupersetOf(digits[7]));
    digits[9] = signalSets.FindDigit(d => d.Count == 6 && d.IsProperSupersetOf(digits[3]));
    digits[0] = signalSets.FindDigit(d => d.Count == 6 && d.IsProperSupersetOf(digits[1]));
    digits[6] = signalSets.FindDigit(d => d.Count == 6);
    digits[5] = signalSets.FindDigit(d => d.Count == 5 && d.IsProperSubsetOf(digits[6]));
    digits[2] = signalSets.FindDigit(d => d.Count == 5);

    HashSet<char>[] outputValues = Array.ConvertAll<string, HashSet<char>>(parts[1].Trim(' ').Split(' '), (s) => new HashSet<char>(s.ToCharArray()));

    string reading = string.Empty;

    foreach (var outputValue in outputValues)
    {
        reading += Array.FindIndex(digits, d => d.SetEquals(outputValue)).ToString();
    }

    total += int.Parse(reading);
}

Console.WriteLine($"Total: {total}"); // 1043697

public static class Utils
{
    public static HashSet<char> FindDigit(this List<HashSet<char>> signalSets, Func<HashSet<char>, bool> predicate)
    {
        var digit = signalSets.Single(predicate);
        signalSets.Remove(digit);
        return digit;
    }
}
