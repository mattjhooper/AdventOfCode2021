// See https://aka.ms/new-console-template for more information
using System.Collections;

Console.WriteLine("--- Day 11: Dumbo Octopus ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

var cavern = new Cavern(lines);

//cavern.Print();

Console.WriteLine($"Flash count {cavern.GetFlashCount()}"); // 1642

//cavern.Print();


public class Cavern : Grid<Octopus>
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
        bool allFlash = false;

        for (int step = 1; !allFlash; step++)
        {
            foreach (IMarker m in this)
            {
                m.LevelUp();
            }

            foreach (IMarker m in this)
            {
                flashCount += m.GetFlashCount();
            }

            int levelSum = this.Sum(m => m.Level);
            if (levelSum == 0)
            {
                allFlash = true;
                Console.WriteLine($"All flash at step {step}"); //320
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
    private readonly Grid<Octopus> _grid;
    private bool _flashed = false;

    public Octopus(Grid<Octopus> g, Point p, int level)
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
       for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                yield return _grid[Location.Move(new Point(x, y))];
            }
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

public abstract class Grid<T> : IEnumerable<IMarker> where T : IMarker
{
    private readonly T[,] _grid;

    protected Grid(int height, int width)
    {
        Height = height;
        Width = width;

        _grid = new T[height, width];
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

    internal void SetMarker(T m)
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