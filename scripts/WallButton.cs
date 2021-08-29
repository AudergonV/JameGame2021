using Godot;
using System;

public class WallButton : RigidBody2D
{
    private Sprite sprite;
    private Tween tween;
    private Timer timer;
    private bool status;

    private Color off = new Color(0.7f,1,1,1);
    private Color on = new Color(1.7f,1,1,1);

    [Signal]
    public delegate void StatusChanged(bool status);
    [Export]
    public float time;
    private AudioStreamPlayer2D audio;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        timer = GetNode<Timer>("Timer");
        tween = GetNode<Tween>("Tween");
        audio = GetNode<AudioStreamPlayer2D>("Audio");
        sprite.Modulate = off;
    }

    public void activate(){
        if (!status) audio.Play();
        status = true;
        EmitSignal("StatusChanged", status);
        sprite.Modulate = on;
        timer.Start(time);
    }

    public void OnTimerTimeout(){
        status = false;
        EmitSignal("StatusChanged", status);
        sprite.Modulate = off;
    }
}
