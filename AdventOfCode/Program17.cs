string input = File.ReadAllText(@"..\..\..\..\Inputs\input17.txt");
if (input[^1] == '\n') input = input[..^1];
int inputLength = input.Length;

(int, int)[][] rocks = new (int, int)[][]
{
    new (int, int)[]{(0, 0), (1, 0), (2, 0), (3, 0)},
    new (int, int)[]{(0, 1), (1, 0), (1, 1), (1, 2), (2, 1)},
    new (int, int)[]{(0, 0), (1, 0), (2, 0), (2, 1), (2, 2)},
    new (int, int)[]{(0, 0), (0, 1), (0, 2), (0, 3) },
    new (int, int)[]{(0, 0), (0, 1), (1, 0), (1, 1)}
};

bool[,] chamber = new bool[100000, 7];
int tallestRock = -1;
long skippedHeight = 0;
int gustIndex = 0;
int[] highestRocks = new int[7];

Dictionary<State, StateInfo> states = new(new MyEqualityComparer());

long totalRockCount = 1000000000000;
bool repeat = false;
for (long i = 0; i < totalRockCount; i++)
{
    int x = 2;
    int y = tallestRock + 4;
    int rockIndex = (int)(i % 5);
    while (true)
    {
        if (input[gustIndex] == '<' &&
            Placeable(x - 1, y, rockIndex))
        {
            x--;
        }
        else if (input[gustIndex] == '>' &&
            Placeable(x + 1, y, rockIndex))
        {
            x++;
        }
        gustIndex = (gustIndex + 1) % inputLength;

        if (Placeable(x, y - 1, rockIndex))
        {
            y--;
        }
        else
        {
            var stateRocks = Place(x, y, rockIndex);
            if (!repeat)
            {
                State state = new(stateRocks, rockIndex, gustIndex);
                if (states.TryGetValue(state, out StateInfo info))
                {
                    repeat = true;
                    long cycleLength = i - info.rockCount;
                    long cycleHeight = highestRocks.Min() - info.baseHeight;
                    long skipCount = (totalRockCount - i - 1) / cycleLength;
                    skippedHeight = skipCount * cycleHeight;
                    i += skipCount * cycleLength;
                }
                else
                {
                    if (highestRocks.Min() > 0)
                    {
                        states.Add(state, new StateInfo(highestRocks.Min(), i));
                    }
                }
            }
            break;
        }
    }
}

Console.WriteLine(tallestRock + skippedHeight + 1);

bool Placeable(int x, int y, int rockIndex)
{
    foreach ((int rockX, int rockY) in rocks[rockIndex])
    {
        int xx = x + rockX;
        int yy = y + rockY;
        if (xx < 0 ||
            xx >= 7 ||
            yy < 0 ||
            chamber[yy, xx])
        {
            return false;
        }
    }
    return true;
}

bool[,] Place(int x, int y, int rockIndex)
{
    foreach ((int rockX, int rockY) in rocks[rockIndex])
    {
        int xx = x + rockX;
        int yy = y + rockY;
        chamber[yy, xx] = true;
        if (yy > tallestRock)
            tallestRock = yy;
        if (yy > highestRocks[xx])
            highestRocks[xx] = yy;
    }

    int lowestY = highestRocks.Min();
    int stateHeight = tallestRock - lowestY + 1;
    bool[,] stateRocks = new bool[stateHeight, 7];
    Array.Copy(chamber, 7 * lowestY, stateRocks, 0, stateHeight * 7);
    return stateRocks;
}

record State
{
    public bool[,] rocks;
    public int rockIndex;
    public int gustIndex;

    public State(bool[,] rocks, int rockIndex, int gustIndex)
    {
        this.rocks = rocks;
        this.rockIndex = rockIndex;
        this.gustIndex = gustIndex;
    }

    public override string ToString()
    {
        return "";
    }
}

record StateInfo
{
    public long baseHeight;
    public long rockCount;

    public StateInfo(long baseHeight, long rockCount)
    {
        this.baseHeight = baseHeight;
        this.rockCount = rockCount;
    }
}

class MyEqualityComparer : IEqualityComparer<State>
{
    public bool Equals(State? x, State? y)
    {
        if (x == null || y == null)
            return (x == null && y == null);
        if (x.rockIndex != y.rockIndex)
            return false;
        if (x.gustIndex != y.gustIndex)
            return false;
        if (x.rocks.GetLength(0) != y.rocks.GetLength(0))
            return false;
        for (int i = 0; i < x.rocks.GetLength(0); i++)
        {
            for (int j = 0; j < x.rocks.GetLength(1); j++)
            {
                if (x.rocks[i, j] != y.rocks[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public int GetHashCode(State obj)
    {
        int result = 17;
        result = result * 23 + obj.rockIndex;
        result = result * 23 + obj.gustIndex;
        for (int i = 0; i < obj.rocks.GetLength(0); i++)
        {
            for (int j = 0; j < obj.rocks.GetLength(1); j++)
            {
                unchecked
                {
                    result = result * 23 + (obj.rocks[i, j] ? 1 : 0);
                }
            }
        }
        return result;
    }
}