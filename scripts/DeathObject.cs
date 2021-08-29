using Godot;
using System;

public class DeathObject : StaticBody2D
{

    public void OnBodyEntered(Node2D body){
        if (body.IsInGroup("player")){
            ((Player) body).takeDamage(100);
        }
    }
}
