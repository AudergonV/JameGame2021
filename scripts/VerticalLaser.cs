using Godot;
using System;

public class VerticalLaser : Node2D
{

    [Export]
    public float length;

    private Sprite top;
    private Sprite bot;
    private Line2D laser;
    private Tween tween;
    private CollisionShape2D collisionShape;

    private bool status = true;

    public override void _Ready()
    {
        top = GetNode<Sprite>("Top");
        bot = GetNode<Sprite>("Bot");
        laser = GetNode<Line2D>("Laser");
        tween = GetNode<Tween>("Tween");
        collisionShape = GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D");
        laser.SetPointPosition(1, laser.Points[0]+new Vector2(0,length));
        top.Position = laser.Points[0];
        bot.Position = laser.Points[1];
        ((SegmentShape2D) collisionShape.Shape).A = laser.Points[0];
        ((SegmentShape2D) collisionShape.Shape).B = laser.Points[1];
    }

    public override void _PhysicsProcess(float delta)
    {
        
    }

    public void toggleLaser(bool status){
        status = !status;
        if (status != this.status){
            //collisionShape.Disabled = status;
            collisionShape.CallDeferred("set_disabled", !status);
            this.status = status;
            if (this.status) {
                turnOn();
            } else {
                turnOff();
            }
        }
    }

    private void turnOn(){
        tween.StopAll();
        tween.InterpolateProperty(laser, "width", 0, 2, 0.2f);
        tween.Start();
    }

    private void turnOff(){
        tween.StopAll();
        tween.InterpolateProperty(laser, "width", 2, 0, 0.2f);
        tween.Start();
    }

    public void OnBodyEntered(Node2D body){
        if (body.IsInGroup("player")){
            ((Player) body).die();
        }
    }
}
