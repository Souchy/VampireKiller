using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util;

public static class Util
{

    public static HashSet<int> PickUniques(Random rnd, List<int> list, int min, int max)
    {
        // Make keys distinct
        HashSet<int> keys = list.ToHashSet();
        // Pick a count to return
        int randomCount = rnd.Next(min, max);
        randomCount = Math.Clamp(randomCount, 0, list.Count);
        // Remove random keys until we get the correct count
        while (keys.Count > randomCount)
        {
            var ra = rnd.Next(keys.Count);
            keys.Remove(ra);
        }
        return keys;
    }

}
