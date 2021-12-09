// See https://aka.ms/new-console-template for more information
using System.Collections;

Console.WriteLine("--- Day 9: Smoke Basin ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

var terrain = new Terrain(lines);

//terrain.Print();

Console.WriteLine($"Sum of Risk: {terrain.GetSumOfRiskLevels()}");


public class Terrain : IEnumerable<Marker>
{
    private readonly int[,] _t;
    private readonly int _height;
    private readonly int _width;

    public Terrain(string[] input)
    {
        _height = input.Length;
        _width = input[0].Length;
        _t = new int[_height, _width];

        for(int row = 0; row < input.Length; row++)
        {
            for(int col = 0; col < input[row].Length; col++)
            {
                _t[row, col] = int.Parse(input[row][col].ToString());
            }
        }

    }

    public int this[Point p]
    {
        get
        {
            return InBounds(p) ? _t[p.Y,p.X] : 10;
        }

        set
        {
            if (InBounds(p))
            {
                _t[p.Y,p.X] = value;
            }
        }
    }

    public int GetSumOfRiskLevels()
    {
        int sum = 0;
        foreach (Marker m in this)
        {
            sum += m.GetRiskLevel();
        }

        return sum;
    }

    public void Print()
    {
        Console.WriteLine("");
        for (int row = 0; row < _height; row++)
        {
            for(int col = 0; col < _width; col++)
            {
                Console.Write(_t[row, col]);
            }
            Console.WriteLine("");
        }
        Console.WriteLine("");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Marker> GetEnumerator()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                yield return new Marker(this, new Point(x, y));
            }
        }
    }

    public bool InBounds(Point p) => 0 <= p.Y && p.Y < _height && 0 <= p.X && p.X < _width;

}

public class Marker
{
    private readonly Terrain _t;

    public Marker(Terrain t, Point p)
    {
        _t = t;
        Location = p;
    }

    public Point Location { get; private set; }

    public int Level => _t[Location];

    public bool IsLowerThan(int checkLevel) => Level <= checkLevel;

    public int GetRiskLevel()
    {
        if (HasLowerNeighbour())
            return 0;

        return Level + 1;
    }

    private bool HasLowerNeighbour()
    {
        Point[] directions = { Point.Left, Point.Up, Point.Right, Point.Down };
        bool lowerLevelFound = false;
        for (int i = 0; i < directions.Length && !lowerLevelFound; i++)
        {
            lowerLevelFound = GetMarkerInDirection(directions[i]).IsLowerThan(Level); ;
        }
        return lowerLevelFound;
    }

    
    private Marker GetMarkerInDirection(Point direction) => new Marker(_t, Location.Move(direction));    
}


public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; private set; }
    public int Y { get; private set; }

    public Point Move(Point delta) => new Point(this.X + delta.X, this.Y + delta.Y);

    public override string ToString() => $"[{X},{Y}]";

    public static readonly Point Left = new Point(-1, 0);
    public static readonly Point Right = new Point(1, 0);
    public static readonly Point Up = new Point(0, 1);
    public static readonly Point Down = new Point(0, -1);
}