using Godot;
using System;

public class LevelEnd : Control
{
    private Global global;

    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
    }

    public void end(){
        global.restart();
    }


}
