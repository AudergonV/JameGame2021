using Godot;
using System;

public class TitleScreen : Control
{
  
    private Global global;

    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
        global.menu.backMenu = "TitleScreen";
    }

    public void OnPlayButtonPressed(){
        global.menu.clear();
        global.ui.Show();
        global.sceneLoader.loadLevel("1");
    }

    public void OnSettingsButtonPressed(){
        global.menu.loadMenu("settings");
    }

    public void OnCreditsButtonPressed(){
        global.menu.loadMenu("Credits");
    }

    public void OnExitButtonPressed(){
        GetTree().Quit();
    }

}
