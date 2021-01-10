using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Helpers
{
    public static class IntHelper
    {
        public static int Bound(this int val, int minPossible, int maxPossible)
        {
            if (val > maxPossible) return maxPossible;
            if (val < minPossible) return minPossible;
            return val;
        }

        public static int BoundMin(this int val, int minPossible)
        {
            if (val < minPossible) return minPossible;
            return val;
        }

        public static int BoundMax(this int val, int maxPossible)
        {
            if (val > maxPossible) return maxPossible;
            return val;
        }
    }
}
