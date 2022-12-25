using System.Text.RegularExpressions;

var reader = File.OpenText(@"..\..\..\..\Inputs\input5.txt");

string line = reader.ReadLine()!;
var stackCount = (line.Length + 1)/ 4;
var stacks = new List<List<char>>();
for (int i = 0; i < stackCount; i++) stacks.Add(new List<char>());
AddCrates(line);

while (true)
{
    line = reader.ReadLine()!;
    if (line[1] == '1') break;
    AddCrates(line);
}
reader.ReadLine();

Regex regex = new Regex(@"^move ([0-9]+) from ([0-9]+) to ([0-9]+)$");
while (!reader.EndOfStream)
{
    line = reader.ReadLine()!;
    var match = regex.Match(line);
    if (match is null) throw new Exception();
    int mvSize = int.Parse(match.Groups[1].Value);
    Move(mvSize, int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));

    Console.WriteLine("-----------------");
    foreach (var stack in stacks)
    {
        foreach (var crate in stack)
        {
            Console.Write(crate);
        }
        Console.WriteLine();
    }
}
Console.WriteLine("-----------------");
foreach (var stack in stacks)
{
    Console.Write(stack[^1]);
}
Console.WriteLine();


void Move(int count, int source, int dest)
{
    var srcStack = stacks[source - 1];
    var destStack = stacks[dest - 1];
    List<char> crates = srcStack.GetRange(srcStack.Count - count, count);
    //crates.Reverse();
    srcStack.RemoveRange(srcStack.Count - count, count);
    destStack.AddRange(crates);
}

void AddCrates(string line)
{
    for (int i = 0; 4 * i < line.Length; i++)
    {
        if (line[4 * i] == '[' && line[4 * i + 2] == ']')
        {
            stacks[i].Insert(0, line[4 * i + 1]);
        }
    }
}