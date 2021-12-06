// See https://aka.ms/new-console-template for more information
using AdventOfCode_Day6_Lanternfish;

Console.WriteLine("--- Day 6: Lanternfish ---");

string input = System.IO.File.ReadAllText(@"Input.txt");

var timers = input.Split(',');

Dictionary<int, long> timerCounts = new();

for(int timer = 0; timer <= 8; timer++)
{
    timerCounts[timer] = timers.Where(x => x == timer.ToString()).Count();
}

for (int day = 1; day <= 256; day++)
{
    long newFish = timerCounts[0];

    for (int i = 0; i < 8; i++)
    {
        timerCounts[i] = timerCounts[i + 1];
    }

    timerCounts[8] = newFish;
    timerCounts[6] += newFish;

    long total = 0;
    foreach (var timerCount in timerCounts)
    {
        total = total + timerCount.Value;
    }

    Console.WriteLine($"{DateTime.Now.ToLocalTime()} Day: {day}. There are now {total} lanternfish");
}
