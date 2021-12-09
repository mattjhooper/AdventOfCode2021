// See https://aka.ms/new-console-template for more information
using System.Collections;

Console.WriteLine("--- Day 9: Smoke Basin ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

var terrain = new Terrain(lines);

//terrain.Print();

Console.WriteLine($"Sum of Risk: {terrain.GetSumOfRiskLevels()}"); // 522

terrain.PrintBasinCounts();

public class Terrain : Grid
{
    public Terrain(string[] input) : base(input.Length, input[0].Length)
    {
        for(int y = 0; y < input.Length; y++)
        {
            for(int x = 0; x < input[y].Length; x++)
            {
                SetMarker(new Marker(this, new Point(x, y), int.Parse(input[y][x].ToString())));
            }
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
    private readonly Grid _grid;
    private bool _Counted = false;

    public Marker(Grid g, Point p, int level)
    {
        _grid = g;
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
        foreach (var neighbour in GetNeighbours())
        {
            count += neighbour.GetBasinCount();
        }
        return count;
    }

    public override string ToString()
    {
        return Level.ToString();
    }

    private bool HasLowerNeighbour()
    {
        foreach(var neighbour in GetNeighbours())
        {
            if (neighbour.IsLowerThan(Level))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerable<IMarker> GetNeighbours()
    {
        Point Left = new(-1, 0), Right = new(1, 0), Up = new(0, -1), Down = new (0, 1);
        foreach (var direction in new Point[] { Left, Up, Right, Down })
        {
            yield return _grid[Location.Move(direction)];
        }
    }
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

public abstract class Grid : IEnumerable<IMarker>
{
    private readonly IMarker[,] _grid;

    protected Grid(int height, int width)
    {
        Height = height;
        Width = width;

        _grid = new Marker[height, width];
    }

    public int Height { get; init; }
    public int Width { get; init; }

    public IMarker this[Point p]
    {
        get
        {
            return InBounds(p) ? _grid[p.Y, p.X] : new OutOfBounds();
        }
    }

    protected void SetMarker(Marker m)
    {
        if (InBounds(m.Location))
        {
            _grid[m.Location.Y, m.Location.X] = m;
        }
    }

    public IEnumerator<IMarker> GetEnumerator()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                yield return _grid[y, x];
            }
        }
    }

    public bool InBounds(Point p) => 0 <= p.Y && p.Y < Height && 0 <= p.X && p.X < Width;

    public void Print()
    {
        Console.WriteLine("");
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(_grid[y, x]);
            }
            Console.WriteLine("");
        }
        Console.WriteLine("");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed record Point(int X, int Y)
{
    public Point Move(Point delta) => new Point(X + delta.X, Y + delta.Y);

    public override string ToString() => $"[{X},{Y}]";
}