// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 7: The Treachery of Whales ---");

string input = System.IO.File.ReadAllText(@"Input.txt");

var positionStrings = input.Split(',');

int[] positions = Array.ConvertAll<string, int>(positionStrings, (s) => int.Parse(s));

int average = (int)Math.Round(positions.Average(),0);
int median = positions.OrderBy(i => i).ToArray()[positions.Length / 2];
int range = Math.Max(Math.Abs(average - median), 20);
int minVal = positions.Min();
int maxVal = positions.Max();

Console.WriteLine($"Average: {average}. Median: {median}");

Dictionary<int, int> positionCounts = new();

for(int i = 0; i < positions.Length; i++)
{
    positionCounts[positions[i]] = positionCounts.GetValueOrDefault(positions[i],0) + 1;
}

long bestFuel = long.MaxValue;
//bool keepChecking = true;
int bestPosition = 0;

for (int position = minVal; position <= maxVal; position++)
{
    long currentFuel = 0;
    foreach (var p in positionCounts)
    {
        currentFuel += Factoral(Math.Abs(position - p.Key)) * p.Value;
    }

    Console.WriteLine($"Pos: {position}, Fuel: {currentFuel}");
    if (currentFuel < bestFuel)
    {
        bestFuel = currentFuel;
        bestPosition = position;
    }
}

Console.WriteLine($"Best Position: {bestPosition}, Fuel: {bestFuel}");

static int Factoral(int i)
{
    if (i == 0)
    {
        return 0;
    }

    return i + Factoral(i-1);
}
