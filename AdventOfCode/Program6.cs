var reader = File.OpenText(@"..\..\..\..\Inputs\input6.txt");
var input = reader.ReadToEnd();

int rangeStart = 0;
int rangeEnd = 0;

while (rangeEnd - rangeStart < 14)
{
    char newChar = input[rangeEnd];

    for (int i = rangeEnd - 1; i >= rangeStart; i--)
    {
        if (input[i] == newChar)
        {
            rangeStart = i + 1;
            break;
        }
    }
    rangeEnd++;
}
for (int i = rangeStart; i < rangeEnd; i++)
{
    Console.Write(input[i]);
}
Console.WriteLine();
Console.WriteLine(rangeEnd);