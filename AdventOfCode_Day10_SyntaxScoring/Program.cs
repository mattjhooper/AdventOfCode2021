// See https://aka.ms/new-console-template for more information
Console.WriteLine("---Day 10: Syntax Scoring---");

string[] lines = System.IO.File.ReadAllLines(@"Input.txt");

List<string> corruptLines = new ();
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

                break;

            case ')':
                check = chunks.Pop();
                if(check != '(')
                {
                    corrupt = true;
                    corruptLines.Add(line);
                    errorScore += 3;
                }

                break;

            case ']':
                check = chunks.Pop();
                if (check != '[')
                {
                    corrupt = true;
                    corruptLines.Add(line);
                    errorScore += 57;
                }

                break;

            case '}':
                check = chunks.Pop();
                if (check != '{')
                {
                    corrupt = true;
                    corruptLines.Add(line);
                    errorScore += 1197;
                }

                break;

            case '>':
                check = chunks.Pop();
                if (check != '<')
                {
                    corrupt = true;
                    corruptLines.Add(line);
                    errorScore += 25137;
                }

                break;

            default:
                break;
        }                
    }
}

/*
foreach (var corruptLine in corruptLines)
{
    Console.WriteLine(corruptLine);
}
*/

Console.WriteLine($"Error score: {errorScore}");
