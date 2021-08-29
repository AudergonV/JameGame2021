using Godot;
using System;

public class Teleporter : StaticBody2D
{
    
    [Export]
    public string teleportTo = "1";
    private Global global;
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void OnBodyEntered(Node2D body){
        if (body.IsInGroup("player")){
            animationPlayer.Play("teleport");
        }
    }

    private void teleport(){
        global.sceneLoader.CallDeferred("loadLevel",teleportTo);
    }

}
