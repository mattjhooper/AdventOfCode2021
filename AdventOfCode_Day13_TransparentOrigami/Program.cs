// See https://aka.ms/new-console-template for more information
using System.Collections;

Console.WriteLine("--- Day 13: Transparent Origami ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

int endOfCoords = Array.FindIndex(lines, l => l.Length == 0);

Point[] coords = Array.ConvertAll<string, Point>(lines.Take(endOfCoords).ToArray(), (s) =>
{
    var parts = s.Split(",");
    return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
});

string[] instructions = lines.Skip(endOfCoords + 1).Take(lines.Length - endOfCoords).ToArray();

Console.WriteLine($"Width: {1 + coords.Select(p => p.X).Max()}. Height: {1 + coords.Select(p => p.Y).Max()}");

var paper = new Paper(coords);

//paper.Print();

foreach (string instruction in instructions)
{
    paper = Instruction.Parse(instruction).Fold(paper);
    //paper.Print();
    Console.WriteLine($"Visible Dots: {paper.GetDotCount()}");
}

paper.Print();


public class Paper : Grid<Coord>
{
    public Paper(IEnumerable<Point> input) : base(1 + input.Select(p => p.Y).Max(), 1 + input.Select(p => p.X).Max())
    {
        foreach(var coord in input)
        {
            new Coord(this, coord);
        }

    }

    public Paper(int height, int width) : base(height, width)
    { }

    public int GetDotCount()
    {
        return this.Count();
    }

    public override IMarker this[Point p]
    {
        get
        {
            return base[p] ?? new Coord(this, p, ' ');
        }
    }
}

public record Instruction(char Direction, int Position)
{
    public bool IsVertical => Direction == 'x';

    public Paper Fold(Paper paper)
    {
        var newPaper = GetBlankPaper(paper);

        foreach (var m in paper)
        {
            Fold(m, newPaper);
        }

        return newPaper;
    }

    private Paper GetBlankPaper(Paper paper)
    {
        int newHeight = IsVertical ? paper.Height : Position;
        int newWidth = IsVertical ? Position : paper.Width;

        return new Paper(newHeight, newWidth);
    }

    private void Fold(IMarker m, Paper paper)
    {
        int x = IsVertical && Position < m.Location.X ? 2 * Position - m.Location.X : m.Location.X;
        int y = !IsVertical && Position < m.Location.Y ? 2 * Position - m.Location.Y : m.Location.Y;

        new Coord(paper, new Point(x, y));
    }

    public static Instruction Parse(string instruction)
    {
        return new Instruction(instruction[11], int.Parse(instruction.Split("=")[1]));
    }
}

public interface IMarker
{
    Point Location { get; }
    string ToString();    
}

public class Coord : IMarker
{
    private readonly Grid<Coord> _grid;
    
    public Coord(Grid<Coord> g, Point p, char m = '#')
    {
        _grid = g;
        Location = p;
        Marker = m;

        g.SetMarker(this);
    }

    public char Marker { get; private set; }

    public Point Location { get; private set; }

    public override string ToString()
    {
        return Marker.ToString();
    }

    private IEnumerable<IMarker> GetNeighbours()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                yield return _grid[Location.Move(x, y)];
            }
        }
    }
}

public class OutOfBounds : IMarker
{
    public Point Location => new Point(-1, -1);

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

    public virtual IMarker this[Point p]
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
                var m = _grid[y, x];

                if (m != null)
                {
                    yield return _grid[y, x];
                }
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
                Console.Write(this[new Point(x, y)]);
            }
            Console.WriteLine("");
        }
        Console.WriteLine("");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed record Point(int X, int Y)
{
    public Point Move(Point delta) => Move(delta.X, delta.Y);

    public Point Move(int x, int y) => new Point(X + x, Y + y);

    public override string ToString() => $"[{X},{Y}]";
}


