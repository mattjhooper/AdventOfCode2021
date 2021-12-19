// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 17: Trick Shot ---");

Point start = new(0, 0);

//target area: x = 20..30, y = -10..-5
// 85..145, y=-163..-108

(Point min, Point max) targetArea = (new(85, -163), new(145, -108));

Dictionary<Point,int> velocityHeight = new Dictionary<Point,int>();

for (int x = 0; x < 20; x++)
{
    for (int y = 100; y < 300; y++)
    {
        Point currPosition = start;

        Point velocity = new Point(x, y);

        bool inArea = false;

        int HighestPoint = 0;

        bool beyondArea = false;

        while (!inArea && !beyondArea)// || currPosition.Y > targetArea.min.Y)
        {
            currPosition = currPosition.Move(velocity);
            velocity = velocity.Move(CalculateXVelocity(velocity.X), -1);

            HighestPoint = Math.Max(HighestPoint, currPosition.Y);

            inArea = targetArea.InArea(currPosition);

           // Console.WriteLine($"Current Postion: {currPosition}. Velocity: {velocity}. In Area: {inArea}");
            //string? x = Console.ReadLine();

            beyondArea = currPosition.X > targetArea.max.X || currPosition.Y < targetArea.min.Y;
        }        

        if (inArea)
        {
            velocityHeight[new Point(x, y)] = HighestPoint;
        }
    }
}

foreach ((Point key, int value) in velocityHeight)
{
    Console.WriteLine($"Velocity: {key} Max height reached: {value}");
}



int CalculateXVelocity(int x) => x == 0 ? 0 : x > 0 ? -1 : 1;




public static class ExtensionUtils
{
    public static bool InArea(this (Point min, Point max) area, Point p)
    {
        return area.min.X <= p.X && p.X <= area.max.X && area.min.Y <= p.Y && p.Y <= area.max.Y;
    }
}


public sealed record Point(int X, int Y)
{
    public Point Move(Point delta) => Move(delta.X, delta.Y);

    public Point Move(int x, int y) => new Point(X + x, Y + y);

    public override string ToString() => $"[{X},{Y}]";
}