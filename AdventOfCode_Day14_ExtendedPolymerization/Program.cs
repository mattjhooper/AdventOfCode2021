// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 14: Extended Polymerization ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

string polymer = lines[0];

Dictionary<string, string> map = new Dictionary<string, string>();

for (int i = 2; i < lines.Length; i++)
{
    string key = lines[i].Substring(0, 2);
    string value = $"{key[0]}{lines[i].Substring(lines[i].Length - 1, 1)}{key[1]}";

    map.Add(key, value);
}

/*
foreach(var item in map)
{
    Console.WriteLine($"{item.Key} -> {item.Value}");
}*/

for (int loop = 0; loop < 10; loop++)
{

    List<string> parts = new List<string>();

    for (int i = 0; i < polymer.Length - 1; i++)
    {
        parts.Add(polymer.Substring(i, 2));
    }

    for (int i = 0; i < parts.Count; i++)
    {
        parts[i] = map[parts[i]];
    }

    polymer = polymer[0].ToString() + parts.Aggregate("", (a, b) => a + b.Substring(1, 2));

   // Console.WriteLine(polymer);
}

Dictionary<char, int> charCount = new();
foreach (char c in polymer)
{
    charCount[c] = charCount.GetValueOrDefault(c, 0) + 1;
}

int highestCount = charCount.Max(c => charCount[c.Key]);
int lowestCount = charCount.Min(c => charCount[c.Key]);

Console.WriteLine($"{highestCount} - {lowestCount} = {highestCount - lowestCount}");