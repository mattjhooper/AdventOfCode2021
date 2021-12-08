// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 8: Seven Segment Search ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

long total = 0;

foreach (string line in lines)
{
    var parts = line.Split('|');

    var signalValues = parts[0].Trim(' ').Split(' ');

    string[] digits = new string[signalValues.Length];

    digits[1] = signalValues.First(d => d.Length == 2);
    digits[7] = signalValues.First(d => d.Length == 3);
    digits[4] = signalValues.First(d => d.Length == 4);
    digits[8] = signalValues.First(d => d.Length == 7);
    digits[3] = signalValues.First(d => d.Length == 5 && isFoundIn(digits[7], d));
    digits[9] = signalValues.First(d => d.Length == 6 && isFoundIn(digits[3], d));
    digits[0] = signalValues.First(d => d.Length == 6 && d != digits[9] && isFoundIn(digits[1], d));
    digits[6] = signalValues.First(d => d.Length == 6 && d != digits[0] && d != digits[9]);
    digits[5] = signalValues.First(d => d.Length == 5 && isFoundIn(d, digits[6]));
    digits[2] = signalValues.First(d => d.Length == 5 && d != digits[5] && d != digits[3]);

    for(int i = 0; i < digits.Length; i++)
    {
        //Console.WriteLine($"{i}: {digits[i]}");
        digits[i] = String.Concat(digits[i].OrderBy(c => c));
    }

    var outputValues = parts[1].Trim(' ').Split(' ');

    string reading = string.Empty;

    //Console.WriteLine($"--- New Line ----");
    foreach (var outputValue in outputValues)
    {
        reading += Array.FindIndex(digits, d => d == String.Concat(outputValue.OrderBy(c => c))).ToString();
    }

    //Console.WriteLine($"Interesting Digits: {digitCount}");
    //Console.WriteLine($"Reading: {reading}");
    total += int.Parse(reading);
}

Console.WriteLine($"Total: {total}");

bool isFoundIn(string a, string b)
{
    foreach (char c in a.ToCharArray())
    {
        if (!b.Contains(c))
        {
            return false;
        }
    }

    return true;
}