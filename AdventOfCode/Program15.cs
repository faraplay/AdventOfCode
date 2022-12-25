using System.Text.RegularExpressions;

var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input15.txt");

int y = 2000000;
//int y = 10;

List<(int, int)> intervals = new();
List<(int, int)> knownBeacons = new();
List<(int x, int y, int r)> sensors = new();

Regex regex = new(@"^Sensor at x=(-?[0-9]+), y=(-?[0-9]+): closest beacon is at x=(-?[0-9]+), y=(-?[0-9]+)$");
foreach (var input in inputs)
{
    var match = regex.Match(input);
    int sensorX = int.Parse(match.Groups[1].Value);
    int sensorY = int.Parse(match.Groups[2].Value);
    int baconX = int.Parse(match.Groups[3].Value);
    int baconY = int.Parse(match.Groups[4].Value);
    int radius = Math.Abs(baconX - sensorX) + Math.Abs(baconY - sensorY);

    sensors.Add((sensorX, sensorY, radius));
    if (!knownBeacons.Contains((baconX, baconY)))
    {
        knownBeacons.Add((baconX, baconY));
    }

}


foreach ((int sensorX, int sensorY, int radius) in sensors)
{
    if (y > sensorY + radius) { }
    else if (y >= sensorY)
    {
        int intervalRadius = radius - (y - sensorY);
        int intervalLeft = sensorX - intervalRadius;
        int intervalRight = sensorX + intervalRadius + 1;
        intervals.Add((intervalLeft, intervalRight));
    }
    else if (y >= sensorY - radius)
    {
        int intervalRadius = radius - (sensorY - y);
        int intervalLeft = sensorX - intervalRadius;
        int intervalRight = sensorX + intervalRadius + 1;
        intervals.Add((intervalLeft, intervalRight));
    }
}

foreach ((int intervalLeft, int intervalRight) in intervals)
{
    Console.WriteLine($"{intervalLeft}, {intervalRight}");
}
Console.WriteLine();
int totalLength = 0;
foreach ((int intervalLeft, int intervalRight) in Compress(intervals))
{
    Console.WriteLine($"{intervalLeft}, {intervalRight}");
    totalLength += intervalRight - intervalLeft;
}
totalLength -= knownBeacons.Where(b => b.Item2 == y).Count();
Console.WriteLine(totalLength);



// part b
List<(int, int, int, int)> boundaryBoxes =
    sensors.Select(sensor => (
    sensor.x - sensor.y - sensor.r,
    sensor.x - sensor.y + sensor.r + 1,
    sensor.x + sensor.y - sensor.r,
    sensor.x + sensor.y + sensor.r + 1
    )).ToList();

List<int> diffBoundaries = new();
diffBoundaries.Add(int.MinValue / 4);
diffBoundaries.Add(int.MaxValue / 4);
foreach (var b in boundaryBoxes)
{
    if (!diffBoundaries.Contains(b.Item1))
        diffBoundaries.Add(b.Item1);
    if (!diffBoundaries.Contains(b.Item2))
        diffBoundaries.Add(b.Item2);
}
diffBoundaries.Sort();

Console.WriteLine();
int beaconX = -1;
int beaconY = -1;
for (int i = 0; i < diffBoundaries.Count - 1; i++)
{
    int diffL = diffBoundaries[i];
    int diffR = diffBoundaries[i + 1];
    List<(int, int)> sumIntervals = new();
    foreach (var b in boundaryBoxes)
    {
        if (diffL >= b.Item1 && diffR <= b.Item2)
        {
            sumIntervals.Add((b.Item3, b.Item4));
        }
    }
    List<(int, int)> compressedIntervals = Compress(sumIntervals);
    List<int> sumBoundaries = new();
    sumBoundaries.Add(int.MinValue / 4);
    foreach (var interval in compressedIntervals)
    {
        sumBoundaries.Add(interval.Item1);
        sumBoundaries.Add(interval.Item2);
    }
    sumBoundaries.Add(int.MaxValue / 4);
    for (int j = 0; j < sumBoundaries.Count; j += 2)
    {
        int sumL = sumBoundaries[j];
        int sumR = sumBoundaries[j + 1];
        if (Overlaps(diffL, diffR, sumL, sumR))
        {
            Console.WriteLine($"{diffL}, {diffR}, {sumL}, {sumR}");
            int diffWidth = diffR - diffL;
            int sumWidth = sumR - sumL;
            if ((diffL % 2) == (sumL % 2))
            {
                if (diffWidth + sumWidth < 4)
                {
                    beaconX = (sumL + diffL) / 2;
                    beaconY = (sumL - diffL) / 2;
                }
                else
                {
                    throw new Exception("Region too big");
                }
            }
            else
            {
                if (diffWidth == 1 && (sumWidth == 2 || sumWidth == 3))
                {
                    beaconX = (sumL + 1 + diffL) / 2;
                    beaconY = (sumL + 1 - diffL) / 2;
                }
                else if (sumWidth == 1 && (diffWidth == 2 || diffWidth == 3))
                {
                    beaconX = (sumL + 1 + diffL) / 2;
                    beaconY = (sumL - 1 - diffL) / 2;
                }
                else
                {
                    throw new Exception("Region too big");
                }
            }
        }
    }
}
Console.WriteLine($"{beaconX}, {beaconY}");
Console.WriteLine(4000000 * (long)beaconX + beaconY);



static List<(int, int)> Compress(List<(int, int)> intervals)
{
    intervals.Sort(delegate ((int left, int right) a1, (int left, int right) a2)
    {
        return a1.left.CompareTo(a2.left);
    });
    if (intervals.Count == 0) return new List<(int, int)>();
    List<(int, int)> compressedIntervals = new();
    (int left, int right) = intervals[0];
    for (int i = 1; i < intervals.Count; i++)
    {
        if (right < intervals[i].Item1)
        {
            compressedIntervals.Add((left, right));
            (left, right) = intervals[i];
        }
        else if (right < intervals[i].Item2)
        {
            right = intervals[i].Item2;
        }
    }
    compressedIntervals.Add((left, right));
    return compressedIntervals;
}

static bool Overlaps(int diffL, int diffR, int sumL, int sumR)
{
    const int boxSize = 4000000 + 1;
    return 
        (diffL + sumR > 2 * 0) &&
        (diffR + sumL < 2 * boxSize) &&
        (sumR - diffR > 2 * 0) &&
        (sumL - diffL < 2 * boxSize) &&
        (sumR > 0) &&
        (sumL < boxSize + boxSize) &&
        (diffL > -boxSize) &&
        (diffR < boxSize);
}