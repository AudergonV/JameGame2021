using Godot;
using System;

public class Button : Node2D
{
    private Sprite sprite;
    private Tween tween;
    private Timer timer;
    private bool status;

    private Color off = new Color(0.7f,1,1,1);
    private Color on = new Color(1.4f,1,1,1);

    [Signal]
    public delegate void StatusChanged(bool status);
    [Export]
    public float time;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        timer = GetNode<Timer>("Timer");
        tween = GetNode<Tween>("Tween");
        sprite.Modulate = off;
    }

    public void OnBodyEntered(PhysicsBody2D body){
        if (body.IsInGroup("player")){
            status = true;
            EmitSignal("StatusChanged", status);
            sprite.Modulate = on;
            timer.Stop();
        }
    }

    public void OnBodyExited(PhysicsBody2D body){
        if (body.IsInGroup("player")){
            timer.Start(time);
           /* tween.StopAll();
            tween.InterpolateProperty();
            tween.Start();*/
        }
    }

    public void OnTimerTimeout(){
        status = false;
        EmitSignal("StatusChanged", status);
        sprite.Modulate = off;
    }

}
