var stream = File.OpenText(@"..\..\..\..\Inputs\input13.txt");

int total = 0;
var divider1 = new ListData(new List<Data> { new ListData(new List<Data> { new IntData(2) }) });
var divider2 = new ListData(new List<Data> { new ListData(new List<Data> { new IntData(6) }) });

List<Data> datas = new List<Data>();
datas.Add(divider1);
datas.Add(divider2);

for (int i = 1; !stream.EndOfStream; i++)
{
    var data1 = Data.GetData(stream);
    stream.Read();
    var data2 = Data.GetData(stream);
    stream.Read();
    stream.Read();
    if (data1.CompareTo(data2) < 0)
    {
        Console.WriteLine(i);
        total += i;
    }
    datas.Add(data1);
    datas.Add(data2);
}
Console.WriteLine(total);

datas.Sort();
int index1 = datas.IndexOf(divider1) + 1;
int index2 = datas.IndexOf(divider2) + 1;
Console.WriteLine(index1);
Console.WriteLine(index2);
Console.WriteLine(index1 * index2);

abstract class Data : IComparable<Data>
{
    static public Data GetData(StreamReader stream)
    {
        char c = (char)stream.Read();
        if (c == '[')
        {
            List<Data> list = new();
            if (stream.Peek() == ']')
            {
                stream.Read();
                return new ListData(list);
            }
            while (true)
            {
                list.Add(GetData(stream));
                char c2 = (char)stream.Read();
                if (c2 == ']') break;
                if (c2 != ',') throw new Exception();
            }
            return new ListData(list);
        }
        else if (c >= '0' && c <= '9')
        {
            int value = c - '0';
            while (stream.Peek() >= '0' && stream.Peek() <= '9')
            {
                value *= 10;
                value += stream.Read() - '0';
            }
            return new IntData(value);
        }
        else throw new Exception("Error parsing data");
    }
    public abstract int CompareTo(Data? other);
}

class IntData : Data
{
    public int Value { get; set; }
    public IntData(int value)
    {
        Value = value;
    }
    public override string ToString()
    {
        return Value.ToString();
    }
    public override int CompareTo(Data? other)
    {
        switch (other)
        {
            case IntData intData:
                return Value.CompareTo(intData.Value);
            case ListData listData:
                var meListData = new ListData(new List<Data> { this });
                return meListData.CompareTo(listData);
            case null:
                return 1;
            default:
                throw new Exception();
        }
    }
}

class ListData : Data
{
    public List<Data> Values { get; set; }
    public ListData(List<Data> values)
    {
        Values = values;
    }
    public override string ToString()
    {
        return "[" + string.Join(',', Values.Select(d => d.ToString())) + "]";
    }
    public override int CompareTo(Data? other)
    {
        switch (other)
        {
            case ListData otherListData:
                for (int i = 0; i < Values.Count; i++)
                {
                    if (otherListData.Values.Count <= i) return 1;
                    int compared = Values[i].CompareTo(otherListData.Values[i]);
                    if (compared != 0) return compared;
                }
                if (otherListData.Values.Count == Values.Count) return 0;
                else return -1;
            case IntData otherIntData:
                var newOtherData = new ListData(new List<Data> { otherIntData });
                return CompareTo(newOtherData);
            case null:
                return 1;
            default:
                throw new Exception();
        }
    }
}