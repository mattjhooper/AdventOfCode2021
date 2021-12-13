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

Console.WriteLine($"Path count: {p.PathCount}"); // 91292



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

    private bool _doubleVisit = false;

    public Path()
    {
        _path = new List<Cave>();
    }

    private bool CheckForEnd(Cave cave)
    {
        if (cave.IsEnd)
        {
            _path.Add(cave);
            _pathCount++;
            return false;
        }

        return true;
    }

    private bool CheckInPath(Cave cave)
    {
        if (_path.Contains(cave))
        {
            if (cave.IsStart)
            {
                return false;
            }

            if (cave.IsSmall)
            {
                if (!_doubleVisit)
                {
                    _doubleVisit = true;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }



    public bool AddToPath(Cave cave)
    {
        bool keepGoing = CheckForEnd(cave);
        keepGoing = keepGoing && CheckInPath(cave);  

        _path.Add(cave);
        return keepGoing;
    }

    public Path Clone()
    {
        Path clonedPath = new();
        clonedPath._path.AddRange(_path);
        clonedPath._doubleVisit = _doubleVisit;
        return clonedPath;
    }

    public int PathCount => _pathCount;

    public override string ToString()
    {
        StringBuilder pathAsString = new("");

        foreach(var cave in _path)
        {
            pathAsString.Append(pathAsString.Length == 0 ? "" : ",");
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