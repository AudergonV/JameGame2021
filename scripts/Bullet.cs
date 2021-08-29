using Godot;
using System;

public class Bullet : KinematicBody2D
{
    private bool launched = false;
    private Vector2 direction;
    [Export]
    public int bulletType;
    [Export]
    public Material material;
    [Export]
    public PackedScene Particles;
    [Export]
    public int bulletSpeed = 50;

    private KinematicCollision2D collision; 

    public void fire(Vector2 pos, Vector2 direction, int bulletType){
        Position = pos;
        this.direction = direction;
        this.bulletType = bulletType;
        launched = true;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (launched) {
            MoveAndSlide(direction*bulletSpeed);   
        }
        if (GetSlideCount() > 0) {
            for (int i = 0; i < GetSlideCount(); i++) {
                KinematicCollision2D collision = GetSlideCollision(i);
                if (collision.Collider is TileMap) {
                    TileMap tmap = (TileMap) collision.Collider;
                    if (tmap.IsInGroup("paint_platforms")){
                        Vector2 pos = tmap.WorldToMap(Position);
                        int id = tmap.GetCellv(pos-collision.Normal);
                        GD.Print("HIT " + id);
                        if (id >= 0 && id < 30 && (id-id%6)/6 != bulletType) { 
                            tmap.SetCellv(pos-collision.Normal, bulletType*6+id%6);
                            Particles2D particles = (Particles2D)Particles.Instance();
                            if (direction.y < 0) particles.Rotation = (float)Math.PI;
                            particles.ProcessMaterial = material;
                            particles.GlobalPosition = Position; 
                            GetParent().AddChild(particles);
                        }
                    }
                }
            }
            QueueFree();
        }
    }

}
