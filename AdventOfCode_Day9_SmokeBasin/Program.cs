// See https://aka.ms/new-console-template for more information
using System.Collections;

Console.WriteLine("--- Day 9: Smoke Basin ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

var terrain = new Terrain(lines);

//terrain.Print();

Console.WriteLine($"Sum of Risk: {terrain.GetSumOfRiskLevels()}");

terrain.PrintBasinCounts();


public class Terrain : IEnumerable<IMarker>
{
    private readonly Marker[,] _t;
    private readonly int _height;
    private readonly int _width;

    public Terrain(string[] input)
    {
        _height = input.Length;
        _width = input[0].Length;
        _t = new Marker[_height, _width];

        for(int y = 0; y < input.Length; y++)
        {
            for(int x = 0; x < input[y].Length; x++)
            {
                _t[y, x] = new Marker(this, new Point(x, y), int.Parse(input[y][x].ToString()));
            }
        }

    }

    public IMarker this[Point p]
    {
        get
        {
            return InBounds(p) ? _t[p.Y, p.X] : new OutOfBounds();
        }
    }

    public int GetSumOfRiskLevels()
    {
        int sum = 0;
        foreach (IMarker m in this)
        {
            sum += m.GetRiskLevel();
        }

        return sum;
    }

    public void PrintBasinCounts()
    {
        var basinCounts = new List<int>();

        foreach (IMarker m in this)
        {
            var basinCount = m.GetBasinCount();

            if (basinCount > 0)
            {
                //Console.WriteLine($"{m.Location}: {basinCount}");
                basinCounts.Add(basinCount);
            }
        }

        Console.WriteLine($"Basin calc: {basinCounts.OrderByDescending(i => i).Take(3).Aggregate(1, (a, b) => a * b)}"); // 916688
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

    public IEnumerator<IMarker> GetEnumerator()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                yield return _t[y,x];
            }
        }
    }

    public bool InBounds(Point p) => 0 <= p.Y && p.Y < _height && 0 <= p.X && p.X < _width;

}

public interface IMarker
{
    int Level { get; }
    Point Location { get; }

    int GetBasinCount();
    int GetRiskLevel();
    bool IsLowerThan(int checkLevel);
    string ToString();
}

public class Marker : IMarker
{
    private readonly Terrain _t;
    private bool _Counted = false;

    public Marker(Terrain t, Point p, int level)
    {
        _t = t;
        Level = level;
        Location = p;
    }

    public int Level { get; private set; }

    public Point Location { get; private set; }

    public bool IsLowerThan(int checkLevel) => Level <= checkLevel;

    public int GetRiskLevel()
    {
        if (HasLowerNeighbour())
            return 0;

        return Level + 1;
    }

    public int GetBasinCount()
    {
        if (!_Counted && Level < 9)
        {
            _Counted = true;
            return 1 + GetNeighboursBasinCount();
        }

        return 0;
    }

    private int GetNeighboursBasinCount()
    {
        int count = 0;
        Point[] directions = { Point.Left, Point.Up, Point.Right, Point.Down };
        foreach (var direction in directions)
        {
            count += GetMarkerInDirection(direction).GetBasinCount();
        }

        return count;
    }

    public override string ToString()
    {
        return Level.ToString();
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


    private IMarker GetMarkerInDirection(Point direction) => _t[Location.Move(direction)];
}

public class OutOfBounds : IMarker
{
    public int Level => 10;

    public Point Location => new Point(-1, -1);

    public int GetBasinCount() => 0;
    public int GetRiskLevel() => 0;
    public bool IsLowerThan(int checkLevel) => false;

    public override string ToString()
    {
        return "Out of Bounds";
    }
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