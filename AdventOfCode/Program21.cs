using System.Text.RegularExpressions;

var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input21.txt");

Regex numRegex = new Regex(@"([a-z]{4}): ([0-9]+)");
Regex refRegex = new Regex(@"([a-z]{4}): ([a-z]{4}) (.) ([a-z]{4})");
foreach (var input in inputs)
{
    if (input.StartsWith("humn"))
    {
        Monkey.monkeys.Add("humn", new Human());
        continue;
    }
    var numMatch = numRegex.Match(input);
    if (numMatch.Success)
    {
        Monkey.monkeys.Add(numMatch.Groups[1].Value,
            new NumberMonkey(long.Parse(numMatch.Groups[2].Value)));
        continue;
    }
    var refMatch = refRegex.Match(input);
    if (refMatch.Success)
    {
        if (refMatch.Groups[1].Value == "root")
            Monkey.monkeys.Add(refMatch.Groups[1].Value,
                new RefMonkey(refMatch.Groups[2].Value, refMatch.Groups[4].Value, '-'));
        else
            Monkey.monkeys.Add(refMatch.Groups[1].Value,
                new RefMonkey(refMatch.Groups[2].Value, refMatch.Groups[4].Value, refMatch.Groups[3].Value[0]));
    }
}

var root = Monkey.monkeys["root"];
Console.WriteLine($"({root.XCoeff}x + {root.Value})/{root.Denominator}");

Console.WriteLine(-root.Value / root.XCoeff);


abstract class Monkey
{
    static public Dictionary<string, Monkey> monkeys = new();
    abstract public long Value { get; }
    abstract public long XCoeff { get; }
    abstract public long Denominator { get; }
}

class NumberMonkey : Monkey
{
    long value;

    public NumberMonkey(long value)
    {
        this.value = value;
    }

    public override long Value => value;
    public override long XCoeff => 0;
    public override long Denominator => 1;
}

class RefMonkey : Monkey
{
    string monkey1;
    string monkey2;
    char operation;

    public RefMonkey(string monkey1, string monkey2, char operation)
    {
        this.monkey1 = monkey1;
        this.monkey2 = monkey2;
        this.operation = operation;
    }

    public Monkey Left => monkeys[monkey1];
    public Monkey Right => monkeys[monkey2];

    private long value = 0;
    private long xCoeff = 0;
    private long denominator = 0;
    private bool calculated = false;

    private void Calculate()
    {
        calculated = true;
        switch (operation)
        {
            case '+':
                value = Left.Value * Right.Denominator + Right.Value * Left.Denominator;
                xCoeff = Left.XCoeff * Right.Denominator + Right.XCoeff * Left.Denominator;
                denominator = Left.Denominator * Right.Denominator;
                break;
            case '-':
                value = Left.Value * Right.Denominator - Right.Value * Left.Denominator;
                xCoeff = Left.XCoeff * Right.Denominator - Right.XCoeff * Left.Denominator;
                denominator = Left.Denominator * Right.Denominator;
                break;
            case '*':
                value = Left.Value * Right.Value;
                if (Left.XCoeff == 0)
                    xCoeff = Left.Value * Right.XCoeff;
                else if (Right.XCoeff == 0)
                    xCoeff = Left.XCoeff * Right.Value;
                else
                    throw new Exception();
                denominator = Left.Denominator * Right.Denominator;
                break;
            case '/':
                if (Right.XCoeff != 0)
                    throw new Exception();
                value = Left.Value * Right.Denominator;
                xCoeff = Left.XCoeff * Right.Denominator;
                denominator = Left.Denominator * Right.Value;
                break;
            default:
                throw new Exception();
        }

        long gcd = GCD(GCD(value, xCoeff), denominator);
        value /= gcd;
        xCoeff /= gcd;
        denominator /= gcd;
    }

    static long GCD(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (true)
        {
            if (a == 0) return b;
            b %= a;
            if (b == 0) return a;
            a %= b;
        }
    }

    public override long Value
    {
        get
        {
            if (!calculated) Calculate();
            return value;
        }
    }

    public override long XCoeff
    {
        get
        {
            if (!calculated) Calculate();
            return xCoeff;
        }
    }

    public override long Denominator
    {
        get
        {
            if (!calculated) Calculate();
            return denominator;
        }
    }
}

class Human : Monkey
{
    public override long Value => 0;

    public override long XCoeff => 1;
    public override long Denominator => 1;
}