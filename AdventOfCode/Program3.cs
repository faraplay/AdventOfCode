var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input3.txt");
int score = 0;
foreach (string input in inputs)
{
    int length = input.Length / 2;
    bool[] firstContains = new bool[53];

    string sack1 = input[0..length];
    string sack2 = input[length..];
    foreach (char c in sack1)
    {
        firstContains[getCode(c)] = true;
    }
    int repeated = -1;
    foreach (char c in sack2)
    {
        int code = getCode(c);
        if (firstContains[code])
        {
            if (repeated != -1 && repeated != code)
            {
                throw new Exception("1");
            }
            repeated = getCode(c);
        }
    }
    if (repeated == -1) throw new Exception("2");

    score += repeated;

    Console.WriteLine($"{repeated}");
}
Console.WriteLine(score);

score = 0;
for (int i = 0; i < inputs.Length;)
{
    int[] contains = new int[53];
    foreach (char c in inputs[i])
    {
        contains[getCode(c)] |= 0x1;
    }
    i++;
    foreach (char c in inputs[i])
    {
        contains[getCode(c)] |= 0x2;
    }
    i++;
    foreach (char c in inputs[i])
    {
        contains[getCode(c)] |= 0x4;
    }
    i++;
    int repeated = -1;
    for (int j = 0; j < 53; j++)
    {
        if (contains[j] == 0x7)
        {
            if (repeated != -1) throw new Exception("3");
            repeated = j;
        }
    }
    if (repeated == -1) throw new Exception("4");
    Console.WriteLine(repeated);
    score += repeated;
}
Console.WriteLine(score);

int getCode(char c)
{
    if ('a' <= c && c <= 'z')
    {
        return c - 'a' + 1;
    }
    else if ('A' <= c && c <= 'Z')
    {
        return c - 'A' + 27;
    }
    else throw new Exception();
}