using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.ecs;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.util;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.zones;

namespace vampirekiller.logia.extensions;

public static class ZoneExtensions
{
    public static Vector3? getZoneOrigin(this IZone zone, IAction action)
    {
        return zone.worldOrigin switch
        {
            ZoneOriginType.Source => action.getSourceEntity()?.get<PositionGetter>()?.Invoke(),
            ZoneOriginType.Target => action.getRaycastEntity()?.get<PositionGetter>()?.Invoke(),
            ZoneOriginType.Raycast => action.raycastPosition,
            _ => throw new NotImplementedException()
        };
    }

    public static Entity? getTargetActor(this IZone zone, IAction action)
    {
        return zone.worldOrigin switch
        {
            ZoneOriginType.Source => action.getSourceEntity(),
            ZoneOriginType.Target => action.getRaycastEntity(),
            ZoneOriginType.Raycast => null
        };
    }


}
