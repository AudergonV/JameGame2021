using Godot;
using System;

public class Pause : Control
{
    private Global global;
    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");   
        global.menu.backMenu = "Pause";
    }

    public override void _Process(float delta){
		if (Input.IsActionJustPressed("pause")){
			global.togglePause(false);
		}
	}


    public void OnResumeButtonPressed(){
        global.togglePause(false);
    }

    public void OnSettingsButtonPressed(){
        global.menu.loadMenu("Settings");
    }

    public void OnExitButtonPressed(){
        global.restart();
    }
}
