using System.Text.RegularExpressions;

var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input16.txt");

Regex regex = new(@"^Valve (..) has flow rate=([0-9]+); tunnels? leads? to valves? (.*)$");

Dictionary<string, string[]> adjacencies = new();
List<string> workingValves = new();
List<int> valveCapacities = new();
Dictionary<string, int> valveIndex = new();

foreach (var input in inputs)
{
    var match = regex.Match(input);
    string valveName = match.Groups[1].Value;
    int valveCapacity = int.Parse(match.Groups[2].Value);
    string[] adjacent = match.Groups[3].Value.Split(", ");

    if (valveCapacity > 0)
    {
        workingValves.Add(valveName);
        valveCapacities.Add(valveCapacity);
    }
    adjacencies.Add(valveName, adjacent);
}
for (int i = 0; i < workingValves.Count; i++)
    valveIndex.Add(workingValves[i], i);

int totalTime = 26;

Dictionary<State, Dictionary<uint, int>>[] states = new Dictionary<State, Dictionary<uint, int>>[totalTime + 1];
for (int i = 0; i < totalTime + 1; i++)
    states[i] = new();
states[0].Add(new State
{ 
    CurrentValve = "AA", 
    ElephantValve = "AA"
}, new() { { 0, 0 } });

for (int i = 0; i < totalTime; i++)
{
    foreach ((State state, var valveStates) in states[i])
    {
        foreach ((uint releasedValves, int releasedPressure) in valveStates)
        {
            int newReleasedPressure = releasedPressure;
            for (int j = 0; j < valveCapacities.Count; j++)
            {
                if ((releasedValves & (1u << j)) != 0)
                    newReleasedPressure += valveCapacities[j];
            }

            List<(State, uint)> newStates = new();
            foreach ((State halfNewState, uint halfReleasedValues) in GetNewStates(state, releasedValves))
                newStates.AddRange(GetElephantNewStates(halfNewState, halfReleasedValues));

            foreach ((State newState, uint newValves) in newStates)
            {
                if (!states[i + 1].TryGetValue(newState, out var valveFlags))
                {
                    states[i + 1].Add(newState,
                        new() { { newValves, newReleasedPressure } });
                }
                else
                {
                    bool dominated = false;
                    foreach ((uint listedValves, int listedPressure) in valveFlags)
                    {
                        if ((listedValves & newValves) == listedValves &&
                            listedPressure <= newReleasedPressure)
                            valveFlags.Remove(listedValves);
                        else if ((listedValves & newValves) == newValves && 
                            listedPressure >= newReleasedPressure)
                            dominated = true;
                    }
                    if (!dominated)
                    {
                        valveFlags[newValves] = newReleasedPressure;
                    }
                }
            }
        }
    }
}

Console.WriteLine(states[totalTime].Values.Select(dict => dict.Values.Max()).Max());



IEnumerable<(State, uint)> GetNewStates(State state, uint ReleasedValves)
{
    // try open
    if (valveIndex.TryGetValue(state.CurrentValve, out int index) &&
        (ReleasedValves & (1u << index)) == 0)
    {
        yield return (new State
        {
            CurrentValve = state.CurrentValve,
            ElephantValve = state.ElephantValve
        },
        ReleasedValves | (1u << index));
    }
    // try move
    foreach (string nextValve in adjacencies[state.CurrentValve])
    {
        yield return (new State
        {
            CurrentValve = nextValve,
            ElephantValve = state.ElephantValve
        },
        ReleasedValves);
    }
}

IEnumerable<(State, uint)> GetElephantNewStates(State state, uint ReleasedValves)
{
    // try open
    if (valveIndex.TryGetValue(state.ElephantValve, out int index) &&
        (ReleasedValves & (1u << index)) == 0)
    {
        yield return (new State
        {
            CurrentValve = state.CurrentValve,
            ElephantValve = state.ElephantValve,
        },
        ReleasedValves | (1u << index));
    }
    // try move
    foreach (string nextValve in adjacencies[state.ElephantValve])
    {
        yield return (new State
        {
            CurrentValve = state.CurrentValve,
            ElephantValve = nextValve,
        },
        ReleasedValves);
    }
}

record struct State
{
    public string CurrentValve;
    public string ElephantValve;
    public override string ToString()
    {
        return $"{CurrentValve} {ElephantValve}";
    }
}