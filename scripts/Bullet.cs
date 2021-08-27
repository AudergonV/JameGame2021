using Godot;
using System;

public class Bullet : Area2D
{
    private bool launched = false;
    private Vector2 direction;
    [Export]
    public int bulletType;
    [Export]
    public Material material;
    [Export]
    public PackedScene Particles;


    public void fire(Vector2 pos, Vector2 direction, int bulletType){
        Position = pos;
        this.direction = direction;
        this.bulletType = bulletType;
        launched = true;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (launched) Position += direction*1000*delta;
    }

    public void OnBulletBodyEntered(Node2D body){
        if (body is TileMap){
            TileMap tmap = (TileMap)body;
            Vector2 pos = tmap.WorldToMap(Position);
            int id = tmap.GetCellv(pos);

            if (id >= 0 && id < 30 && (id-id%6)/6 != bulletType) { 
                tmap.SetCellv(pos, bulletType*6+id%6);
                Particles2D particles = (Particles2D)Particles.Instance();
                if (direction.y < 0) particles.Rotation = (float)Math.PI;
                particles.SetProcessMaterial(material);
                particles.GlobalPosition = Position; 
                GetParent().AddChild(particles);
            }
            QueueFree();
        }
    }
}
