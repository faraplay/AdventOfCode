var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input14.txt");

HashSet<(int, int)> blockedSquares = new();
int maxY = 0;

// add rocks
foreach (var input in inputs)
{
    var points = input.Split(" -> ")
        .Select(point =>
        {
            var coords = point.Split(',');
            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);
            return (x, y);
        }).ToArray();
    for (int i = 0; i < points.Length - 1; i++)
    {
        if (points[i].x == points[i + 1].x)
        {
            int x = points[i].x;
            if (points[i].y < points[i + 1].y)
            {
                for (int y = points[i].y; y <= points[i + 1].y; y++)
                    blockedSquares.Add((x, y));
            }
            else
            {
                for (int y = points[i].y; y >= points[i + 1].y; y--)
                    blockedSquares.Add((x, y));
            }
        }
        else if (points[i].y == points[i + 1].y)
        {
            int y = points[i].y;
            if (points[i].x < points[i + 1].x)
            {
                for (int x = points[i].x; x <= points[i + 1].x; x++)
                    blockedSquares.Add((x, y));
            }
            else
            {
                for (int x = points[i].x; x >= points[i + 1].x; x--)
                    blockedSquares.Add((x, y));
            }
        }
        else throw new Exception();
        if (points[i].y > maxY) maxY = points[i].y;
        if (points[i + 1].y > maxY) maxY = points[i + 1].y;
    }
}

int sandCount = 0;
// simulate sand
while (true)
{
    int sandX = 500;
    int sandY = 0;
    bool pluggedUp = false;
    while (true)
    {
        if (sandY == maxY + 1)
        {
            blockedSquares.Add((sandX, sandY));
            sandCount++;
            break;
        }
        else
        {
            if (!blockedSquares.Contains((sandX, sandY + 1)))
            {
                sandY++;
            }
            else if (!blockedSquares.Contains((sandX - 1, sandY + 1)))
            {
                sandX--;
                sandY++;
            }
            else if (!blockedSquares.Contains((sandX + 1, sandY + 1)))
            {
                sandX++;
                sandY++;
            }
            else
            {
                blockedSquares.Add((sandX, sandY));
                sandCount++;
                if (sandX == 500 && sandY == 0) pluggedUp = true;
                break;
            }
        }
    }
    if (pluggedUp) break;
}

Console.WriteLine(sandCount);
