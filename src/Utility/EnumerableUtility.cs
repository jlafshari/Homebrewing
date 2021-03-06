﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Utility
{
    /// <summary>
    /// Provides extension methods for IEnumerable objects.
    /// </summary>
    public static class EnumerableUtility
    {
        public static ReadOnlyObservableCollection<T> ToReadOnlyObservableCollection<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
                return null;

            return sequence as ReadOnlyObservableCollection<T> ?? new ReadOnlyObservableCollection<T>(new ObservableCollection<T>(sequence));
        }

        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
                return null;

            return sequence as ReadOnlyCollection<T> ?? new ReadOnlyCollection<T>(sequence as List<T> ?? new List<T>(sequence));
        }
    }
}
