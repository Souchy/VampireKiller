using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Util.entity;

//public interface ID
//{
//    public ID fromString(string str);
//    public string toString();
//}

public struct ID
{
    public ID() { }
    public ID(string value) => this.value = value;
    public string value { get; set; }

    public static implicit operator string(ID id) => id.value;
    public static implicit operator ID(string value) => new(value);
}
