using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;

namespace Util.entity;

public interface Identifiable : IDisposable
{
    public ID entityUid { get; set; }

    /// <summary>
    /// Hook called after registering the ID and EventBus in the Register
    /// </summary>
    public void initialize()
    {
    }
}
