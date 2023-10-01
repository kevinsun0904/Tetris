using System;

static class RandomExtensions
{
    /// <summary>
    /// Shuffles the array
    /// </summary>
    /// <typeparam name="T">Type of array</typeparam>
    /// <param name="rng">Random object</param>
    /// <param name="array">Array to shuffle</param>
    public static void Shuffle<T> (this Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}

