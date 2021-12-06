using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_Day6_Lanternfish
{
    internal class LanternFish
    {
        public LanternFish(string timerStart)
        {
            Timer = int.Parse(timerStart);
        }

        public int Timer { get; private set; }

        public void CheckTimer(List<LanternFish> newFish)
        {
            if (Timer == 0)
            {
                newFish.Add(new LanternFish("8"));
                Timer = 6;
                return;
            }

            Timer--;
        }
    }
}
