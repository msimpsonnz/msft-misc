using System;

namespace Functions.MemTest.Common
{
    public class AllocateMemory
    {
        public static void GetEmptyByteArray()
        {
            byte[] array = new byte[10240 * 10240];
            Array.Clear(array, 0, array.Length);
        }
    }
}
