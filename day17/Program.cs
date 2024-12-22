// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices.ComTypes;

var lines = File.ReadAllLines("input.txt");

var programStr = lines[4].Split(":")[1].Trim().Split(",");

void part1()
{
    var computer = new Computer(
        registerA: int.Parse(lines[0].Split(":")[1].Trim()),
        registerB: int.Parse(lines[1].Split(":")[1].Trim()),
        registerC: int.Parse(lines[2].Split(":")[1].Trim()));
    var output =  computer.RunProgram(programStr);
    foreach(var e in output)
        Console.Write(e + ",");
    Console.WriteLine();
}

void part2()
{
    var computer = new Computer(
        registerA: int.Parse(lines[0].Split(":")[1].Trim()),
        registerB: int.Parse(lines[1].Split(":")[1].Trim()),
        registerC: int.Parse(lines[2].Split(":")[1].Trim()));
    var initialOutputStr = computer.RunProgram(programStr)
        .Aggregate("", (acc, num) => acc + num + ",");
    
    var candidateMax = int.MaxValue;
    foreach (var candidateValue in Enumerable.Range(51342984, 100000 ))
    {
        computer.RegisterA = candidateValue;
        computer.RegisterB = int.Parse(lines[1].Split(":")[1].Trim());
        computer.RegisterC = int.Parse(lines[2].Split(":")[1].Trim());

        var output = computer.RunProgram(programStr)
            .Aggregate("", (acc, num) => acc + num + ",");
        if (output == initialOutputStr 
            && candidateValue != int.Parse(lines[0].Split(":")[1].Trim()))
        {
            Console.WriteLine($"CandidateA is {candidateValue}");
            break;
        }
        if(candidateValue % 1000000 == 0)
            Console.WriteLine($"{candidateValue}. Candidate processed");
    }
    
    Console.WriteLine();
}

// part1();
part2();

class Computer(int registerA, int registerB, int registerC)
{
    public int RegisterA
    {
        get => registerA;
        set => registerA = value;
    }

    public int RegisterB
    {
        get => registerB;
        set => registerB = value;
    }

    public int RegisterC
    {
        get => registerC;
        set => registerC = value;
    }

    int GetComboValue(int combo)
    {
        if (combo <= 3)
            return combo;
        if (combo == 4)
            return registerA;
        if (combo == 5)
            return registerB;
        if (combo == 6)
            return registerC;
        if (combo == 7)
            throw new Exception("Combo = 7");
        return -1;
    }
    
    public List<int> RunProgram(string[] programStr)
    {
        int instructionPtr = 0;
        var output = new List<int>();
            
        while (instructionPtr < programStr.Length)
        {
            var opcode = int.Parse(programStr[instructionPtr]);
            var operand = int.Parse(programStr[instructionPtr + 1]);
            
            switch ((OpCode)opcode)
            {
                case OpCode.Adv:
                    registerA /= (int)Math.Pow(2, GetComboValue(operand));
                    break;
                case OpCode.Bxl:
                    registerB ^= operand;
                    break;
                case OpCode.Bst:
                    registerB = GetComboValue(operand) % 8;
                    break;
                case OpCode.Jnz:
                    if (registerA != 0)
                    {
                        // check if this should be lowered
                        instructionPtr = operand;
                        continue;
                    }
                    break;
                case OpCode.Bxc:
                    registerB ^= registerC;
                    break;
                case OpCode.Out:
                    output.Add(GetComboValue(operand) % 8);
                    break;
                case OpCode.Bdv: 
                    registerB = registerA / (int)Math.Pow(2, GetComboValue(operand));
                    break;
                case OpCode.Cdv: 
                    registerC = registerA / (int)Math.Pow(2, GetComboValue(operand));
                    break;
            }

            instructionPtr += 2;
        }
        return output;
    }
    
}

enum OpCode
{
    Adv = 0,
    Bxl,
    Bst,
    Jnz,
    Bxc,
    Out,
    Bdv,
    Cdv
}
