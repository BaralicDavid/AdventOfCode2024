namespace Day16;

public class VertexPriority((int, int) vertex, long dist) : IComparable<VertexPriority>
{
    public (int, int) Vertex { get; } = vertex;
    public long Distance { get; } = dist;

    public int CompareTo(VertexPriority? other)
    {
        if(Distance < other.Distance)
        {
            return -1;
        }

        if(Distance > other.Distance)
        {
            return 1;
        }

        return 0;
    }
}