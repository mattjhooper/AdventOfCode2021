// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("--- Day 16: Packet Decoder ---");

string input = System.IO.File.ReadAllText(@"Input.txt");

var pp = new PacketProcessor(input);

pp.ProcessHeader();

Console.WriteLine($"Packets Process: {pp.PacketCount}. Total version count: {pp.TotalVersionCount}");


public class Packet
{
    private readonly string _binary;
    private int _pos;
    private readonly long _version;
    private readonly long _type;
    private bool _hasSubPackets;
    private long _lengthOfBits;
    private long _numberOfPackets;
    private long _value;

    private List<Packet> _subpackets;

    public Packet(string packetString)
    {
        _binary = packetString;
        _pos = 0;
        _version = GetVersion();
        _type = _binary.Substring(_pos, 3).BinaryToNumber();
        _pos += 3;
        _subpackets = new();

        TotalVersion = (int)_version;

        PackageCount = 1;

        if (IsOperator)
        {
            ProcessOperator();
        }
        else
        {
            ProcessLiteral();
        }

        Console.WriteLine(ToString());
    }

    public long Version => _version;

    public long Value => _value;

    public int TotalVersion { get; private set; }

    public bool IsOperator => _type != 4;

    public bool SubPacketsType => _hasSubPackets;

    public int PackageCount { get; private set; }

    public int Length => _pos;

    public override string ToString()
    {
        string typeDesc = IsOperator ? "Operator" : "Literal";
        string packetSummary = IsOperator ? SubPacketsType ? $"Sub-packets: {_numberOfPackets}" : $"Bit Length: {_lengthOfBits}" : "";
        return $"{typeDesc} packet. Type: {_type}. {packetSummary}. Value: {_value}";
    }

    private void ProcessOperator()
    {
        _hasSubPackets = _binary[_pos] == '1';
        _pos++;

        if (_hasSubPackets)
        {
            ProcessSubPackets();
        }
        else
        {
            ProcessBits();
        }
    }

    private long GetVersion()
    {
        long version = _binary.Substring(_pos, 3).BinaryToNumber();

        _pos += 3;
        return version;
    }

    private void ProcessSubPackets()
    {
        _numberOfPackets = _binary.Substring(_pos, 11).BinaryToNumber();
        _pos += 11;

        for (int i = 0; i < _numberOfPackets; i++)
        {
            var subPacket = new Packet(_binary.Substring(_pos));

            TotalVersion += subPacket.TotalVersion;

            PackageCount += subPacket.PackageCount;

            _pos += subPacket.Length;

            _subpackets.Add(subPacket);
        }

        _value = _subpackets.Operate(_type);
    }

    private void ProcessBits()
    {
        _lengthOfBits = _binary.Substring(_pos, 15).BinaryToNumber();
        _pos += 15;
        long endOfSubPackets = _pos + _lengthOfBits;

        while (_pos < endOfSubPackets)
        {
            var subPacket = new Packet(_binary.Substring(_pos,(int) endOfSubPackets - _pos));

            TotalVersion += subPacket.TotalVersion;

            PackageCount += subPacket.PackageCount;

            _pos += subPacket.Length;

            _subpackets.Add(subPacket);
        }

        _value = _subpackets.Operate(_type);
    }

    private void ProcessLiteral()
    {
        bool keepLooking = true;

        string binaryNumberAsString = string.Empty;

        while (keepLooking)
        {
            string group = _binary.Substring(_pos, 5);

            _pos += group.Length;

            binaryNumberAsString += group.Substring(1, 4);

            keepLooking = group[0] == '1' && _pos + 4 < _binary.Length;

        }

        /*
        while (_pos < _binary.Length && _binary[_pos] == '0')
        {
            _pos++;
        }
        */

        _value = binaryNumberAsString.BinaryToNumber();

        //Console.WriteLine($"Position: {_pos} Packet: {PacketCount}. Binary Number. {binaryNumberAsString.BinaryToNumber()}");
    }

}



public class PacketProcessor
{
    private readonly string _input;
    private readonly string _binary;
    
    public PacketProcessor(string input)
    {
        _input = input;
        _binary = ConvertHexToBinary(input);
    }

    public int PacketCount { get; private set; }

    public long TotalVersionCount { get; private set; }

    public bool ProcessHeader()
    {
        var packet = new Packet(_binary);

        PacketCount = packet.PackageCount;
        TotalVersionCount = packet.TotalVersion;

        return true;
    }

    

    private string ConvertHexToBinary(string input)
    {
        var lookup = GetHexToBinaryMap();

        StringBuilder s = new();

        foreach (char c in input)
        {
            s.Append(lookup[c]);
        }
        Console.WriteLine(s.ToString());
        return s.ToString();
    }

    private Dictionary<char, string> GetHexToBinaryMap()
    {
        Dictionary<char, string> map = new();

        map['0'] = "0000";
        map['1'] = "0001";
        map['2'] = "0010";
        map['3'] = "0011";
        map['4'] = "0100";
        map['5'] = "0101";
        map['6'] = "0110";
        map['7'] = "0111";
        map['8'] = "1000";
        map['9'] = "1001";
        map['A'] = "1010";
        map['B'] = "1011";
        map['C'] = "1100";
        map['D'] = "1101";
        map['E'] = "1110";
        map['F'] = "1111";


        return map;
    }

}

public static class ExtensionUtils
{
    public static long BinaryToNumber(this string binaryString)
    {
        /*
        if (binaryString.Length >= "1111111111111111111111111111111".Length)
            return int.MaxValue;*/
        
        return Convert.ToInt64(binaryString, 2);
    }

    public static long Operate(this List<Packet> packets, long type) => type switch
    {
        0 => packets.Sum(p => p.Value),
        1 => packets.Aggregate((long)1, (acc, p) => acc * p.Value),
        2 => packets.Min(p => p.Value),
        3 => packets.Max(p => p.Value),
        5 => packets[0].Value > packets[1].Value ? 1 : 0,
        6 => packets[0].Value < packets[1].Value ? 1 : 0,
        7 => packets[0].Value == packets[1].Value ? 1 : 0,
        _ => throw new ArgumentOutOfRangeException(nameof(type), $"Not expected type value: {type}"),
    };        
}

