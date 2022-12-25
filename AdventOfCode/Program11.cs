var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input11.txt");

int monkeyCount = (inputs.Length + 1) / 7;
var monkeys = new MonkeyData[monkeyCount];

for (int i = 0; i < monkeyCount; i++)
{
    var items = inputs[7 * i + 1][17..]
        .Split(',')
        .Select(s => long.Parse(s.Trim()));
    char operationType = inputs[7 * i + 2][23];
    string operationValueString = inputs[7 * i + 2][25..];
    int testDivisor = int.Parse(inputs[7 * i + 3][21..]);
    int trueMonkey = int.Parse(inputs[7 * i + 4][29..]);
    int falseMonkey = int.Parse(inputs[7 * i + 5][30..]);

    monkeys[i] = new MonkeyData(
        monkeys,
        items,
        operationType,
        operationValueString,
        testDivisor,
        trueMonkey,
        falseMonkey);
}

for (int i = 0; i < 10000; i++)
{
    for (int j=0; j<monkeyCount; j++)
    {
        monkeys[j].HandleItems();
    }
    //foreach (var monkey in monkeys)
    //{
    //    foreach (var item in monkey.items)
    //    {
    //        Console.Write($"{item}, ");
    //    }
    //    Console.WriteLine();
    //}
    //Console.WriteLine();
}

foreach (var monkey in monkeys) Console.WriteLine(monkey.inspectedCount);
var mostActive = monkeys.Select(m => (long)m.inspectedCount).ToList();
mostActive.Sort();
Console.WriteLine(mostActive[^2] * mostActive[^1]);

class MonkeyData
{
    public Queue<long> items;
    public Func<long, long> operation;
    public int testDivisor;
    public int trueMonkey;
    public int falseMonkey;

    public int inspectedCount = 0;

    private MonkeyData[] monkeys;
    private int operationValue;

    public MonkeyData(
        MonkeyData[] monkeys,
        IEnumerable<long> items,
        char operationType,
        string operationValueString,
        int testDivisor,
        int trueMonkey,
        int falseMonkey)
    {
        this.monkeys = monkeys;
        this.items = new Queue<long>(items);
        if (operationValueString == "old")
        {
            if (operationType == '+')
                operation = old => old + old;
            else
                operation = old => old * old;
        }
        else
        {
            operationValue = int.Parse(operationValueString);
            if (operationType == '+')
                operation = old => old + operationValue;
            else
                operation = old => old * operationValue;
        }
        this.testDivisor = testDivisor;
        this.trueMonkey = trueMonkey;
        this.falseMonkey = falseMonkey;
    }

    public void HandleItems()
    {
        while (items.Count > 0)
        {
            HandleItem();
            inspectedCount++;
        }
    }

    private void HandleItem()
    {
        long item = items.Dequeue();
        //Console.WriteLine($"Inspect item {item}");
        item = operation(item);
        //Console.WriteLine($"item is now {item}");
        item %= 2 * 3 * 5 * 7 * 11 * 13 * 17 * 19 * 23;
        //Console.WriteLine($"item is now {item}");
        if (item % testDivisor == 0)
        {
            //Console.WriteLine($"item is divisible by {testDivisor}");
            //Console.WriteLine($"Giving to monkey {trueMonkey}");
            monkeys[trueMonkey].items.Enqueue(item);
        }
        else
        {
            //Console.WriteLine($"item is not divisible by {testDivisor}");
            //Console.WriteLine($"Giving to monkey {falseMonkey}");
            monkeys[falseMonkey].items.Enqueue(item);
        }
    }
}