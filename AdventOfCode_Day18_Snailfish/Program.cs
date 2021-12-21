// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 18: Snailfish ---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

var numbers = new List<ISnailfishNumber>();

foreach (string line in lines)
{
   // bool left = true;

    var pairs = new Stack<Pair>();
       
    for(int c = 0; c < line.Length; c++)
    {
        int i;
        if(line[c] == '[')
        {
            Pair newPair = new Pair();

            if (pairs.Count > 0)
            {
                var p = pairs.Peek();

                if (p.Left == null)
                {
                    p.Left = newPair;
                }
                else
                {
                    p.Right = newPair;
                }
            }
            
            pairs.Push(newPair);
        }
        else if (int.TryParse(line[c].ToString(), out i))
        {
            var r = new Regular(i);

            var p = pairs.Peek();

            if (p.Left == null)
            {
                p.Left = r;                
            }
            else
            {
                p.Right = r;              
            }
        }
        else if (line[c] == ']')
        {
            if (pairs.Count > 1)
            {
                pairs.Pop();
            }
        }
         
    }

    numbers.Add(pairs.Pop());
}

foreach (var num in numbers)
{
    Console.WriteLine(num);
}

public interface ISnailfishNumber
{

}

public class Pair : ISnailfishNumber
{
    public Pair()
    {
    }

    public Pair(ISnailfishNumber left, ISnailfishNumber right)
    {
        Left = left;
        Right = right;
    }

    public ISnailfishNumber? Left { get; set; }
    public ISnailfishNumber? Right { get; set; }

    public override string ToString()
    {
        return $"[{Left},{Right}]";
    }
}

public class Regular : ISnailfishNumber
{
    public Regular()
    { }

    public Regular(int value)
    {
        Value = value;
    }

    public int? Value { get; set; }

    public override string ToString()
    {
        return Value.HasValue ? Value.ToString() : string.Empty;
    }
}