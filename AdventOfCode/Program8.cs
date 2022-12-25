var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input8.txt");
int colLen = inputs.Length;
int rowLen = inputs[0].Length;

int[,] heights = new int[colLen, rowLen];
for (int i = 0; i < colLen; i++)
{
    for (int j = 0; j < rowLen; j++)
    {
        heights[i, j] = inputs[i][j] - '0';
    }
}

int[,,] visibility = new int[colLen, rowLen, 4];
// rightwards
for (int i = 0; i < colLen; i++)
{
    int[] closestIndex = new int[10];
    Array.Fill(closestIndex, 0);
    for (int j = 0; j < rowLen; j++)
    {
        visibility[i, j, 0] = j - closestIndex[heights[i, j]];
        for (int k = heights[i, j]; k >= 0; k--)
        {
            closestIndex[k] = j;
        }
    }
}
// leftwards
for (int i = 0; i < colLen; i++)
{
    int[] closestIndex = new int[10];
    Array.Fill(closestIndex, rowLen - 1);
    for (int j = rowLen - 1; j >= 0; j--)
    {
        visibility[i, j, 1] = closestIndex[heights[i, j]] - j;
        for (int k = heights[i, j]; k >= 0; k--)
        {
            closestIndex[k] = j;
        }
    }
}
// downwards
for (int j = 0; j < rowLen; j++)
{
    int[] closestIndex = new int[10];
    Array.Fill(closestIndex, 0);
    for (int i = 0; i < colLen; i++)
    {
        visibility[i, j, 2] = i - closestIndex[heights[i, j]];
        for (int k = heights[i, j]; k >= 0; k--)
        {
            closestIndex[k] = i;
        }
    }
}
// upwards
for (int j = 0; j < rowLen; j++)
{
    int[] closestIndex = new int[10];
    Array.Fill(closestIndex, colLen - 1);
    for (int i = colLen - 1; i >= 0; i--)
    {
        visibility[i, j, 3] = closestIndex[heights[i, j]] - i;
        for (int k = heights[i, j]; k >= 0; k--)
        {
            closestIndex[k] = i;
        }
    }
}

int bestScore = 0;
for (int i = 0; i < colLen; i++)
{
    for (int j = 0; j < rowLen; j++)
    {
        int score = visibility[i,j,0] * visibility[i,j,1] * visibility[i,j,2] * visibility[i,j,3];
        if (score > bestScore)
        {
            bestScore = score;
        }
    }
}
Console.WriteLine(bestScore);