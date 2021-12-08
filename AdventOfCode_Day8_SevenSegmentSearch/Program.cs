// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 8: Seven Segment Search ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

long total = 0;

foreach (string line in lines)
{
    var parts = line.Split('|');

    HashSet<char>[] signalSets = Array.ConvertAll<string, HashSet<char>>(parts[0].Trim(' ').Split(' '), (s) => new HashSet<char>(s.ToCharArray()));

    HashSet<char>[] digits = new HashSet<char>[signalSets.Length];

    digits[1] = signalSets.First(d => d.Count == 2);
    digits[7] = signalSets.First(d => d.Count == 3);
    digits[4] = signalSets.First(d => d.Count == 4);
    digits[8] = signalSets.First(d => d.Count == 7);
    digits[3] = signalSets.First(d => d.Count == 5 && d.IsProperSupersetOf(digits[7]));
    digits[9] = signalSets.First(d => d.Count == 6 && d.IsProperSupersetOf(digits[3]));
    digits[0] = signalSets.First(d => d.Count == 6 && !d.SetEquals(digits[9]) && d.IsProperSupersetOf(digits[1]));
    digits[6] = signalSets.First(d => d.Count == 6 && !d.SetEquals(digits[0]) && !d.SetEquals(digits[9]));
    digits[5] = signalSets.First(d => d.Count == 5 && d.IsProperSubsetOf(digits[6]));
    digits[2] = signalSets.First(d => d.Count == 5 && !d.SetEquals(digits[3]) && !d.SetEquals(digits[5]));

    HashSet<char>[] outputValues = Array.ConvertAll<string, HashSet<char>>(parts[1].Trim(' ').Split(' '), (s) => new HashSet<char>(s.ToCharArray()));

    string reading = string.Empty;

    foreach (var outputValue in outputValues)
    {
        reading += Array.FindIndex(digits, d => d.SetEquals(outputValue)).ToString();
    }

    total += int.Parse(reading);
}

Console.WriteLine($"Total: {total}"); // 1043697
