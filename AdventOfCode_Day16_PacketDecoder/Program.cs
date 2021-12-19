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
    private readonly int _version;
    private readonly int _type;
    private bool _hasSubPackets;
    private int _lengthOfBits;
    private int _numberOfPackets;
    private int _literalValue;

    public Packet(string packetString)
    {
        _binary = packetString;
        _pos = 0;
        _version = GetVersion();
        _type = _binary.Substring(_pos, 3).BinaryToNumber();
        _pos += 3;

        TotalVersion = _version;

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

    public int Version => _version;

    public int TotalVersion { get; private set; }

    public bool IsOperator => _type != 4;

    public bool SubPacketsType => _hasSubPackets;

    public int PackageCount { get; private set; }

    public int Length => _pos;

    public override string ToString()
    {
        string typeDesc = IsOperator ? "Operator" : "Literal";
        string packetSummary = IsOperator ? SubPacketsType ? $"Sub-packets: {_numberOfPackets}" : $"Bit Length: {_lengthOfBits}" : $"Number: {_literalValue}";
        return $"{typeDesc} packet. Version: {_version}. {packetSummary}";
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

    private int GetVersion()
    {
        int version = _binary.Substring(_pos, 3).BinaryToNumber();

        /*
        if (version == 0)
        {
            while (_pos < _binary.Length && _binary[_pos] == '0')
            {
                _pos++;
            }

            version = _binary.Substring(_pos, 3).BinaryToNumber();
        }
        */

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
        }
    }

    private void ProcessBits()
    {
        _lengthOfBits = _binary.Substring(_pos, 15).BinaryToNumber();
        _pos += 15;
        int endOfSubPackets = _pos + _lengthOfBits;


        while (_pos < endOfSubPackets)
        {
            var subPacket = new Packet(_binary.Substring(_pos, endOfSubPackets - _pos));

            TotalVersion += subPacket.TotalVersion;

            PackageCount += subPacket.PackageCount;

            _pos += subPacket.Length;
        }
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

        _literalValue = binaryNumberAsString.BinaryToNumber();

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
    public static int BinaryToNumber(this string binaryString)
    {
        if (binaryString.Length >= "1111111111111111111111111111111".Length)
            return int.MaxValue;
        
        return Convert.ToInt32(binaryString, 2);
    }
}