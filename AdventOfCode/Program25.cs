var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input25.txt");

long total = 0;
foreach (var input in inputs)
{
    total += Snafu.ToInt(input);
}
Console.WriteLine(Snafu.FromInt(total));

class Snafu
{
    static public long ToInt(string s)
    {
        long n = 0;
        foreach (char c in s)
        {
            n *= 5;
            n += c switch
            {
                '0' => 0,
                '1' => 1,
                '2' => 2,
                '-' => -1,
                '=' => -2,
                _ => throw new Exception()
            };
        }
        return n;
    }

    static public string FromInt(long n)
    {
        if (n == 0) return "0";
        string s = "";
        while (n != 0)
        {
            switch (((n % 5) + 5) % 5)
            {
                case 0:
                    s = '0' + s;
                    n -= 0;
                    break;
                case 1:
                    s = '1' + s;
                    n -= 1;
                    break;
                case 2:
                    s = '2' + s;
                    n -= 2;
                    break;
                case 3:
                    s = '=' + s;
                    n += 2;
                    break;
                case 4:
                    s = '-' + s;
                    n += 1;
                    break;
                default:
                    throw new Exception();
            }
            n /= 5;
        }
        return s;
    }
}