// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using Day9;

var lines = File.ReadAllLines("input.txt");
var diskNodes = new List<Node>();

var id = 0;
for (var i = 0; i < lines[0].Length; i++, id++)
{
    var node = new Node(int.Parse(lines[0][i].ToString()));
    node.AddAll(id);
    diskNodes.Add(node);
    
    i++;
    if( i == lines[0].Length)
        break;
    
    node = new Node(int.Parse(lines[0][i].ToString()));
    diskNodes.Add(node);
}

#region Part1

void part1()
{
    int firstEmptyPos = 1;
    for (var j = diskNodes.Count - 1; j >=0 && j>firstEmptyPos; j-- )
    {
        var node = diskNodes[j];
        if (node.Content.Count == 0)
            continue;
        
        var emptyNode = diskNodes[firstEmptyPos];
        for (var k = node.Content.Count - 1; k >= 0; k--)
        {
            if (emptyNode.Content.Count == emptyNode.BlockSize)
            {
                firstEmptyPos += 2;
                if(firstEmptyPos >= diskNodes.Count)
                    break;
                emptyNode = diskNodes[firstEmptyPos];
            }
            emptyNode.AddContent(node.Content[k]);
            node.RemoveLast();
        }
    }

    long checksum = 0;
    var id = 0;
    foreach (var node in diskNodes)
    {
        foreach (var value in node.Content)
        {
            checksum += id * value;
            Console.Write($" {id}*{value},");
            id++;   
        }
    }
    Console.WriteLine();
    Console.WriteLine("CheckSum is " + checksum);
}
#endregion

foreach (var node in diskNodes)
{
    Console.Write($" {node.Content.Count}:{node.BlockSize}--");
}

// part1();