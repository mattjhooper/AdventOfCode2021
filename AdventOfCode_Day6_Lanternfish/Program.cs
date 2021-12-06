// See https://aka.ms/new-console-template for more information
using AdventOfCode_Day6_Lanternfish;

Console.WriteLine("--- Day 6: Lanternfish ---");

string input = System.IO.File.ReadAllText(@"Input.txt");

var timers = input.Split(',');

List<LanternFish> shoal = new();

foreach (var timer in timers)
{
    //Console.WriteLine(timer);
    shoal.Add(new LanternFish(timer));

}

for (int day = 1; day <= 80; day++)
{
    List<LanternFish> newFish = new();

    foreach(var fish in shoal)
    {
        fish.CheckTimer(newFish);
    }

    shoal.AddRange(newFish);
}

Console.WriteLine($"There are now {shoal.Count} lanternfish");