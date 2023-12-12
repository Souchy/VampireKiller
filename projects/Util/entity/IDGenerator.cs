using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.entity;

public interface IDGenerator
{
    public static IDGenerator Instance { get; }
    public ID Generate();
}