using System.Collections.Generic;

public class PriorityQueue
{
    private List<(EnemyStats, float)> elements = new List<(EnemyStats, float)>();
    private bool descending;

    public PriorityQueue(bool descending)
    {
        this.descending = descending;
    }

    public void Enqueue(EnemyStats item, float priority)
    {
        elements.Add((item, priority));
        elements.Sort((x, y) => descending ? y.Item2.CompareTo(x.Item2) : x.Item2.CompareTo(y.Item2));
    }

    public EnemyStats Dequeue()
    {
        var item = elements[0];
        elements.RemoveAt(0);
        return item.Item1;
    }

    public (EnemyStats, float) First()
    {
        return elements[0];
    }

    public int Count => elements.Count;
}