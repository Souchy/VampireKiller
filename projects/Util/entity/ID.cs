using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.entity;

public interface ID
{
    public ID fromString(string str);
    public string toString();
}
