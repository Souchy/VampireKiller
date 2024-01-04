using Godot;
using System;
using System.Drawing;
using System.Linq;
using vampirekiller.eevee.campaign;

public partial class DebugLevelGraph : Node2D
{
	private static int VERTICE_RADIUS = 10;
	private static int POSITION_COEFF = 50;

	private LevelInstance.LevelGraph graph;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		this.init();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if (@event.IsActionPressed("lock_camera"))
		{
			this.init();
		}
    }

	private void init() {
		this.GetChildren().Where(i => i is not Camera2D).ToList().ForEach(i => i.QueueFree());
		this.graph = LevelInstance.LevelGraph.generateGraph(5, 2, 5, 5);
        this.graph.edges.ForEach(createEdge);
        this.graph.vertices.ForEach(createVertice);
    }

    private void createVertice(Point p) {
		Polygon2D poly = new Polygon2D();
		poly.Polygon = new Vector2[] {
			new Vector2(VERTICE_RADIUS, VERTICE_RADIUS),
			new Vector2(-VERTICE_RADIUS, VERTICE_RADIUS),
			new Vector2(-VERTICE_RADIUS, -VERTICE_RADIUS),
			new Vector2(VERTICE_RADIUS, -VERTICE_RADIUS)
        };
		poly.Color = p.Y == 0
			? new Godot.Color(0, 255, 0)
			: new Godot.Color(255, 0, 0);
		poly.Offset = new Vector2(p.X * POSITION_COEFF, -p.Y * POSITION_COEFF);
		this.AddChild(poly);
	}

	private void createEdge(Tuple<Point, Point> path) {
		Line2D line = new Line2D();
        line.Points = new Vector2[] { 
			new Vector2(path.Item1.X * POSITION_COEFF, -path.Item1.Y * POSITION_COEFF),
			new Vector2(path.Item2.X * POSITION_COEFF, -path.Item2.Y * POSITION_COEFF)
		};
		line.Width = 5;
		this.AddChild(line);
	}
}
