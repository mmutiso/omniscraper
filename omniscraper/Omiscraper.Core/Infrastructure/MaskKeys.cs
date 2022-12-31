using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Infrastructure
{
    internal static class MaskKeys
    {
        public static string PartialMask(this string key)
        {
            int length = key.Length;
            if (length == 0)
                return key;

            if (length < 6)
                return "******";

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                if (i < 6)
                    stringBuilder.Append(key[i]);
                else
                    stringBuilder.Append("*");
            }

            return stringBuilder.ToString();
        }
    }
}
