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

    public static T SafelySetScript<T>(this GodotObject obj, Resource resource) where T : GodotObject
    {
        var godotObjectId = obj.GetInstanceId();
        // Replaces old C# instance with a new one. Old C# instance is disposed.
        obj.SetScript(resource);
        // Get the new C# instance
        return GodotObject.InstanceFromId(godotObjectId) as T;
    }

    public static T SafelySetScript<T>(this GodotObject obj, string resource) where T : GodotObject
    {
        return SafelySetScript<T>(obj, ResourceLoader.Load(resource));
    }


    public static (CreatureNode?, Vector3?) getRayCast(this Camera3D PlayerCamera)
    {
        //var rayLength = 100;
        var floorHeight = 0;
        var mousePos = PlayerCamera.GetViewport().GetMousePosition();
        var origin = PlayerCamera.ProjectRayOrigin(mousePos);
        var direction = PlayerCamera.ProjectRayNormal(mousePos);

        //var to = origin + direction * rayLength;
        var distance = (floorHeight - origin.Y) / direction.Y;
        Vector3 floorPos = direction * distance + origin;

        var space = PlayerCamera.GetWorld3D().DirectSpaceState;
        var ray = new PhysicsRayQueryParameters3D()
        {
            From = origin,
            To = floorPos //to,
            //CollideWithAreas = false,
            //CollideWithBodies = false,
        };
        var result = space.IntersectRay(ray);
        CreatureNode? raycastEntity = null;
        //Vector3? raycastPosition = null;
        if (result.ContainsKey("collider"))
        {
            // Different colliders: ground (StaticBody3D), creature (CreatureNode).
            // Theorical: poe corpses (spectres), totems (prob a creature), maybe some bullshit doors or destructible decor  
            Node3D collider = (Node3D) result["collider"];
            if (collider is CreatureNode crea)
                raycastEntity = crea;
        }
        //if (result.ContainsKey("position"))
        //{
        //    Vector3 pos = (Vector3) result["position"];
        //    pos.Y = 0;
        //    raycastPosition = pos;
        //}
        EventBus.centralBus.publish(nameof(UiSapphire.onRaycast), raycastEntity, floorPos);
        return (raycastEntity, floorPos);
    }

}
