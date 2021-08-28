using Godot;
using System;

public class Weapon : Sprite
{

    private PackedScene BlueBullet;    
    private PackedScene GreenBullet;
    private PackedScene PurpleBullet;
    private PackedScene RedBullet;
    private Timer timer;

    private int bulletType = 1;
    
    public override void _Ready()
    {
        BlueBullet = ResourceLoader.Load<PackedScene>("res://scenes/BlueBullet.tscn");
        GreenBullet = ResourceLoader.Load<PackedScene>("res://scenes/GreenBullet.tscn");
        PurpleBullet = ResourceLoader.Load<PackedScene>("res://scenes/PurpleBullet.tscn");
        RedBullet = ResourceLoader.Load<PackedScene>("res://scenes/RedBullet.tscn");
        timer = GetNode<Timer>("Timer");
    }

    public void changeWeapon(){
        if (bulletType+1 > 4) bulletType = 1;
        else bulletType++;
    }


    public void shoot(){
        
        if (timer.IsStopped()){
            Bullet bullet;
            switch (bulletType){
                case 1:
                    bullet = (Bullet) BlueBullet.Instance();
                break;
                case 2:
                    bullet = (Bullet) GreenBullet.Instance();
                break;
                case 3:
                    bullet = (Bullet) PurpleBullet.Instance();
                break;
                default:
                    bullet = (Bullet) RedBullet.Instance();
                break;
            }
            bullet.Transform = Transform;
            bullet.fire(GlobalPosition, new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)), bulletType);
            GetParent().GetParent().AddChild(bullet);
            timer.Start();
        }
    }


}
