using System.Reflection;

namespace Day14;

public class Robot
{
    public (int, int) Position;
    public (int, int) Velocity;

    public Robot((int, int) position, (int, int) velocity)
    {
        Position = position;
        Velocity = velocity;
    }
}