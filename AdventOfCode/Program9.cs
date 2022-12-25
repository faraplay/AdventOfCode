var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input9.txt");

int ropeLength = 10;
int[] x = new int[ropeLength];
int[] y = new int[ropeLength];
Array.Fill(x, 0);
Array.Fill(y, 0);

HashSet<(int, int)> locations = new();
locations.Add((0, 0));

foreach (var line in inputs)
{
    for (int i = 0; i < int.Parse(line[2..]); i++)
    {
        Move(line[0]);
        for (int j = 0; j < ropeLength - 1; j++)
        {
            FixRope(j);
        }
        locations.Add((x[ropeLength - 1], y[ropeLength - 1]));
    }
}

foreach (var loc in locations)
{
    Console.WriteLine(loc);
}
Console.WriteLine(locations.Count);

void Move(char direction)
{
    switch (direction)
    {
        case 'R':
            x[0]++;
            break;
        case 'L':
            x[0]--;
            break;
        case 'U':
            y[0]++;
            break;
        case 'D':
            y[0]--;
            break;
        default:
            throw new Exception();
    }
}

void FixRope(int index)
{ 
    if (x[index + 1] == x[index] - 2)
    {
        x[index + 1] = x[index] - 1;
        if (y[index + 1] > y[index]) y[index + 1]--;
        else if (y[index + 1] < y[index]) y[index + 1]++;
    }
    if (x[index + 1] == x[index] + 2)
    {
        x[index + 1] = x[index] + 1;
        if (y[index + 1] > y[index]) y[index + 1]--;
        else if (y[index + 1] < y[index]) y[index + 1]++;
    }
    if (y[index + 1] == y[index] - 2)
    {
        y[index + 1] = y[index] - 1;
        x[index + 1] = x[index];
    }
    if (y[index + 1] == y[index] + 2)
    {
        y[index + 1] = y[index] + 1;
        x[index + 1] = x[index];
    }
}