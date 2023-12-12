using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;

namespace Util.entity;

public interface IEntity : IDisposable
{
    public ID entityUid { get; set; }
}
