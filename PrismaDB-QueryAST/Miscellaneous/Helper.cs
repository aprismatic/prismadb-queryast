using System;

namespace PrismaDB.QueryAST.Misc
{
    internal static class Helper
    {
        private static readonly uint[] _lookup32 = CreateLookup32();

        private static readonly char[] _alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static Random _rnd = new Random();
        private static long _uniqueStringCounter = 0;

        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (var i = 0; i < 256; i++)
            {
                var s = i.ToString("X2");
                result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
            }
            return result;
        }

        public static string ByteArrayToHex(byte[] bytes)
        {
            var lookup32 = _lookup32;
            var result = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

        /*public static string GetRandomString(int length)
        {
            var res = new char[length];

            for (var i = 0; i < res.Length; i++)
            {
                res[i] = _alpha[_rnd.Next(_alpha.Length)];
            }

            return new string(res);
        }*/

        /*public static string GetUniqueString()
        {
            var digits = new[] { _alpha[0], _alpha[0], _alpha[0],
                                 _alpha[0], _alpha[0], _alpha[0],
                                 _alpha[0], _alpha[0], _alpha[0],
                                 _alpha[0], _alpha[0], _alpha[0] }; // 12 characters
            var slotCount = digits.Length;

            var current = _uniqueStringCounter++;
            var count = 0;
            while (current > 0)
            {
                long rem;
                current = Math.DivRem(current, _alpha.Length, out rem);
                digits[slotCount - ++count] = _alpha[rem];
            }

            return new string(digits);
        }*/
    }
}
