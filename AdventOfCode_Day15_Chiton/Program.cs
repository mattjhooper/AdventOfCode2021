// See https://aka.ms/new-console-template for more information
using System.Collections;

Console.WriteLine("--- Day 15: Chiton ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

var cavern = new Cavern(lines);

//cavern.Print();

cavern.MapPath();

Console.WriteLine($"Lowest Risk Level: {cavern.GetLowestRiskLevel()}");

Console.WriteLine($"Unreached nodes: {cavern.Sum(m => m.TotalRisk == int.MaxValue ? 1 : 0)} out of {cavern.Count()}");


public class Cavern : Grid<Position>
{
    public Cavern(string[] input) : base(input.Length, input[0].Length)
    {
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                new Position(this, new Point(x, y), int.Parse(input[y][x].ToString()));
            }
        }        
    }

    public IMarker GetStartPosition() => this.Single(m => m.Start);

    public void MapPath()
    {
        var currentVertex = GetStartPosition();

        currentVertex.Check(new OutOfBounds(), 0);
        currentVertex.CheckNeighbours();


        while (this.Any(m => m.Temporary))
        {
            int minPath = this.Where(m => m.Temporary).Select(m => m.TotalRisk).Min();

            currentVertex = this.First(m => m.Temporary && m.TotalRisk == minPath);
            currentVertex.CheckNeighbours();

        }


    }

    public int GetLowestRiskLevel()
    {
        return this.Single(m => m.End).TotalRisk - this.Single(m => m.Start).RiskLevel;
    }

}


public interface IMarker
{
    Point Location { get; }
    string ToString();
    bool Start { get; }
    bool End { get; }

    bool Temporary { get; }

    int TotalRisk { get; }

    int RiskLevel { get; }

    void CheckNeighbours();

    void Check(IMarker p, int currentPathCost);
}

public class Position : IMarker
{
    private readonly Grid<Position> _grid;

    private IMarker _previousPosition;
    private int _cost = int.MaxValue;

    public Position(Grid<Position> g, Point p, int r)
    {
        _grid = g;
        Location = p;
        RiskLevel = r;

        Temporary = true;

        g.SetMarker(this);

        _previousPosition = new OutOfBounds();
    }

    public int TotalRisk => _cost;

    public bool Temporary { get; private set; }

    public void Check(IMarker p, int currentPathCost)
    {
        if (currentPathCost + RiskLevel < _cost)
        {
            _cost = currentPathCost + RiskLevel;
            _previousPosition = p;            
        }
    }
    
    public void CheckNeighbours()
    {
        Temporary = false;
        foreach (var neighbour in GetNeighbours().Where(n => n.Temporary))
        {
            neighbour.Check(this, _cost);
        }        
    }    

    public int RiskLevel { get; private set; }

    public bool Start => Location == new Point(0,0);
    public bool End => Location == new Point(_grid.Height -1, _grid.Width -1);

    public Point Location { get; private set; }

    public override string ToString()
    {
        return RiskLevel.ToString();
    }

    private IEnumerable<IMarker> GetNeighbours()
    {
        Point Left = new(-1, 0), Right = new(1, 0), Up = new(0, -1), Down = new(0, 1);
        foreach (var direction in new Point[] { Left, Up, Right, Down })
        {
            yield return _grid[Location.Move(direction)];
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

    public bool Start => false;
    public bool End => false;

    public bool Temporary => false;

    public int TotalRisk => int.MaxValue;

    public int RiskLevel => int.MaxValue;

    public void CheckNeighbours() { }

    public void Check(IMarker p, int currentPathCost) { }
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