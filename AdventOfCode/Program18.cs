using System.Text.RegularExpressions;

var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input18.txt");
var regex = new Regex(@"^([0-9]+),([0-9]+),([0-9]+)$");

var surfacess = new HashSet<(int, int, int)>[3]
{
    new(), new(), new()
};
HashSet<(int, int, int)> blocks = new();

int minX = int.MaxValue, maxX = int.MinValue;
int minY = int.MaxValue, maxY = int.MinValue;
int minZ = int.MaxValue, maxZ = int.MinValue;

foreach (var input in inputs)
{
    var match = regex.Match(input);
    int x = int.Parse(match.Groups[1].Value);
    int y = int.Parse(match.Groups[2].Value);
    int z = int.Parse(match.Groups[3].Value);
    if (x < minX) minX = x;
    if (x > maxX) maxX = x;
    if (y < minY) minY = y;
    if (y > maxY) maxY = y;
    if (z < minZ) minZ = z;
    if (z > maxZ) maxZ = z;
    blocks.Add((x, y, z));
}

minX--; maxX += 2;
minY--; maxY += 2;
minZ--; maxZ += 2;
HashSet<(int, int, int)> steams = new();
Queue<(int, int, int)> steamQueue = new();
steams.Add((minX, minY, minZ));
steamQueue.Enqueue((minX, minY, minZ));

while (steamQueue.Count > 0)
{
    (int x, int y, int z) = steamQueue.Dequeue();
    TryAdd(x - 1, y, z);
    TryAdd(x + 1, y, z);
    TryAdd(x, y - 1, z);
    TryAdd(x, y + 1, z);
    TryAdd(x, y, z - 1);
    TryAdd(x, y, z + 1);
}

for (int x = minX; x < maxX; x++)
{
    for (int y = minY; y < maxY; y++)
    {
        for (int z = minZ; z < maxZ; z++)
        {
            if (!steams.Contains((x, y, z)) &&
                !blocks.Contains((x, y, z)))
            {
                Console.WriteLine($"{x},{y},{z}");
                blocks.Add((x, y, z));
            }
        }
    }
}

foreach ((int x, int y, int z) in blocks)
{
    Toggle(surfacess[0], x, y, z);
    Toggle(surfacess[0], x + 1, y, z);
    Toggle(surfacess[1], x, y, z);
    Toggle(surfacess[1], x, y + 1, z);
    Toggle(surfacess[2], x, y, z);
    Toggle(surfacess[2], x, y, z + 1);
}
Console.WriteLine(surfacess.Select(surfaces => surfaces.Count).Sum());

void Toggle(HashSet<(int, int, int)> surfaces, int x, int y, int z)
{
    if (surfaces.Contains((x, y, z)))
        surfaces.Remove((x, y, z));
    else surfaces.Add((x, y, z));
}

void TryAdd(int x, int y, int z)
{
    if (x >= minX && x < maxX &&
        y >= minY && y < maxY &&
        z >= minZ && z < maxZ &&
        !steams.Contains((x, y, z)) &&
        !blocks.Contains((x, y, z)))
    {
        steams.Add((x, y, z));
        steamQueue.Enqueue((x, y, z));
    }
}