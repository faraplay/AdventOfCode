var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input20.txt");

long key = 811589153;

var list = inputs.Select((x, index) => (key * int.Parse(x), index)).ToList();
int len = list.Count;

for (int k = 0; k < 10; k++)
{
    for (int j = 0; j < len; j++)
    {
        int i = list.FindIndex(pair => pair.index == j);
        long num = list[i].Item1;
        list.RemoveAt(i);
        int newIndex = (int)Modulo(i + num, len - 1);
        list.Insert(newIndex, (num, j));
    }
}

//foreach ((long num, _) in list)
//{
//    Console.WriteLine($"{num}");
//}

int zeroIndex = list.FindIndex(pair => pair.Item1 == 0);
long total = 0;
for (int i = 1000; i <= 3000; i += 1000)
{
    total += list[(zeroIndex + i) % len].Item1;
}
Console.WriteLine(total);



long Modulo(long num, int m)
{
    return ((num % m) + m) % m;
}