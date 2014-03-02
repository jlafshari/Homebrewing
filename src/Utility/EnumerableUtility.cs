using System.Collections.Generic;
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
    }
}
