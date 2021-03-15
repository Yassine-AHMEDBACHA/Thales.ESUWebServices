using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.DashbordWS.Extensions
{
    public static class StringExtensions
    {
        internal static bool IsValid(this string value)
        {
            return !value.IsNotValid();
        }

        internal static bool IsNotValid(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}
