using Godot;
using System;

public class Teleporter : StaticBody2D
{
    
    [Export]
    public int teleportTo = 1;
    private Global global;

    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
    }

    public void OnBodyEntered(Node2D body){
        if (body.IsInGroup("player")){
            teleport();
        }
    }

    private void teleport(){
        global.sceneLoader.CallDeferred("loadLevel",teleportTo);
    }

}
