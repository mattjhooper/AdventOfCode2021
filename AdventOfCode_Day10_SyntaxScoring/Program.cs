// See https://aka.ms/new-console-template for more information
Console.WriteLine("---Day 10: Syntax Scoring---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

List<long> completionScores = new ();
int errorScore = 0;




foreach(var line in lines)
{
    var chunks = new Stack<char>();

    var chars = line.ToCharArray();

    bool corrupt = false;

    for(int i = 0; i < chars.Length && !corrupt; i++)
    {

        char check = ' ';
        switch (chars[i])
        {
            case '(':
            case '[':
            case '{':
            case '<':
                chunks.Push(chars[i]);
                //Console.WriteLine($"Push: {chars[i]}. Chunks: {chunks.Count}");

                break;

            case ')':
                check = chunks.Pop();
                //Console.WriteLine($"Pop: {check}. Chunks: {chunks.Count}");
                if (check != '(')
                {
                    corrupt = true;
                    errorScore += 3;
                }

                break;

            case ']':
                check = chunks.Pop();
                //Console.WriteLine($"Pop: {check}. Chunks: {chunks.Count}");
                if (check != '[')
                {
                    corrupt = true;
                    errorScore += 57;
                }

                break;

            case '}':
                check = chunks.Pop();
                //Console.WriteLine($"Pop: {check}. Chunks: {chunks.Count}");
                if (check != '{')
                {
                    corrupt = true;
                    errorScore += 1197;
                }

                break;

            case '>':
                check = chunks.Pop();
                //Console.WriteLine($"Pop: {check}. Chunks: {chunks.Count}");
                if (check != '<')
                {
                    corrupt = true;
                    errorScore += 25137;
                }

                break;

            default:
                break;
        }                
    }

    if (!corrupt)
    {
        long completionScore = 0;
        //Console.WriteLine($"{line} -  Chunks: {chunks.Count}");
        var count = chunks.Count;

        for (int i = 0; i < count; i++)
        //foreach(var c in chunks.Reverse())
        {
            char c = chunks.Pop();

            //Console.WriteLine($"Pop: {c}. Chunks: {chunks.Count}");

           // Console.Write(c);

            switch (c)
            {
                case '(':
                    completionScore = (completionScore * 5) + 1;

                    break;
                case '[':
                    completionScore = (completionScore * 5) + 2;

                    break;
                case '{':
                    completionScore = (completionScore * 5) + 3;

                    break;
                case '<':
                    completionScore = (completionScore * 5) + 4;

                    break;
                

                default:
                    break;
            }
        }
        Console.WriteLine($"{completionScore}");
        completionScores.Add(completionScore);
    }
}

/*
foreach (var corruptLine in corruptLines)
{
    Console.WriteLine(corruptLine);
}
*/

Console.WriteLine($"Error score: {errorScore}");

Console.WriteLine($"Completion score: {completionScores.OrderBy(i => i).Skip((completionScores.Count / 2)).Take(1).First()}");
