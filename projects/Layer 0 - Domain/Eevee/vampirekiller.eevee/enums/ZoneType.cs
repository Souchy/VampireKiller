using souchy.celebi.eevee.face.shared.zones;
using souchy.celebi.eevee.face.util.math;
using souchy.celebi.eevee.impl.objects.zones;
using System.Reflection;
using static souchy.celebi.eevee.impl.objects.zones.AreaGenerator;

namespace souchy.celebi.eevee.enums
{

    /// <summary>
    /// TODO: will need to transform this into a class with static constants to avoid [Attributes] on the enum
    /// </summary>
    public enum ZoneType
    {
        [ZoneSize<ZoneSizePoint>] point,
        //multi, // multi zones in one

        [ZoneSize<ZoneSizeLength>] line,
        [ZoneSize<ZoneSizeLength>] diagonal, // diag = line rotated 45°. base is aligned with character, can use 90° rotation to make it perpendicular diagonal

        [ZoneSize<ZoneSizeRadius2>] cross, // '+' made of 2 lines orthogonal
        [ZoneSize<ZoneSizeRadius2>] xcross, // 'x' made of 2 diagonals
        [ZoneSize<ZoneSizeRadius2>] star, // both crosses (=8 directions)

        [ZoneSize<ZoneSizeRadius2>] crossHalf,
        [ZoneSize<ZoneSizeRadius2>] xcrossHalf,

        [ZoneSize<ZoneSizeRadius>] circle = 8,      // like a diamond
        [ZoneSize<ZoneSizeRadius>] circleHalf,  // cone, like a triangle
        [ZoneSize<ZoneSizeRadius>] square,      // orthogonal square
        [ZoneSize<ZoneSizeRadius>] squareHalf,  // orthogonal half square makes a bit of a rectangle
        [ZoneSize<ZoneSizeRadius2>] rectangle,
        [ZoneSize<ZoneSizeRadius2>] ellipse,     // ellipse : radius, radius
        [ZoneSize<ZoneSizeRadius2>] ellipseHalf,

        // everyone on the board
        board,
        // everyone off the board (creatures available in their party)
        offboard,
        // everyone on & off the board
        all
    }

    public abstract class IZoneSizeAttribute : Attribute
    {
        public abstract Type type();
        public IZoneSize create(IZone zone)
        {
            var t = (IZoneSize) Activator.CreateInstance(type());
            t.sizeParams = zone.size.value;
            return t;
        }
    }
    public class ZoneSizeAttribute<T> : IZoneSizeAttribute where T : IZoneSize
    {
        public override Type type() => typeof(T); 
        public T createT(IZone zone) {
            var t = (T) Activator.CreateInstance(typeof(T));
            t.sizeParams = zone.size.value;
            return t;
        }
    }
    public static class ZoneTypeExtensions
    {
        public static T GetSize<T>(this IZone zone) where T : IZoneSize
        {
            var attr = (ZoneSizeAttribute<T>) typeof(ZoneType)
                    .GetField(Enum.GetName(zone.zoneType.value))
                    .GetCustomAttribute(typeof(ZoneSizeAttribute<T>), true);
            return attr.createT(zone);
        }
        public static IZoneSize GetSize(this IZone zone)
        {
            var attr = (IZoneSizeAttribute) typeof(ZoneType)
                    .GetField(Enum.GetName(zone.zoneType.value))
                    .GetCustomAttribute(typeof(IZoneSizeAttribute), true);
            return attr.create(zone);
        }
        public static Points GeneratePoints(this IZone zone)
        {
            var methodName = Enum.GetName(zone.zoneType.value) + (zone.GetRingWidth() != 0 ? "Ring" : "");
            var points = (Points) typeof(AreaGenerator)
                .GetMethod(methodName)
                .Invoke(null, new object[] { zone });
            return points
                .anchor()
                .rotate()
                .offset();
        }
    }

    public abstract class IZoneSize
    {
        public IVector3 sizeParams;
        public int ringWidth { get => sizeParams.y; set => sizeParams.y = value; }
    }
    public class ZoneSizePoint : IZoneSize
    {
    }
    // line, diagonal
    public class ZoneSizeLength : IZoneSize
    {
        public int length { get => sizeParams.x; set => sizeParams.x = value; }
    }
    // circle O, square [], halfcircle /_\, v  /\
    public class ZoneSizeRadius : IZoneSize
    {
        public int radius { get => sizeParams.x; set => sizeParams.x = value; }
    }
    // cross, xcross, star, rectangle, ellipse, ellipseHalf
    public class ZoneSizeRadius2 : IZoneSize
    {
        public int radiusForward { get => sizeParams.x; set => sizeParams.x = value; }
        public int radiusSide { get => sizeParams.z; set => sizeParams.z = value; }
    }


}