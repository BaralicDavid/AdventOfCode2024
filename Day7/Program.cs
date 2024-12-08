// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

var lines = File.ReadAllLines("input.txt");
var testValues = new BigInteger[lines.Length];
var listArray = new List<BigInteger>[lines.Length];

for (var i = 0; i < lines.Length; i++)
{
    testValues[i] = BigInteger.Parse(lines[i].Split(":")[0]);
    listArray[i] = lines[i].Split(":")[1].Trim().Split(" ")
        .Select(BigInteger.Parse)
        .ToList();
}


bool CanBeCombinedPart1(BigInteger testValue, List<BigInteger> numbers)
{
   // for operations + and *
   var calculatedValue = numbers.First();
   var skipFirst = true;
   var numberOfOperations = numbers.Count - 1;

   // get all of the permutations via binary values
   for (var i = 0; i < Math.Pow(2, numberOfOperations); i++)
   {
       var permutation = Convert.ToString(i, 2).PadLeft(numberOfOperations, '0');
       var charIdx = 0;
       foreach (var number in numbers)
       {
           if (skipFirst)
           {
               skipFirst = false;
               continue;
           }
           if (permutation[charIdx] == '0')
               calculatedValue += number;
           else
               calculatedValue *= number;
           if (calculatedValue > testValue)
               break;
           charIdx++;
       }

       if (calculatedValue == testValue)
       {
           var operations = permutation.Replace('0', '+').Replace('1', '*');
           Console.WriteLine("Permutation is "  + operations);
           return true;
       }

       calculatedValue = numbers.First();
       skipFirst = true;
   }

   return false;
}

string ConvertToBase3(int number)
{
    if (number == 0)
        return "0";

    string result = string.Empty;
    while (number > 0)
    {
        result = (number % 3) + result;
        number /= 3;
    }
    return result;
}

bool CanBeCombinedPart2(BigInteger testValue, List<BigInteger> numbers)
{
    // for operations + and *
    var calculatedValue = BigInteger.Zero;
    var skipFirst = true;
    var numberOfOperations = numbers.Count - 1;

    // get all of the permutations via ternary values
    for (var i = 0; i < Math.Pow(3, numberOfOperations); i++)
    {
        var permutation = ConvertToBase3(i).PadLeft(numberOfOperations, '0');
        var charIdx = 0;
        foreach (var number in numbers)
        {
            if (skipFirst)
            {
                calculatedValue = numbers.First();
                skipFirst = false;
                continue;
            }
            if (permutation[charIdx] == '0')
                calculatedValue += number;
            else if (permutation[charIdx] == '1')
                calculatedValue *= number;
            else
                calculatedValue = BigInteger.Parse(calculatedValue.ToString() + number);
            if (calculatedValue > testValue)
                break;
            charIdx++;
        }
        
        if (calculatedValue == testValue)
        {
            var operations = permutation
                .Replace('0', '+')
                .Replace('1', '*')
                .Replace("2","||");
            Console.WriteLine("Permutation is "  + operations);
            return true;
        }
        
        skipFirst = true;
    }

    return false;
}


var sum = new BigInteger(0);
for (var i = 0; i < lines.Length; i++)
{
    Console.WriteLine(testValues[i]);
    // if (CanBeCombinedPart1(testValues[i], listArray[i]))
    if (CanBeCombinedPart2(testValues[i], listArray[i]))
    {
        Console.WriteLine("Test value " + testValues[i]);
        sum += testValues[i];
    }
}

Console.WriteLine("Total sum is " + sum);


