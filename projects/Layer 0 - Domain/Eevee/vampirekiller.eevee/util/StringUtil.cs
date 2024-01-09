using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.eevee.util;


public static class StringUtil
{

    public static string trimPathExt(this string path)
    {
        var data = path.Replace("\\", "/").Split("/");
        var last = data[data.Length - 1];
        var dot = last.LastIndexOf(".");
        if(dot == -1)
            return last;
        return last.Substring(0, dot);
    }

}
