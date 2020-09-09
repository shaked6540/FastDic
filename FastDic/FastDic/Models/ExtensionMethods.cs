using System;
using System.Collections.Generic;
using System.Text;

namespace FastDic.Models
{
    public static class ExtensionMethods
    {
        public static int BinarySearchBy<T, K>(this IList<K> inputArray, T key, Func<K, T> func, IComparer<T> comparer)
        {
            int min = 0;
            int max = inputArray.Count - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                if (comparer.Compare(key,func(inputArray[mid])) == 0)
                {
                    return mid;
                }
                else if (comparer.Compare(key, func(inputArray[mid])) < 0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }
            return -1;
        }
        public static string UpperCaseFirstLetter(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToLower().ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }


}
