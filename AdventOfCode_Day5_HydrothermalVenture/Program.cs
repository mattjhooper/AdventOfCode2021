// See https://aka.ms/new-console-template for more information
using AdventOfCode_Day5_HydrothermalVenture;
using System.Drawing;

Console.WriteLine("--- Day 5: Hydrothermal Venture ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

Dictionary<Point,int> vents = new ();

foreach (string lineDefinition in lines)
{
    var line = new Line(lineDefinition);

    foreach (var point in line.GetPoints())
    {      
        AddToVents(point);     
    }    
}

int dangerPoints = vents.Where(v => v.Value >= 2).Count();

Console.WriteLine($"Danger areas: {dangerPoints}");

// Keep the console window open in debug mode.
Console.WriteLine("Press any key to exit.");
System.Console.ReadKey();

void AddToVents(Point point)
{
    var count = vents.GetValueOrDefault(point, 0);
    vents[point] = count + 1;
}
