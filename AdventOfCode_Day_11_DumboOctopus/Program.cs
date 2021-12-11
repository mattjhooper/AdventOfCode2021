// See https://aka.ms/new-console-template for more information
using System.Collections;

Console.WriteLine("--- Day 11: Dumbo Octopus ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

var cavern = new Cavern(lines);

//cavern.Print();

Console.WriteLine($"Flash count {cavern.GetFlashCount()}"); // 1642

//cavern.Print();


public class Cavern : Grid
{
    public Cavern(string[] input) : base(input.Length, input[0].Length)
    {
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                new Octopus(this, new Point(x, y), int.Parse(input[y][x].ToString()));
            }
        }

    }

    public long GetFlashCount()
    {
        long flashCount = 0;

        for (int step = 1; step <= 100; step++)
        {
            foreach (IMarker m in this)
            {
                m.LevelUp();
            }

            foreach (IMarker m in this)
            {
                flashCount += m.GetFlashCount();
            }
        }

        return flashCount;
    }    
}

public interface IMarker
{
    int Level { get; }
    Point Location { get; }

    void LevelUp();

    void IncreaseLevel();

    int GetFlashCount();
    string ToString();
}

public class Octopus : IMarker
{
    private readonly Grid _grid;
    private bool _flashed = false;

    public Octopus(Grid g, Point p, int level)
    {
        _grid = g;
        Level = level;
        Location = p;
        g.SetMarker(this);
    }

    public int Level { get; private set; }

    public Point Location { get; private set; }

    public void LevelUp()
    {
        _flashed = false;
        IncreaseLevel();
    }

    public void IncreaseLevel()
    {
        if (!_flashed)
        {
            Level++;
        }
    }

    public int GetFlashCount()
    {
        if (!_flashed && Level > 9)
        {
            _flashed = true;
            Level = 0;
            return 1 + GetNeighboursFlashCount();
        }

        return 0;
    }

    private int GetNeighboursFlashCount()
    {
        int count = 0;
        foreach (var neighbour in GetNeighbours())
        {
            neighbour.IncreaseLevel();
            count += neighbour.GetFlashCount();
        }
        return count;
    }

    public override string ToString()
    {
        return Level.ToString();
    }

    private IEnumerable<IMarker> GetNeighbours()
    {
        Point Left = new(-1, 0), Right = new(1, 0), Up = new(0, -1), Down = new(0, 1);
        foreach (var direction in new Point[] { Left, Up, Right, Down, new (-1, -1), new(-1, 1), new(1, 1), new(1, -1) })
        {
            yield return _grid[Location.Move(direction)];
        }
    }    
}

public class OutOfBounds : IMarker
{
    public int Level => 10;

    public Point Location => new Point(-1, -1);

    public int GetFlashCount() => 0;
    
    public void LevelUp()
    {
        throw new NotImplementedException();
    }

    public void IncreaseLevel()
    {        
    }

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

        _grid = new Octopus[height, width];
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

    internal void SetMarker(Octopus m)
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