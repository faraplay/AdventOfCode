using System.Text.RegularExpressions;

var inputs = File.ReadAllLines(@"..\..\..\..\Inputs\input19.txt");

int totalTime = 32;

int total = 0;
int prod = 1;
for (int i = 0; i < 3; i++)
{
    int best = bestGeodeCount(new(inputs[i]));
    Console.WriteLine(best);
    //total += i * best;
    prod *= best;
}
Console.WriteLine(prod);

int bestGeodeCount(Blueprint blueprint)
{
    Dictionary<State, int>[] geodeCounts = new Dictionary<State, int>[totalTime + 1];
    int maxOreBot = new int[]{blueprint.oreCost, blueprint.clayCost, blueprint.obsCostOre, blueprint.geoCostOre}.Max();
    int maxClayBot = blueprint.obsCostClay;
    int maxObsBot = blueprint.geoCostObs;

    geodeCounts[0] = new Dictionary<State, int>
    {
        { new State{oreBot = 1}, 0 }
    };
    for (int i = 0; i < totalTime; i++)
    {
        var newGeodeCounts = new Dictionary<State, int>();
        geodeCounts[i + 1] = newGeodeCounts;
        foreach ((State state, int geodeCount) in geodeCounts[i])
        {
            // don't keep track of ore/clay/obs counts if we have enough bots
            if (state.oreBot >= maxOreBot)
            {
                state.oreCount = Math.Min(state.oreCount, state.oreBot);
            }
            if (state.clayBot >= maxClayBot)
            {
                state.clayCount = Math.Min(state.clayCount, state.clayBot);
            }
            if (state.obsBot >= maxObsBot)
            {
                state.obsCount = Math.Min(state.obsCount, state.obsBot);
            }

            int newGeodeCount = geodeCount + state.geoBot;
            
            bool canMakeBot = false;
            if (state.oreBot < maxOreBot && state.oreCount >= blueprint.oreCost)
            {
                canMakeBot = true;
                State oreNewState = new(state);
                oreNewState.oreCount -= blueprint.oreCost;
                oreNewState.oreBot++;
                TryAdd(newGeodeCounts, oreNewState, newGeodeCount);
            }
            if (state.clayBot < maxClayBot && state.oreCount >= blueprint.clayCost)
            {
                canMakeBot = true;
                State clayNewState = new(state);
                clayNewState.oreCount -= blueprint.clayCost;
                clayNewState.clayBot++;
                TryAdd(newGeodeCounts, clayNewState, newGeodeCount);
            }
            if (state.obsBot < maxObsBot && state.oreCount >= blueprint.obsCostOre && state.clayCount >= blueprint.obsCostClay)
            {
                canMakeBot = true;
                State obsNewState = new(state);
                obsNewState.oreCount -= blueprint.obsCostOre;
                obsNewState.clayCount -= blueprint.obsCostClay;
                obsNewState.obsBot++;
                TryAdd(newGeodeCounts, obsNewState, newGeodeCount);
            }
            if (state.oreCount >= blueprint.geoCostOre && state.obsCount >= blueprint.geoCostObs)
            {
                canMakeBot = true;
                State geoNewState = new(state);
                geoNewState.oreCount -= blueprint.geoCostOre;
                geoNewState.obsCount -= blueprint.geoCostObs;
                geoNewState.geoBot++;
                TryAdd(newGeodeCounts, geoNewState, newGeodeCount);
            }
            if (true)
            {
                State newState = new(state);
                TryAdd(newGeodeCounts, newState, newGeodeCount);
            }
        }
        Console.WriteLine($"{i} {newGeodeCounts.Count}");
    }
    return geodeCounts[totalTime].Values.Max();
}

void TryAdd(Dictionary<State, int> newGeodeCounts, State newState, int newGeodeCount)
{
    //if (!(newGeodeCounts.TryGetValue(newState, out int listedCount) &&
    //                listedCount >= newGeodeCount))
    //{
    //    newGeodeCounts[newState] = newGeodeCount;
    //}
    bool dominated = false;
    foreach ((State listedState, int listedCount) in newGeodeCounts)
    {
        if (listedState <= newState && listedCount <= newGeodeCount)
        {
            newGeodeCounts.Remove(listedState);
        }
        else if (newState <= listedState && newGeodeCount <= listedCount)
        {
            dominated = true;
        }
    }
    if (!dominated)
    {
        newGeodeCounts.Add(newState, newGeodeCount);
    }
}

record State
{
    public int oreCount;
    public int clayCount;
    public int obsCount;
    // public int geoCount;
    public int oreBot;
    public int clayBot;
    public int obsBot;
    public int geoBot;

    public State() { }
    public State(State otherState)
    {
        this.oreCount = otherState.oreCount + otherState.oreBot;
        this.clayCount = otherState.clayCount + otherState.clayBot;
        this.obsCount = otherState.obsCount + otherState.obsBot;
        this.oreBot = otherState.oreBot;
        this.clayBot = otherState.clayBot;
        this.obsBot = otherState.obsBot;
        this.geoBot = otherState.geoBot;
    }

    public static bool operator <=(State state1, State state2)
    {
        return state1.oreCount <= state2.oreCount &&
            state1.clayCount <= state2.clayCount &&
            state1.obsCount <= state2.obsCount &&
            state1.oreBot <= state2.oreBot &&
            state1.clayBot <= state2.clayBot &&
            state1.obsBot <= state2.obsBot &&
            state1.geoBot <= state2.geoBot;
    }
    public static bool operator >=(State state1, State state2)
    {
        return state2 <= state1;
    }
}

record Blueprint
{
    public int oreCost;
    public int clayCost;
    public int obsCostOre;
    public int obsCostClay;
    public int geoCostOre;
    public int geoCostObs;

    static Regex regex = new Regex(@"^Blueprint [0-9]+: Each ore robot costs ([0-9]+) ore\. Each clay robot costs ([0-9]+) " +
    @"ore\. Each obsidian robot costs ([0-9]+) ore and ([0-9]+) clay\. Each geode robot costs ([0-9]+) ore and ([0-9]+) obsidian\.$");

    public Blueprint(string input)
    {
        var match = regex.Match(input);
        oreCost = int.Parse(match.Groups[1].Value);
        clayCost = int.Parse(match.Groups[2].Value);
        obsCostOre = int.Parse(match.Groups[3].Value);
        obsCostClay = int.Parse(match.Groups[4].Value);
        geoCostOre = int.Parse(match.Groups[5].Value);
        geoCostObs = int.Parse(match.Groups[6].Value);
    }
}