var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input23.txt");

for (int i = 0; i < inputs.Length; i++)
{
    for (int j = 0; j < inputs[i].Length; j++)
    {
        if (inputs[i][j] == '#')
        {
            Elf elf = new Elf(j, i);
        }
    }
}

int moveCount = 0;
do
{
    Elf.CalculateElfLocations();
    Elf.ProposeAll();
    Elf.direction++;
    moveCount++;
} while (Elf.MoveAll());
Console.WriteLine(moveCount);

class Elf
{
    public static List<Elf> elves = new();
    public static HashSet<(int, int)> elfLocations = new();
    public static Dictionary<(int, int), Elf> proposals = new();


    public int x;
    public int y;
    public int newX;
    public int newY;
    public static int direction = 0;
    public bool willMove;

    public Elf(int x, int y)
    {
        this.x = x;
        this.y = y;
        elves.Add(this);
    }

    public static void CalculateElfLocations()
    {
        elfLocations = elves.Select(elf => (elf.x, elf.y)).ToHashSet();
    }

    public static void ProposeAll()
    {
        proposals.Clear();
        foreach (var elf in elves)
        {
            elf.Propose();
        }
    }

    public static bool MoveAll()
    {
        bool anyoneMoved = false;
        foreach (var elf in elves)
        {
            if (elf.willMove)
            {
                anyoneMoved = true;
                elf.x = elf.newX;
                elf.y = elf.newY;
            }
        }
        return (anyoneMoved);
    }

    public static int EmptyTiles()
    {
        int minX = elves.Select(elf => elf.x).Min();
        int maxX = elves.Select(elf => elf.x).Max();
        int minY = elves.Select(elf => elf.y).Min();
        int maxY = elves.Select(elf => elf.y).Max();
        int area = (maxX - minX + 1) * (maxY - minY + 1);
        return area - elves.Count();
    }

    public void Propose()
    {
        willMove = false;
        if (IsAlone()) return;
        bool canMove = false;
        for (int d = direction; d < direction + 4; d++)
        {
            if (IsFree(d))
            {
                canMove = true;
                (newX, newY) = NewLocation(d);
                break;
            }
        }
        if (!canMove) return;
        if (proposals.TryGetValue((newX, newY), out var otherElf))
        {
            willMove = false;
            otherElf.willMove = false;
        }
        else
        {
            willMove = true;
            proposals.Add((newX, newY), this);
        }
    }

    bool IsAlone()
    {
        return
            !elfLocations.Contains((x - 1, y - 1)) &&
            !elfLocations.Contains((x, y - 1)) &&
            !elfLocations.Contains((x + 1, y - 1)) &&
            !elfLocations.Contains((x - 1, y)) &&
            !elfLocations.Contains((x + 1, y)) &&
            !elfLocations.Contains((x - 1, y + 1)) &&
            !elfLocations.Contains((x, y + 1)) &&
            !elfLocations.Contains((x + 1, y + 1));
    }

    bool IsFree(int direction)
    {
        return (direction % 4) switch
        {
            0 => !elfLocations.Contains((x - 1, y - 1)) &&
                    !elfLocations.Contains((x, y - 1)) &&
                    !elfLocations.Contains((x + 1, y - 1)),
            1 => !elfLocations.Contains((x - 1, y + 1)) &&
                    !elfLocations.Contains((x, y + 1)) &&
                    !elfLocations.Contains((x + 1, y + 1)),
            2 => !elfLocations.Contains((x - 1, y - 1)) &&
                    !elfLocations.Contains((x - 1, y)) &&
                    !elfLocations.Contains((x - 1, y + 1)),
            3 => !elfLocations.Contains((x + 1, y - 1)) &&
                    !elfLocations.Contains((x + 1, y)) &&
                    !elfLocations.Contains((x + 1, y + 1)),
            _ => throw new Exception(),
        };
    }

    (int, int) NewLocation(int direction)
    {
        return (direction % 4) switch
        {
            0 => (x, y - 1),
            1 => (x, y + 1),
            2 => (x - 1, y),
            3 => (x + 1, y),
            _ => throw new Exception(),
        };
    }
}