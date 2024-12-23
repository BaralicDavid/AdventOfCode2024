namespace Day23;


// Custom IEqualityComparer to compare HashSets by content
class HashSetComparer<T> : IEqualityComparer<HashSet<T>>
{
    public bool Equals(HashSet<T> x, HashSet<T> y)
    {
        // Check if the two hashsets have the same elements
        return x.SetEquals(y);
    }

    public int GetHashCode(HashSet<T> obj)
    {
        // Combine the hash codes of the elements in the set
        int hash = 0;
        foreach (T item in obj)
        {
            hash ^= item.GetHashCode();
        }
        return hash;
    }
}