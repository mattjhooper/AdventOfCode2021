// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 14: Extended Polymerization ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

string polymer = lines[0];

Dictionary<string, InsertionRule> map = new ();

for (int i = 2; i < lines.Length; i++)
{
    string key = lines[i].Substring(0, 2);
    string child1 = $"{key[0]}{lines[i].Substring(lines[i].Length - 1, 1)}";
    string child2 = $"{lines[i].Substring(lines[i].Length - 1, 1)}{key[1]}";

    InsertionRule rule = new InsertionRule { Child1 = child1, Child2 = child2, Count = 0 };

    map.Add(key, rule);
}

for (int i = 0; i < polymer.Length - 1; i++)
{
    map[polymer.Substring(i, 2)].Count++;
}

for (int loop = 0; loop < 40; loop++)
{
    var tempMap = map.ToDictionary(v => v.Key, v => v.Value.Clone());

    map.Reset();

    foreach (var entry in tempMap)
    {
        map[entry.Value.Child1].Count += entry.Value.Count;
        map[entry.Value.Child2].Count += entry.Value.Count;
    }

}

map.Print();

Console.WriteLine($"Total Count: {map.Sum(e => e.Value.Count)}");

var charCount = map.GetCharacterCount(polymer[polymer.Length - 1]);

long highestCount = charCount.Max(c => charCount[c.Key]);
long lowestCount = charCount.Min(c => charCount[c.Key]);

Console.WriteLine($"{highestCount} - {lowestCount} = {highestCount - lowestCount}"); // 1749 - 161 = 1588


public class InsertionRule
{
    public long Count { get; set; }
    public string Child1 { get; set; }
    public string Child2 { get; set; }

    public InsertionRule Clone() => new InsertionRule { Child1 = this.Child1, Child2 = this.Child2, Count = this.Count }; 
}

public static class Utils
{
    public static void Reset(this Dictionary<string, InsertionRule> map)
    {
        foreach (var entry in map)
        {
            entry.Value.Count = 0;
        }
    }

    public static void Print(this Dictionary<string, InsertionRule> map)
    {
        foreach (var item in map)
        {
            Console.WriteLine($"{item.Key} -> {item.Value.Child1} + {item.Value.Child2}. Count: {item.Value.Count}");
        }
    }

    public static Dictionary<char, long> GetCharacterCount(this Dictionary<string, InsertionRule> map, char extraChar)
    {
        Dictionary<char, long> charCount = new();
        foreach (var item in map)
        {
            char c1 = item.Key[0];
            char c2 = item.Key[1];

            charCount[c1] = charCount.GetValueOrDefault(c1, 0) + item.Value.Count;
            charCount[c2] = charCount.GetValueOrDefault(c2, 0) + item.Value.Count;
        }

        // halve each entry
        foreach(var entry in charCount)
        {
            charCount[entry.Key] = entry.Value / 2;
        }

        // Add extra character (the one on the end of the original polymer)
        charCount[extraChar]++;

        return charCount;
    }

}