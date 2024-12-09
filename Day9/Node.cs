using System.Collections.Generic;

namespace Day9;

public class Node
{
    public int BlockSize;
    public int CurrentBlockIdx = 0;
    public List<int> Content { get; } = new List<int>();

    public Node(int blockSize)
    {
        BlockSize = blockSize;
    }

    public void AddAll(int id)
    {
        for(; CurrentBlockIdx<BlockSize; CurrentBlockIdx++)
            Content.Add(id);
    }

    public void AddContent(int value)
    {
        Content.Add(value);
        CurrentBlockIdx++;
    }

    public void RemoveLast()
    {
        Content.RemoveAt(Content.Count-1);
        CurrentBlockIdx--;
    }
};