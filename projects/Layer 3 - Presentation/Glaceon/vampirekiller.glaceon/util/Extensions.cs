using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;

namespace vampirekiller.glaceon.util;

public static class Extensions
{

    public static (CreatureNode?, Vector3?) getRayCast(this Camera3D PlayerCamera)
    {
        var mousePos = PlayerCamera.GetViewport().GetMousePosition();
        var rayLength = 100;
        var from = PlayerCamera.ProjectRayOrigin(mousePos);
        var to = from + PlayerCamera.ProjectRayNormal(mousePos) * rayLength;
        var space = PlayerCamera.GetWorld3D().DirectSpaceState;
        var ray = new PhysicsRayQueryParameters3D()
        {
            From = from,
            To = to,
            CollideWithAreas = true
        };
        var result = space.IntersectRay(ray);
        CreatureNode? raycastEntity = null;
        Vector3? raycastPosition = null;
        if (result.ContainsKey("collider"))
        {
            // Different colliders: ground (StaticBody3D), creature (CreatureNode).
            // Theorical: poe corpses (spectres), totems (prob a creature), maybe some bullshit doors or destructible decor  
            Node3D collider = (Node3D) result["collider"];
            if (collider is CreatureNode crea)
                raycastEntity = crea;
        }
        if (result.ContainsKey("position"))
        {
            Vector3 pos = (Vector3) result["position"];
            pos.Y = 0;
            raycastPosition = pos;
        }
        EventBus.centralBus.publish(nameof(UiSapphire.onRaycast), raycastEntity, raycastPosition);
        return (raycastEntity, raycastPosition);
    }

}
