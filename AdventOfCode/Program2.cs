
var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input2.txt");
int score = 0;
foreach (string input in inputs)
{
    char opponentChoice = input[0];
    char myChoice = input[2];
    int thisScore = 0;

    if (myChoice == 'X') thisScore += 1;
    else if (myChoice == 'Y') thisScore += 2;
    else if (myChoice == 'Z') thisScore += 3;
    else throw new Exception("mychoice is weird");

    int outcome = ((short)myChoice - (short)opponentChoice) % 3;
    if (outcome == 0) thisScore += 6; // e.g. i choose Y, opponent chooses A
    else if (outcome == 1) thisScore += 0; // e.g.i choose Z, opponent chooses A
    else if (outcome == 2) thisScore += 3; //e.g. i choose X, opponent chooses A
    else throw new Exception("outcome is weird");

    score += thisScore;

    // Console.WriteLine($"{myChoice} {opponentChoice} {thisScore}");
}
Console.WriteLine($"Total score is {score}");

int score2 = 0;
foreach (string input in inputs)
{
    char opponentChoice = input[0];
    char myOutcome = input[2];
    int thisScore = 0;

    if (myOutcome == 'X') thisScore += 0;
    else if (myOutcome == 'Y') thisScore += 3;
    else if (myOutcome == 'Z') thisScore += 6;
    else throw new Exception("mychoice is weird");

    int myChoice = ((short)myOutcome - (short)'X' + (short)opponentChoice - (short)'A') % 3;
    if (myChoice == 0) thisScore += 3; // e.g. opponent chooses A, outcome is X
    else if (myChoice == 1) thisScore += 1; // e.g. opponent chooses A, outcome is Y
    else if (myChoice == 2) thisScore += 2; //e.g. opponent chooses A, outcome is Z
    else throw new Exception("outcome is weird");

    score2 += thisScore;

    Console.WriteLine($"{myOutcome} {opponentChoice} {thisScore}");
}
Console.WriteLine($"Total score is {score2}");
