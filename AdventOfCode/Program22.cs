var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input22.txt");

int height = inputs.Length - 2;
var mazeLines = inputs.Take(height);
int width = mazeLines.Select(s => s.Length).Max();

byte[][] maze = new byte[height][];
for (int i = 0; i < height; i++)
{
    maze[i] = new byte[width];
    for (int j = 0; j < inputs[i].Length; j++)
    {
        switch (inputs[i][j])
        {
            case '.':
                maze[i][j] = 1;
                break;
            case '#':
                maze[i][j] = 2;
                break;
        }
    }
}

int[] rowMins = maze.Select(row => Array.FindIndex(row, b => b != 0)).ToArray();
int[] rowMaxs = maze.Select(row => Array.FindLastIndex(row, b => b != 0)).ToArray();
int[] colMins = new int[width];
int[] colMaxs = new int[width];
for (int j = 0; j < width; j++)
{
    var col = maze.Select(row => row[j]).ToList();
    colMins[j] = col.FindIndex(b => b != 0);
    colMaxs[j] = col.FindLastIndex(b => b != 0);
}
var edges = new List<Edge>[4]
{
    new List<Edge>
    {
        new(){ xMin = 150, xMax = 151, yMin = 0, yMax = 50, directionChange = 2, newXMin = 99, newYMin = 149 }, //B
        new(){ xMin = 100, xMax = 101, yMin = 50, yMax = 100, directionChange = 3, newXMin = 100, newYMin = 49 }, //A
        new(){ xMin = 100, xMax = 101, yMin = 100, yMax = 150, directionChange = 2, newXMin = 149, newYMin = 49 }, //B
        new(){ xMin = 50, xMax = 51, yMin = 150, yMax = 200, directionChange = 3, newXMin = 50, newYMin = 149 }, //C
    },
    new List<Edge>
    {
        new(){ xMin = 0, xMax = 50, yMin = 200, yMax = 201, directionChange = 0, newXMin = 100, newYMin = 0 }, //G
        new(){ xMin = 50, xMax = 100, yMin = 150, yMax = 151, directionChange = 1, newXMin = 49, newYMin = 150 }, //C
        new(){ xMin = 100, xMax = 150, yMin = 50, yMax = 51, directionChange = 1, newXMin = 99, newYMin = 50 }, //A
    },
    new List<Edge>
    {
        new(){ xMin = 49, xMax = 50, yMin = 0, yMax = 50, directionChange = 2, newXMin = 0, newYMin = 149 }, //E
        new(){ xMin = 49, xMax = 50, yMin = 50, yMax = 100, directionChange = 3, newXMin = 0, newYMin = 100 }, //D
        new(){ xMin = -1, xMax = 0, yMin = 100, yMax = 150, directionChange = 2, newXMin = 50, newYMin = 49 }, //E
        new(){ xMin = -1, xMax = 0, yMin = 150, yMax = 200, directionChange = 3, newXMin = 50, newYMin = 0 }, //F
    },
    new List<Edge>
    {
        new(){ xMin = 0, xMax = 50, yMin = 99, yMax = 100, directionChange = 1, newXMin = 50, newYMin = 50 }, //D
        new(){ xMin = 50, xMax = 100, yMin = -1, yMax = 0, directionChange = 1, newXMin = 0, newYMin = 150 }, //F
        new(){ xMin = 100, xMax = 150, yMin = -1, yMax = 0, directionChange = 0, newXMin = 0, newYMin = 199 }, //G
    },
};

int y = 0;
int x = rowMins[0];
int direction = 0;

string commands = inputs[^1];
int step = 0;
foreach (char c in commands)
{
    if (c >= '0' && c <= '9')
    {
        step *= 10;
        step += c - '0';
    }
    else
    {
        Move(step);
        step = 0;
        if (c == 'L') direction = (direction + 3) % 4;
        else if (c == 'R') direction = (direction + 1) % 4;
        Console.WriteLine($"{x + 1}, {y + 1}, {direction}");
    }
}
Move(step);
step = 0;

Console.WriteLine(1000 * (y + 1) + 4 * (x + 1) + direction);



void Move(int steps)
{
    for (int i = 0; i < steps; i++)
    {
        if (!TryMove()) break;
    }
}

bool TryMove()
{
    int newX, newY;
    int newDirection = direction;
    switch (direction)
    {
        case 0:
            newX = x + 1;
            newY = y;
            break;
        case 1:
            newX = x;
            newY = y + 1;
            break;
        case 2:
            newX = x - 1;
            newY = y;
            break;
        case 3:
            newX = x;
            newY = y - 1;
            break;
        default:
            throw new Exception();
    }
    foreach (Edge edge in edges[direction])
    {
        if (edge.InEdge(newX, newY))
        {
            (newX, newY) = edge.NewCoords(newX, newY);
            newDirection = (direction + edge.directionChange) % 4;
        }
    }
    if (maze[newY][newX] == 1)
    {
        x = newX; y = newY;
        direction = newDirection;
        return true;
    }
    else if (maze[newY][newX] == 2)
    {
        return false;
    }
    else throw new Exception();
    //switch (direction)
    //{
    //    case 0:
    //        { //right
    //            int newX = (x == rowMaxs[y]) ? rowMins[y] : x + 1;
    //            if (maze[y][newX] == 1)
    //            {
    //                x = newX;
    //                return true;
    //            }
    //            else return false;
    //        }
    //    case 1:
    //        { //down
    //            int newY = (y == colMaxs[x]) ? colMins[x] : y + 1;
    //            if (maze[newY][x] == 1)
    //            {
    //                y = newY;
    //                return true;
    //            }
    //            else return false;
    //        }
    //    case 2:
    //        { //left
    //            int newX = (x == rowMins[y]) ? rowMaxs[y] : x - 1;
    //            if (maze[y][newX] == 1)
    //            {
    //                x = newX;
    //                return true;
    //            }
    //            else return false;
    //        }
    //    case 3:
    //        { //up
    //            int newY = (y == colMins[x]) ? colMaxs[x] : y - 1;
    //            if (maze[newY][x] == 1)
    //            {
    //                y = newY;
    //                return true;
    //            }
    //            else return false;
    //        }
    //    default:
    //        throw new Exception();
    //}
}


class Edge
{
    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;
    public int directionChange;
    public int newXMin;
    public int newYMin;

    public bool InEdge(int x, int y)
    {
        return x >= xMin &&
            x < xMax &&
            y >= yMin &&
            y < yMax;
    }

    public (int, int) NewCoords(int x, int y)
    {
        int dispX = x - xMin;
        int dispY = y - yMin;
        int rotatedDispX, rotatedDispY;
        switch (directionChange)
        {
            case 0:
                rotatedDispX = dispX;
                rotatedDispY = dispY;
                break;
            case 1:
                rotatedDispX = -dispY;
                rotatedDispY = dispX;
                break;
            case 2:
                rotatedDispX = -dispX;
                rotatedDispY = -dispY;
                break;
            case 3:
                rotatedDispX = dispY;
                rotatedDispY = -dispX;
                break;
            default:
                throw new Exception();
        }
        return (newXMin + rotatedDispX, newYMin + rotatedDispY);
    }
}