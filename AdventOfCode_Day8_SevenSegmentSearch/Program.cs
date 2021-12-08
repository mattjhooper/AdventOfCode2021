// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 8: Seven Segment Search ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

int digitCount = 0;

foreach (string line in lines)
{
    var parts = line.Split('|');

    var outputValues = parts[1].Trim(' ').Split(' ');

    Console.WriteLine($"--- New Line ----");
    foreach (var outputValue in outputValues)
    {
        if (outputValue.Length != 5 && outputValue.Length != 6)
        {
            digitCount++;
            Console.WriteLine($"Value: {outputValue}. Length: {outputValue.Length}");
        }
    }
}

Console.WriteLine($"Interesting Digits: {digitCount}");