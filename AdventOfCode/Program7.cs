using System.Text.RegularExpressions;

var reader = File.OpenText(@"..\..\..\..\Inputs\input7.txt");

DirItem root = new(null, "");
DirItem currentDir = root;

Dictionary<string, DirItem> dirs = new();
dirs.Add(root.name, root);

Regex cdRegex = new Regex(@"^\$ cd (.+)$");
Regex dirRegex = new Regex(@"^dir (.+)$");
Regex fileRegex = new Regex(@"^([0-9]+) (.+)$");

List<TreeItem>? items = null;
while (!reader.EndOfStream)
{
    string input = reader.ReadLine()!;
    if (items is not null)
    {
        if (input.StartsWith('$'))
        {
            currentDir.children = items;
            items = null;
        }
    }

    var cdMatch = cdRegex.Match(input);
    if (cdMatch.Success)
    {
        string dirName = cdMatch.Groups[1].Value;
        if (dirName == "..")
        {
            if (currentDir.parent is null) throw new Exception();
            currentDir = currentDir.parent;
        }
        else if (dirName == "/")
        {
            currentDir = root;
        }
        else
        {
            currentDir = dirs[currentDir.fullName + "/" + dirName];
        }
        continue;
    }
    if (input == "$ ls")
    {
        items = new();
        continue;
    }

    if (items is null) throw new Exception("$ command expected");
    var dirMatch = dirRegex.Match(input);
    if (dirMatch.Success)
    {
        DirItem newDir = new(currentDir, dirMatch.Groups[1].Value);
        dirs.Add(newDir.fullName, newDir);
        items.Add(newDir);
        continue;
    }
    var fileMatch = fileRegex.Match(input);
    if (fileMatch.Success)
    {
        FileItem newFile = new(currentDir, fileMatch.Groups[2].Value, int.Parse(fileMatch.Groups[1].Value));
        items.Add(newFile);
        continue;
    }
    throw new Exception("unrecognised line");
}
if (items is not null)
{
    currentDir.children = items;
    items = null;
}

Console.WriteLine("Total sizes of dirs of size at most 100 000 is");
Console.WriteLine(
    dirs.Values.Select(dir => dir.Size).Where(size => size <= 100000).Sum()
    );

int unusedSpace = 70000000 - root.Size;
Console.WriteLine($"Unused space is {unusedSpace}");
int toDelete = 30000000 - unusedSpace;
Console.WriteLine($"Need to delete {toDelete}");

DirItem smallestDir = root;
foreach (var dir in dirs.Values)
{
    if (dir.Size >= toDelete && dir.Size < smallestDir.Size)
    {
        smallestDir = dir;
    }
}
Console.WriteLine($"Smallest dir is {smallestDir.fullName}, it has size {smallestDir.Size}");

abstract class TreeItem
{
    public readonly DirItem? parent;
    public string name;

    public string fullName;
    abstract public int Size { get; }
    protected TreeItem(DirItem? parent, string name)
    {
        this.parent = parent;
        this.name = name;
        if (parent is null) fullName = name;
        else fullName = parent.fullName + "/" + name;
    }
    public override string ToString()
    {
        return $"{name}, {Size}";
    }
}

class FileItem : TreeItem
{
    private readonly int size;
    public FileItem(DirItem? parent, string name, int size) : base(parent, name)
    {
        this.size = size;
    }
    public override int Size => size;
}

class DirItem: TreeItem
{
    public List<TreeItem>? children;
    private int size = -1;
    public DirItem(DirItem? parent, string name) : base(parent, name) { }
    public override int Size
    {
        get
        {
            if (size == -1)
            {
                size = children!.Select(x => x.Size).Sum();
            }
            return size;
        }
    }
}