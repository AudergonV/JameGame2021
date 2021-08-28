using Godot;
using System;

public class Press : Node2D
{

    private AnimationPlayer animationPlayer;
    private Timer timer;
    private bool down;
    private bool wait;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        timer = GetNode<Timer>("Timer");
    }

    public void OnBodyEnteredCrushArea(PhysicsBody2D body){
    }

    public void OnBodyEnteredTriggerArea(PhysicsBody2D body){
        if (body.IsInGroup("player") && !down){
            animationPlayer.Play("slam");
            down = true;
        }
    }

    public void OnBodyExitedTriggerArea(PhysicsBody2D body){
        
    }

    public void OnAnimationFinished(string name){
        if (name.Equals("slam")){
            wait = true;
        } else {
            down = false;
        }
    }

    public void OnTimerTimeout(){
        animationPlayer.Play("reinit");
    }

}
