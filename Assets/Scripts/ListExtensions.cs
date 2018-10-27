using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class ListExtensions
    {
        #region List
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null) throw new ArgumentException("The list is null!");

            System.Random rng = new System.Random();
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T RandomItem<T>(this IList<T> list)
        {
            if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            if (list == null) throw new ArgumentException("The list is null!");

            List<T> newList = new List<T>();

            while (list.Count > 0)
            {
                T item = list[UnityEngine.Random.Range(0, list.Count)];
                list.Remove(item);

                newList.Add(item);
            }

            return newList;
        }

        public static bool Have<T>(this IList<T> list, T value)
        {
            if (list == null) throw new ArgumentException("The list is null!");

            foreach (var item in list)
            {
                if (item.Equals(value))
                    return true;
            }

            return false;
        }
        #endregion

        #region Array
        public static void Shuffle<T>(this T[] arr)
        {
            if (arr == null) throw new ArgumentException("The array is null!");

            System.Random _random = new System.Random();
            T tempObject;
            int n = arr.Length;

            for (int i = 0; i < n; i++)
            {

                int r = i + (int)(_random.NextDouble() * (n - i));
                tempObject = arr[r];
                arr[r] = arr[i];
                arr[i] = tempObject;
            }
        }

        public static bool Have<T>(this T[] arr, T value)
        {
            if (arr == null) throw new ArgumentException("The array is null!");

            foreach (var item in arr)
            {
                if (item.Equals(value))
                    return true;
            }

            return false;
        }
        #endregion
    }
}