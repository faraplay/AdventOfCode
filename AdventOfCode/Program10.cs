var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input10.txt");

int time = 0;
int x = 1;
List<int> states = new();
states.Add(x);
foreach (var line in inputs)
{
    string instruction = line[0..4];
    switch (instruction)
    {
        case "noop":
            states.Add(x);
            time++;
            break;
        case "addx":
            int value = int.Parse(line[4..]);
            states.Add(x);
            x += value;
            states.Add(x);
            time += 2;
            break;
        default:
            throw new Exception("unknown instruction");
    }
}

int sum = 0;
foreach(int timepoint in new int[] { 20, 60, 100, 140, 180, 220 })
{
    int strength = states[timepoint - 1] * timepoint;
    sum += strength;
    Console.WriteLine(strength);
}
Console.WriteLine(sum);

for (int i = 0; i < 6; i++)
{
    for (int j=0; j < 40; j++)
    {
        int nowX = states[40 * i + j];
        if (j >= nowX - 1 && j <= nowX + 1)
        {
            Console.Write('#');
        }
        else
        {
            Console.Write('.');
        }
    }
    Console.WriteLine();
}