using Godot;
using System;

public class Credits : Control
{
    private Global global;
    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
    }

    public void OnBackButtonPressed(){
        global.menu.loadMenu("TitleScreen");
    }


}
