using Godot;

namespace vampirekiller.eevee.zones;

public static class PointsGenerator
{

    public static Points point()
    {
        return new() { new Voxel() };
    }
    public static Points pointRing() => point();

    #region lines
    public static Points line(int length)
    {
        Points points = new();
        for (int i = 0; i < length; i++)
            points.Add(new Voxel(0, i));
        return points;
    }
    public static Points diagonal(int length)
    {
        Points points = new();
        for (int i = 0; i < length; i++)
            points.Add(new Voxel(i, i));
        return points;
    }
    #endregion

    #region crosses
    public static Points cross(int radiusForward, int radiusSide)
    {
        Points points = new();
        for (int i = -radiusForward; i <= radiusForward; i++)
            points.Add(new Voxel(0, i));
        for (int i = -radiusSide; i <= radiusSide; i++)
            points.Add(new Voxel(i, 0));
        return points;
    }
    public static Points crossHalf(int radiusSide, int radiusForward)
    {
        Points points = new();
        
        for (int i = -radiusForward; i <= radiusForward; i++)
            points.Add(new Voxel(0, i));
        for (int i = -radiusSide; i <= radiusSide; i++)
            points.Add(new Voxel(i, 0));
        return points;
    }
    public static Points crossRing(int radiusSide, int radiusForward)
    {
        Points points = new();
        
        for (int i = -radiusForward; i <= radiusForward; i++)
            points.Add(new Voxel(0, i));
        for (int i = -radiusSide; i <= radiusSide; i++)
            points.Add(new Voxel(i, 0));
        return points;
    }
    public static Points crossHalfRing(int radiusSide, int radiusForward)
    {
        Points points = new();
        
        for (int i = -radiusForward; i <= radiusForward; i++)
            points.Add(new Voxel(0, i));
        for (int i = -radiusSide; i <= radiusSide; i++)
            points.Add(new Voxel(i, 0));
        return points;
    }

    public static Points xcross(int radiusSide, int radiusForward)
    {
        Points points = new();
        
        for (int i = -radiusForward; i <= radiusForward; i++)
            points.Add(new Voxel(i, i));
        for (int j = -radiusSide; j <= radiusSide; j++)
            points.Add(new Voxel(j, -j));
        return points;
    }
    public static Points xcrossRing(int radiusSide, int radiusForward)
    {
        Points points = new();
        
        for (int i = -radiusForward; i <= radiusForward; i++)
            points.Add(new Voxel(i, i));
        for (int j = -radiusSide; j <= radiusSide; j++)
            points.Add(new Voxel(j, -j));
        return points;
    }

    public static Points star(int radiusSide, int radiusForward)
    {
        var points = cross(radiusSide, radiusForward).Add(xcross(radiusSide, radiusForward));
        return points;
    }
    #endregion

