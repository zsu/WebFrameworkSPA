using System.Collections.Generic;
using System.Diagnostics;
using System;
namespace App.Common
{


    public static class CollectionExtension
    {
        /// <summary>
        /// ForEach extension that enumerates over all items in an <see cref="IEnumerable{T}"/> and executes 
        /// an action.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="collection">The enumerable instance that this extension operates on.</param>
        /// <param name="action">The action executed for each iten in the enumerable.</param>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
        }

        /// <summary>
        /// ForEach extension that enumerates over all items in an <see cref="IEnumerator{T}"/> and executes 
        /// an action.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="collection">The enumerator instance that this extension operates on.</param>
        /// <param name="action">The action executed for each iten in the enumerable.</param>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerator<T> collection, Action<T> action)
        {
            while (collection.MoveNext())
                action(collection.Current);
        }
        /// <summary>
        /// For Each extension that enumerates over a enumerable collection and attempts to execute 
        /// the provided action delegate and it the action throws an exception, continues enumerating.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="collection">The IEnumerable instance that ths extension operates on.</param>
        /// <param name="action">The action excecuted for each item in the enumerable.</param>
        [DebuggerStepThrough]
        public static void TryForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                try
                {
                    action(item);
                }
                catch { }
            }
        }

        /// <summary>
        /// For each extension that enumerates over an enumerator and attempts to execute the provided
        /// action delegate and if the action throws an exception, continues executing.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="enumerator">The IEnumerator instace</param>
        /// <param name="action">The action executed for each item in the enumerator.</param>
        [DebuggerStepThrough]
        public static void TryForEach<T>(this IEnumerator<T> enumerator, Action<T> action)
        {
            while (enumerator.MoveNext())
            {
                try
                {
                    action(enumerator.Current);
                }
                catch { }
            }
        }
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return (collection == null) || (collection.Count == 0);
        }
    }
}