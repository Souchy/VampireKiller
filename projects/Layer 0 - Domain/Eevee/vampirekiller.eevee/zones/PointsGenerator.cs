using Godot;
using souchy.celebi.eevee.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.zones;

namespace vampirekiller.eevee.zones;

public static class PointsGenerator
{
    public static Vector3 From()
    {
        return new Vector3();
    }
    public static Vector3 From(float x, float z)
    {
        return new Vector3(x, 0, z);
    }
    public static Vector3 From(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    public static Points point(IZone zone)
    {
        return new(zone)
            {
                From()
            };
    }
    public static Points pointRing(IZone zone) => point(zone);

    #region lines
    public static Points line(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeLength>();
        for (int i = 0; i < size.length; i++)
            points.Add(From(0, i));
        return points;
    }
    public static Points diagonal(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeLength>();
        for (int i = 0; i < size.length; i++)
            points.Add(From(i, i));
        return points;
    }
    #endregion

    #region crosses
    public static Points cross(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusForward; i <= size.radiusForward; i++)
            points.Add(From(0, i));
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            points.Add(From(i, 0));
        return points;
    }
    public static Points crossHalf(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusForward; i <= size.radiusForward; i++)
            points.Add(From(0, i));
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            points.Add(From(i, 0));
        return points;
    }
    public static Points crossRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusForward; i <= size.radiusForward; i++)
            points.Add(From(0, i));
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            points.Add(From(i, 0));
        return points;
    }
    public static Points crossHalfRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusForward; i <= size.radiusForward; i++)
            points.Add(From(0, i));
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            points.Add(From(i, 0));
        return points;
    }

    public static Points xcross(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusForward; i <= size.radiusForward; i++)
            points.Add(From(i, i));
        for (int j = -size.radiusSide; j <= size.radiusSide; j++)
            points.Add(From(j, -j));
        return points;
    }
    public static Points xcrossRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusForward; i <= size.radiusForward; i++)
            points.Add(From(i, i));
        for (int j = -size.radiusSide; j <= size.radiusSide; j++)
            points.Add(From(j, -j));
        return points;
    }

    public static Points star(IZone zone)
    {
        var points = cross(zone).Add(xcross(zone));
        return points;
    }
    #endregion

    #region circles
    public static Points circle(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius>();
        for (int i = -size.radius; i <= size.radius; i++)
            for (int j = -size.radius; j <= size.radius; j++)
                if (Math.Abs(i) + Math.Abs(j) <= size.radius)
                    points.Add(From(i, j));
        return points;
    }
    public static Points circleHalf(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius>();
        for (int i = -size.radius; i <= size.radius; i++)
            for (int j = 0; j <= size.radius; j++)
                if (Math.Abs(i) + Math.Abs(j) <= size.radius)
                    points.Add(From(i, j));
        return points;
    }
    public static Points circleRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius>();
        for (int i = -size.radius; i <= size.radius; i++)
            for (int j = -size.radius; j <= size.radius; j++)
            {
                var r = Math.Abs(i) + Math.Abs(j);
                if (r <= size.radius && r > size.radius - size.ringWidth)
                    points.Add(From(i, j));
            }
        return points;
    }
    public static Points circleHalfRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius>();
        for (int i = -size.radius; i <= size.radius; i++)
            for (int j = 0; j <= size.radius; j++)
            {
                var r = Math.Abs(i) + Math.Abs(j);
                if (r <= size.radius && r > size.radius - size.ringWidth)
                    points.Add(From(i, j));
            }
        return points;
    }
    #endregion

    #region squares
    public static Points square(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius>();
        for (int i = -size.radius; i <= size.radius; i++)
            for (int j = -size.radius; j <= size.radius; j++)
                points.Add(From(i, j));
        return points;
    }
    public static Points squareHalf(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius>();
        for (int i = -size.radius; i <= size.radius; i++)
            for (int j = 0; j <= size.radius; j++)
                points.Add(From(i, j));
        return points;
    }
    public static Points squareRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius>();
        for (int i = -size.radius; i <= size.radius; i++)
            for (int j = -size.radius; j <= size.radius; j++)
            {
                var r = Math.Max(Math.Abs(i), Math.Abs(j));
                if (r <= size.radius && r > size.radius - size.ringWidth)
                    points.Add(From(i, j));
            }
        return points;
    }
    public static Points squareHalfRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius>();
        for (int i = -size.radius; i <= size.radius; i++)
            for (int j = 0; j <= size.radius; j++)
            {
                var r = Math.Max(Math.Abs(i), Math.Abs(j));
                if (r <= size.radius && r > size.radius - size.ringWidth)
                    points.Add(From(i, j));
            }
        return points;
    }
    #endregion

    #region rectangles
    public static Points rectangle(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            for (int j = -size.radiusForward; j <= size.radiusForward; j++)
                points.Add(From(i, j));
        return points;
    }
    public static Points rectangleRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            for (int j = -size.radiusForward; j <= size.radiusForward; j++)
            {
                var side = (Math.Abs(i) <= size.radiusSide && Math.Abs(i) > size.radiusSide - size.ringWidth);
                var forward = (Math.Abs(j) <= size.radiusForward && Math.Abs(j) > size.radiusForward - size.ringWidth);
                if (side || forward)
                    points.Add(From(i, j));
            }
        return points;
    }
    public static Points rectangleHalfRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            for (int j = 0; j <= size.radiusForward; j++)
            {
                var ai = Math.Abs(i);
                var aj = Math.Abs(j);
                var side = (ai <= size.radiusSide && ai > size.radiusSide - size.ringWidth);
                var forward = (aj <= size.radiusForward && aj > size.radiusForward - size.ringWidth);
                if (side || forward)
                    points.Add(From(i, j));
            }
        return points;
    }
    #endregion

    #region ellipses
    public static Points ellipse(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        int maxRadius = Math.Max(size.radiusSide, size.radiusForward);
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            for (int j = -size.radiusForward; j <= size.radiusForward; j++)
            {
                var r = Math.Abs(i) + Math.Abs(j);
                if (r <= maxRadius)
                    points.Add(From(i, j));
            }
        return points;
    }
    public static Points ellipseHalf(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        int maxRadius = Math.Max(size.radiusSide, size.radiusForward);
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            for (int j = 0; j <= size.radiusForward; j++)
            {
                var r = Math.Abs(i) + Math.Abs(j);
                if (r <= maxRadius)
                    points.Add(From(i, j));
            }
        return points;
    }
    public static Points ellipseRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        int maxRadius = Math.Max(size.radiusSide, size.radiusForward);
        int minRadius = Math.Min(size.radiusSide, size.radiusForward);
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            for (int j = -size.radiusForward; j <= size.radiusForward; j++)
            {
                var ai = Math.Abs(i);
                var aj = Math.Abs(j);
                var r = ai + aj;
                if (r > maxRadius)
                    continue;
                if (r > maxRadius - size.ringWidth)
                    points.Add(From(i, j));
                if (minRadius == size.radiusSide && ai > size.radiusSide - size.ringWidth)
                    points.Add(From(i, j));
                if (minRadius == size.radiusForward && aj > size.radiusForward - size.ringWidth)
                    points.Add(From(i, j));
            }
        return points;
    }
    public static Points ellipseHalfRing(IZone zone)
    {
        Points points = new(zone);
        var size = zone.GetSize<ZoneSizeRadius2>();
        int maxRadius = Math.Max(size.radiusSide, size.radiusForward);
        int minRadius = Math.Min(size.radiusSide, size.radiusForward);
        for (int i = -size.radiusSide; i <= size.radiusSide; i++)
            for (int j = 0; j <= size.radiusForward; j++)
            {
                var ai = Math.Abs(i);
                var aj = Math.Abs(j);
                var r = ai + aj;
                if (r > maxRadius)
                    continue;
                if (r > maxRadius - size.ringWidth)
                    points.Add(From(i, j));
                if (minRadius == size.radiusSide && ai > size.radiusSide - size.ringWidth)
                    points.Add(From(i, j));
                if (minRadius == size.radiusForward && aj > size.radiusForward - size.ringWidth)
                    points.Add(From(i, j));
            }
        return points;
    }
    #endregion
}
