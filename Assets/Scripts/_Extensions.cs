using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

namespace ExtensionMethods
{

    public static class _Extensions 
    {
        public static int bit(this int i)
        {
            return i == 0 ? 0 : 1;
        }

        public static int bitNeg(this int i)
        {
            return i == 0 ? 0 : 1;
        }
    }
}