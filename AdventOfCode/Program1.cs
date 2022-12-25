// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input1.txt");
var calories = new List<int>();
int calorieCount = 0;
foreach (string input in inputs)
{
    if (string.IsNullOrEmpty(input))
    {
        calories.Add(calorieCount);
        calorieCount = 0;
    }
    else
    {
        calorieCount += int.Parse(input);
    }
}
if (calorieCount > 0) calories.Add(calorieCount);

calories.Sort();
foreach (int cal in calories)
{
    Console.WriteLine(cal);
}
Console.WriteLine("The maximum is");
Console.WriteLine(calories[^1]);

Console.WriteLine("The sum of the top 3 is");
Console.WriteLine(calories[^1] + calories[^2] + calories[^3]);