    #region circles
    public static Points circle(int radius)
    {
        Points points = new();
        
        for (int i = -radius; i <= radius; i++)
            for (int j = -radius; j <= radius; j++)
                if (Math.Abs(i) + Math.Abs(j) <= radius)
                    points.Add(new Voxel(i, j));
        return points;
    }
    public static Points circleHalf(int radius)
    {
        Points points = new();
        
        for (int i = -radius; i <= radius; i++)
            for (int j = 0; j <= radius; j++)
                if (Math.Abs(i) + Math.Abs(j) <= radius)
                    points.Add(new Voxel(i, j));
        return points;
    }
    public static Points circleRing(int radius, int ringWidth)
    {
        Points points = new();
        
        for (int i = -radius; i <= radius; i++)
            for (int j = -radius; j <= radius; j++)
            {
                var r = Math.Abs(i) + Math.Abs(j);
                if (r <= radius && r > radius - ringWidth)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    public static Points circleHalfRing(int radius, int ringWidth)
    {
        Points points = new();
        
        for (int i = -radius; i <= radius; i++)
            for (int j = 0; j <= radius; j++)
            {
                var r = Math.Abs(i) + Math.Abs(j);
                if (r <= radius && r > radius - ringWidth)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    #endregion

    #region squares
    public static Points square(int radius)
    {
        Points points = new();
        
        for (int i = -radius; i <= radius; i++)
            for (int j = -radius; j <= radius; j++)
                points.Add(new Voxel(i, j));
        return points;
    }
    public static Points squareHalf(int radius)
    {
        Points points = new();
        
        for (int i = -radius; i <= radius; i++)
            for (int j = 0; j <= radius; j++)
                points.Add(new Voxel(i, j));
        return points;
    }
    public static Points squareRing(int radius, int ringWidth)
    {
        Points points = new();
        
        for (int i = -radius; i <= radius; i++)
            for (int j = -radius; j <= radius; j++)
            {
                var r = Math.Max(Math.Abs(i), Math.Abs(j));
                if (r <= radius && r > radius - ringWidth)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    public static Points squareHalfRing(int radius, int ringWidth)
    {
        Points points = new();
        
        for (int i = -radius; i <= radius; i++)
            for (int j = 0; j <= radius; j++)
            {
                var r = Math.Max(Math.Abs(i), Math.Abs(j));
                if (r <= radius && r > radius - ringWidth)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    #endregion

    #region rectangles
    public static Points rectangle(int radiusSide, int radiusForward)
    {
        Points points = new();
        
        for (int i = -radiusSide; i <= radiusSide; i++)
            for (int j = -radiusForward; j <= radiusForward; j++)
                points.Add(new Voxel(i, j));
        return points;
    }
    public static Points rectangleRing(int radiusSide, int radiusForward, int ringWidth)
    {
        Points points = new();
        for (int i = -radiusSide; i <= radiusSide; i++)
            for (int j = -radiusForward; j <= radiusForward; j++)
            {
                var side = (Math.Abs(i) <= radiusSide && Math.Abs(i) > radiusSide - ringWidth);
                var forward = (Math.Abs(j) <= radiusForward && Math.Abs(j) > radiusForward - ringWidth);
                if (side || forward)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    public static Points rectangleHalfRing(int radiusSide, int radiusForward, int ringWidth)
    {
        Points points = new();
        for (int i = -radiusSide; i <= radiusSide; i++)
            for (int j = 0; j <= radiusForward; j++)
            {
                var ai = Math.Abs(i);
                var aj = Math.Abs(j);
                var side = (ai <= radiusSide && ai > radiusSide - ringWidth);
                var forward = (aj <= radiusForward && aj > radiusForward - ringWidth);
                if (side || forward)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    #endregion

    #region ellipses
    public static Points ellipse(int radiusSide, int radiusForward)
    {
        Points points = new();
        int maxRadius = Math.Max(radiusSide, radiusForward);
        for (int i = -radiusSide; i <= radiusSide; i++)
            for (int j = -radiusForward; j <= radiusForward; j++)
            {
                var r = Math.Abs(i) + Math.Abs(j);
                if (r <= maxRadius)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    public static Points ellipseHalf(int radiusSide, int radiusForward)
    {
        Points points = new();
        int maxRadius = Math.Max(radiusSide, radiusForward);
        for (int i = -radiusSide; i <= radiusSide; i++)
            for (int j = 0; j <= radiusForward; j++)
            {
                var r = Math.Abs(i) + Math.Abs(j);
                if (r <= maxRadius)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    public static Points ellipseRing(int radiusSide, int radiusForward, int ringWidth)
    {
        Points points = new();
        int maxRadius = Math.Max(radiusSide, radiusForward);
        int minRadius = Math.Min(radiusSide, radiusForward);
        for (int i = -radiusSide; i <= radiusSide; i++)
            for (int j = -radiusForward; j <= radiusForward; j++)
            {
                var ai = Math.Abs(i);
                var aj = Math.Abs(j);
                var r = ai + aj;
                if (r > maxRadius)
                    continue;
                if (r > maxRadius - ringWidth)
                    points.Add(new Voxel(i, j));
                if (minRadius == radiusSide && ai > radiusSide - ringWidth)
                    points.Add(new Voxel(i, j));
                if (minRadius == radiusForward && aj > radiusForward - ringWidth)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    public static Points ellipseHalfRing(int radiusSide, int radiusForward, int ringWidth)
    {
        Points points = new();
        
        int maxRadius = Math.Max(radiusSide, radiusForward);
        int minRadius = Math.Min(radiusSide, radiusForward);
        for (int i = -radiusSide; i <= radiusSide; i++)
            for (int j = 0; j <= radiusForward; j++)
            {
                var ai = Math.Abs(i);
                var aj = Math.Abs(j);
                var r = ai + aj;
                if (r > maxRadius)
                    continue;
                if (r > maxRadius - ringWidth)
                    points.Add(new Voxel(i, j));
                if (minRadius == radiusSide && ai > radiusSide - ringWidth)
                    points.Add(new Voxel(i, j));
                if (minRadius == radiusForward && aj > radiusForward - ringWidth)
                    points.Add(new Voxel(i, j));
            }
        return points;
    }
    #endregion
}
