using FvpWebAppModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FvpWebAppWorker.Infrastructure
{
    public class FvpWebAppUtils
    {
        public static List<List<T>> DividedRows<T>(List<T> data, int maxSalesRows)
        {
            List<List<T>> dividedList = new List<List<T>>();
            int divideCount = (int)Math.Ceiling((double)data.Count / (double)maxSalesRows);
            int index = 0;
            var rows = new List<T>();
            for (int i = 1; i <= divideCount; i++)
            {
                if (i < divideCount)
                    rows = data.GetRange(index, maxSalesRows);
                else
                    rows = data.GetRange(index, (data.Count - (maxSalesRows * (i - 1))));
                dividedList.Add(rows);
                index = (maxSalesRows * i);
            }
            return dividedList;
        }

        public static bool CheckUeCountry(List<Country> countries, string countryCode)
        {
            return countries.Select(c => c.Symbol).Contains(countryCode) ? true : false;
        }

        public static string GetDigitsFromString(string stringWithDigist)
        {
            return new string(stringWithDigist.Where(Char.IsDigit).ToArray());
        }

        public static string TruncateToLength(string stringToCut, int length)
        {
            if (stringToCut.Length > length)
                return stringToCut.Substring(0, length);
            else
                return stringToCut;
        }
    }
}
