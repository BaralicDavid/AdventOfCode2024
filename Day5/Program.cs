// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = File.ReadAllLines("input.txt");
var allowedValuesBefore = new Dictionary<int, HashSet<int>>();
var orderings = new List<List<int>>();

var lineIdx = 0;
for(;lineIdx<lines.Length; lineIdx++)
{
    var line = lines[lineIdx];
    if (line == "")
    {
        lineIdx++;
        break;
    }
    var leftVal = int.Parse(line.Split("|")[0]);
    var rightVal = int.Parse(line.Split("|")[1]);
    if (!allowedValuesBefore.ContainsKey(rightVal))
        allowedValuesBefore[rightVal] = new HashSet<int>();
    allowedValuesBefore[rightVal].Add(leftVal);
}

for(;lineIdx<lines.Length; lineIdx++)
{
    var line = lines[lineIdx];
    orderings.Add(line.Split(",").Select(int.Parse).ToList());
}


#region Part1
void part1()
{
    var totalSum = 0;
    foreach (var orderList in orderings)
    {
        var isInOrder = true; 
        for (var i = 0; i < orderList.Count; i++)
        {
            for (var j = i + 1; j < orderList.Count; j++)
            {
                if (allowedValuesBefore.ContainsKey(orderList[i])
                    && allowedValuesBefore[orderList[i]].Contains(orderList[j]))
                {
                    isInOrder = false;
                    goto Exit;
                }
            }
        }
        Exit: ;
        if (isInOrder)
        {
            totalSum += orderList[orderList.Count / 2];
        }
    }
    Console.WriteLine("Total sum :" +  totalSum);
}
#endregion

#region Part2
void part2()
{
    var totalSum = 0;
    foreach (var orderList in orderings)
    {
        var isInOrder = true; 
        for (var i = 0; i < orderList.Count; i++)
        {
            for (var j = i + 1; j < orderList.Count; j++)
            {
                if (allowedValuesBefore.ContainsKey(orderList[i])
                    && allowedValuesBefore[orderList[i]].Contains(orderList[j]))
                {
                    isInOrder = false;
                    // swapping via deconstruction
                    (orderList[i], orderList[j]) = (orderList[j], orderList[i]);
                }
            }
        }
        if (!isInOrder)
        {
            totalSum += orderList[orderList.Count / 2];
        }
    }
    Console.WriteLine("Total sum of incorrectly ordered :" +  totalSum);
}
#endregion
// part1();
part2();






