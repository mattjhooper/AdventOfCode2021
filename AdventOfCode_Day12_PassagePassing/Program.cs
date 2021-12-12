// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("--- Day 12: Passage Pathing ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

HashSet<Cave> caves = new HashSet<Cave>();

foreach(var line in lines)
{
    var parts = line.Split('-');

    Cave cave1 = getOrCreateCave(parts[0]);
    Cave cave2 = getOrCreateCave(parts[1]);

    cave1.AddLinkedCave(cave2);
    cave2.AddLinkedCave(cave1);
}

/*
foreach(var cave in caves)
{
    Console.WriteLine($"Cave: {cave.Name}. Links: {cave.GetLinkedCaves().Count()}");
}
*/

Cave start = caves.Single(c => c.IsStart);

Path p = new Path();
start.ProcessPaths(p);

Console.WriteLine($"Path count: {p.PathCount}");



Cave getOrCreateCave(string name)
{
    var cave = caves.SingleOrDefault(c => c.Name == name, new Cave(name));

    caves.Add(cave);

    return cave;
}

public class Path
{
    private readonly List<Cave> _path;

    private static int _pathCount = 0;

    private Cave _doubleVisitCave;

    public Path()
    {
        _path = new List<Cave>();
    }

    private Path(IEnumerable<Cave> path, Cave doubleVisit) : this()
    {
        _path.AddRange(path);
        _doubleVisitCave = doubleVisit;
    }

    public bool AddToPath(Cave cave)
    {
        if (cave.IsEnd)
        {
            _path.Add(cave);
           // Console.WriteLine(this.ToString());
            _pathCount++;
            return false;
        }

        if (cave.IsStart && _path.Contains(cave))
        {
            return false;
        }

        if(!cave.IsSmall)
        {
            _path.Add(cave);
            return true;
        }

        if (_path.Contains(cave))
        {
            if (_doubleVisitCave is null)
            {
                _doubleVisitCave = cave;
            }
            else
            {
                return false;
            }
        }

        // Add small cave
        _path.Add(cave);
        return true;
    }

    public Path Clone()
    {
        return new Path(_path, _doubleVisitCave);
    }

    public int PathCount => _pathCount;

    public override string ToString()
    {
        StringBuilder pathAsString = new("");

        foreach(var cave in _path)
        {
            if (pathAsString.Length > 0)
            {
                pathAsString.Append(",");
            }
            pathAsString.Append(cave);
        }
        
        return pathAsString.ToString();
    }
}

public class Cave
{
    private readonly HashSet<Cave> linkedCaves;


    public Cave(string name)
    {
        Name = name;
        linkedCaves = new HashSet<Cave>();
    }
    
    public string Name { get; private set; }
    public bool IsSmall => Name == Name.ToLower();

    public bool IsStart => Name == "start";

    public bool IsEnd => Name == "end";

    public bool AddLinkedCave(Cave cave)
    {
        return linkedCaves.Add(cave);
    }

    public void ProcessPaths(Path path)
    {
        if (path.AddToPath(this))
        {
            foreach (var cave in linkedCaves)
            {
                cave.ProcessPaths(path.Clone());
            }
        }
    }

    public IEnumerable<Cave> GetLinkedCaves()
    {
        foreach(var cave in linkedCaves)
        {
            yield return cave;
        }
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        Cave other = obj as Cave;
        if (other == null)
        {
            return false;
        }
        return Name.Equals(other.Name);
    }

    public override string ToString()
    {
        return Name;
    }
}