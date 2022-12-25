var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input12.txt");

int height = inputs.Length;
int width = inputs[0].Length;
int startX = 0, startY = 0;
int endX = 0, endY = 0;
int[,] heights = new int[height, width];

for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        char c = inputs[i][j];
        if (c == 'S')
        {
            startY = i;
            startX = j;
            heights[i, j] = 'a' - 'a';
        }
        else if (c == 'E')
        {
            endY = i;
            endX = j;
            heights[i, j] = 'z' - 'a';
        }
        else
        {
            heights[i, j] = c - 'a';
        }
    }
}

int[,] distances = new int[height, width];
for (int i = 0; i < height; i++)
    for (int j = 0; j < width; j++)
        distances[i, j] = -1;
Queue<(int, int)> positions = new();
positions.Enqueue((endY, endX));
distances[endY, endX] = 0;

while (positions.Count > 0)
{
    (int y, int x) = positions.Dequeue();
    int thisHeight = heights[y, x];
    int thisDistance = distances[y, x];
    foreach ((int newY, int newX) in new(int, int)[]
        {
            (y, x-1),
            (y, x+1),
            (y-1, x),
            (y+1, x)
        })
    {
        if (newX >= 0 &&
            newX < width &&
            newY >= 0 &&
            newY < height &&
            heights[newY, newX] >= thisHeight - 1 &&
            distances[newY, newX] == -1)
        {
            distances[newY, newX] = thisDistance + 1;
            positions.Enqueue((newY, newX));
        }
    }
}


int closestDist = int.MaxValue;
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        if (heights[i,j] == 'a' - 'a' && distances[i,j] != -1 && distances[i,j] < closestDist)
        {
            closestDist = distances[i,j];
        }
    }
}

Console.WriteLine(closestDist);