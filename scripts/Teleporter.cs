using Godot;
using System;

public class Teleporter : StaticBody2D
{
    
    [Export]
    public int teleportTo = 1;

    public override void _Ready()
    {
        
    }

    public void OnBodyEntered(Node2D body){
        if (body.IsInGroup("player")){
            teleport();
        }
    }

    private void teleport(){
        GetTree().ChangeScene("res://scenes/levels/Level"+teleportTo+".tscn");
    }

}
