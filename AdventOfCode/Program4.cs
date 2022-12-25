using System.Text.RegularExpressions;

var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input4.txt");
int score = 0;
Regex regex = new Regex(@"^([0-9]+)-([0-9]+),([0-9]+)-([0-9]+)$");
foreach (string input in inputs)
{
    var match = regex.Match(input);
    if (!match.Success) throw new Exception("1");
    int start1 = int.Parse(match.Groups[1].Value);
    int end1 = int.Parse(match.Groups[2].Value);
    int start2 = int.Parse(match.Groups[3].Value);
    int end2 = int.Parse(match.Groups[4].Value);

    if (start1 >= start2 && end1 <= end2) // interval1 inside interval2
        score++;
    else if (start1 <= start2 && end1 >= end2) // interval2 inside interval1
        score++;
}
Console.WriteLine(score);

score = 0;
foreach (string input in inputs)
{
    var match = regex.Match(input);
    if (!match.Success) throw new Exception("1");
    int start1 = int.Parse(match.Groups[1].Value);
    int end1 = int.Parse(match.Groups[2].Value);
    int start2 = int.Parse(match.Groups[3].Value);
    int end2 = int.Parse(match.Groups[4].Value);

    if (!(start1 > end2 || start2 > end1))
        score++;
}
Console.WriteLine(score);