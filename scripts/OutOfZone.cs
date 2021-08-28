using Godot;
using System;

public class OutOfZone : Area2D
{
    [Export]
    public Vector2 respawnPoint;

    public override void _Ready()
    {
        
    }

    public void OnBodyEntered(Node2D body){
        if (body.IsInGroup("player")){
            body.SetPosition(respawnPoint);
        }
    }

}

