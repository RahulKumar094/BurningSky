using System;
using System.Collections.Generic;

public class GenericPool<T> : IPool<T> where T : class
{
    Func<T> factoryMethod;
    Func<T, bool> alive; 
    int maxCount;
    public List<T> items { get; private set; }

    public GenericPool(Func<T> factoryMethod, int maxCount, Func<T, bool> alive)
    {
        this.maxCount = maxCount;
        this.factoryMethod = factoryMethod;
        this.alive = alive;
        items = new List<T>(maxCount);
    }

    public T GetInstance()
    {
        int count = items.Count;

        foreach (T item in items)
        {
            if (!alive(item))
                return item;
        }

        if (count > maxCount)
        {
            return null;
        }

        T newItem = factoryMethod();
        items.Add(newItem);
        return newItem;
    }
}

public interface IPool<T>
{
    public T GetInstance();
}
