using System;
using System.Collections.Generic;
using System.Text;
using Debug = System.Diagnostics.Debug;

public class OddmentTable<T> {
    // Allows comparison of generic types.
    private static readonly IEqualityComparer<T> comparer =
        EqualityComparer<T>.Default;
    protected List<Entry> entries = new List<Entry>();
    protected int oddment = 0;
    protected Random random;

    /// <summary>
    /// Each item must be associated with it's oddment
    /// in the list. This simple structure does that
    /// for us.
    /// </summary>
    protected struct Entry {
        public T Item;
        public int Oddment;

        public Entry(T item, int oddment)
        {
            this.Oddment = oddment;
            this.Item = item;
        }
    }

    /// <summary>
    /// Create an Oddment table using a precreated
    /// Random object, that will be used to decide the picks.
    /// Useful for repeatable results.
    /// </summary>
    /// <param name="rand">The random object to use</param>
    public OddmentTable(Random rand)
    {
        random = rand;
    }

    /// <summary>
    /// Default constructor will seed a random
    /// number generator with the current ticktime.
    /// </summary>
    public OddmentTable()
    {
        random = new Random(System.Environment.TickCount);
    }

    /// <summary>
    /// Add an Item and it's Oddment (chance of being picked)
    /// </summary>
    /// <param name="Item">Whatever the Oddment table was intialized to.</param>
    /// <param name="Oddment">The Oddment is the likely hood of getting it.
    /// It should be a non-zero positive number</param>
    public void Add(T item, int oddment)
    {
        Debug.Assert(oddment > 0,
            "The oddment number passed in was" + oddment +
            "\r\nIt should be a positive non-zero integer");

        entries.Add(new Entry(item, oddment));
        this.oddment += oddment;
    }

    /// <summary>
    /// Randomly pick an Item from the table, where
    /// the items are weighted by their oddments.
    /// </summary>
    /// <returns>The picked Item</returns>
    public T Pick()
    {
        int pickNo = random.Next(oddment);

        /// I want to go from 1 to n, not 0 to n - 1
        /// Odds of 0 shouldn't count for anything, in my mind anyway.
        pickNo++;

        // This will record the total of the oddments as
        // we traverse through the table.
        int oddmentTotal = 0;

        // Run through the table, total up each Oddment
        // if the Oddment total is bigger than the pick number
        // then return that Item
        foreach (Entry entry in entries) {
            oddmentTotal += entry.Oddment;

            if (oddmentTotal >= pickNo)
                return entry.Item;
        }

        return entries[entries.Count].Item;
    }

    /// <summary>
    /// Gets the number of elements contained in the Oddment table
    /// </summary>
    public int Count
    {
        get
        {
            return entries.Count;
        }
    }

    /// <summary>
    /// Remove the first instance of an item from the oddment table.
    /// </summary>
    /// <param name="item"></param>
    public void Remove(T item)
    {
        int i = 0;

        // Search through the enteries
        // until we get one with the same name.
        // There maybe a faster way to do this but I don't know how
        for (int j = 0; j < entries.Count; j++) {
            if (comparer.Equals(entries[j].Item, item)) {
                i = j;
            }
            else {
                if (i == j) {
                    //Remove nothing.
                    //Possibly better to throw exception.
                    return;
                }
            }
        }

        int oldOddment = entries[i].Oddment;
        entries.RemoveAt(i);
        oddment -= oldOddment;
    }

    public void Clear()
    {
        entries.Clear();
    }

}
