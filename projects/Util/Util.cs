using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util;

public static class Util
{

    public static HashSet<int> PickUniques(Random rnd, List<int> list, int count) //int min, int max)
    {
        // Make keys distinct
        List<int> keys = list; //.ToList();
        // Pick a count to return
        //int count = rnd.Next(min, max);
        count = Math.Clamp(count, 0, list.Count);
        // Remove random keys until we get the correct count
        while (keys.Count > count)
        {
            var ra = rnd.Next(keys.Count);
            keys.RemoveAt(ra);
        }
        return keys.ToHashSet();
    }

}
