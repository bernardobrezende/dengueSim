using System;

namespace dengueSim.Domain
{
    static class RandomExtensions
    {
        public static Random rand;
        public static Random GetInstance()
        {
            if (rand == null) rand = new Random();

            return rand;
        }
    }
}
