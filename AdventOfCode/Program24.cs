var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input24.txt");

Map.height = inputs.Length - 2;
Map.width = inputs[0].Length - 2;

for (int i = 0; i < Map.height; i++)
{
    for (int j = 0; j < Map.width; j++)
    {
        char d = inputs[i + 1][j + 1];
        if (d != '.')
        {
            _ = new Blizzard(j, i, d);
        }
    }
}

PriorityQueue<State, int> locations = new();
var beginState = new State { t = 0, x = 0, y = -1, visited = 0 };
locations.Enqueue(beginState, Map.Priority(beginState));
while (true)
{
    State state = locations.Dequeue();
    if (state.visited == 3)
    {
        Console.Write(state.t);
        break;
    }
    foreach ((int dx, int dy) in new (int, int)[]
    {
        (0, 1), (1, 0), (0, 0), (0, -1), (-1, 0)
    })
    {
        var newState = state.Shift(dx, dy);
        if (Map.InBounds(newState) &&
            Map.IsClear(newState) &&
            !Map.checkedPoints.Contains(newState)
            )
        {
            Map.checkedPoints.Add(newState);
            locations.Enqueue(newState, Map.Priority(newState));
        }
    }
}

record State
{
    public int t;
    public int x;
    public int y;
    public int visited;

    public State Shift(int dx, int dy)
    {
        int newVisited = visited;
        if (x + dx == Map.width - 1 && y + dy == Map.height && visited % 2 == 0)
            newVisited++;
        if (x + dx == 0 && y + dy == 0 && visited % 2 == 1)
            newVisited++;
        return new State
        {
            t = t + 1,
            x = x + dx,
            y = y + dy,
            visited = newVisited
        };
    }
}

static class Map
{
    static public int width;
    static public int height;
    static public List<bool[,]> points = new();
    static public HashSet<State> checkedPoints = new();

    static public bool InBounds(State state)
    {
        int x = state.x, y = state.y;
        return (x >= 0 && x < width && y >= 0 && y < height) ||
            (x == 0 && y == -1) ||
            (x == width - 1 && y == height);
    }

    static public int Mod(int a, int b)
    {
        return ((a % b) + b) % b;
    }

    static public bool IsClear(State state)
    {
        int t = state.t, x = state.x, y = state.y;
        var grid = GetGrid(t);
        if ((x == 0 && y == -1) || (x == width - 1 && y == height))
            return true;
        return !grid[x, y];
    }

    static public bool[,] GetGrid(int index)
    {
        for (int i = points.Count; i <= index; i++)
        {
            points.Add(Blizzard.GetGrid(i));
        }
        return points[index];
    }

    static public int Priority(State state)
    {
        int t = state.t, x = state.x, y = state.y;
        int dist = state.visited % 2 == 0 ? 
            (width - 1 - x) + (height - y) : 
            x + (y + 1);
        dist += (2 - state.visited) * (width + height);
        return t + dist;
    }
}

class Blizzard
{
    public static List<Blizzard> blizzards = new();
    public int x, y;
    public int direction;

    public Blizzard(int x, int y, char d)
    {
        this.x = x;
        this.y = y;
        direction = d switch
        {
            '>' => 0,
            'v' => 1,
            '<' => 2,
            '^' => 3,
            _ => throw new Exception(),
        };
        blizzards.Add(this);
    }

    public static bool[,] GetGrid(int time)
    {
        bool[,] grid = new bool[Map.width, Map.height];
        foreach (var blizzard in blizzards)
        {
            (int x, int y) = blizzard.GetPosition(time);
            grid[x, y] = true;
        }
        return grid;
    }

    public (int, int) GetPosition(int time)
    {
        return direction switch
        {
            0 => (Map.Mod(x + time, Map.width), y),
            1 => (x, Map.Mod(y + time, Map.height)),
            2 => (Map.Mod(x - time, Map.width), y),
            3 => (x, Map.Mod(y - time, Map.height)),
            _ => throw new Exception(),
        };
    }
}