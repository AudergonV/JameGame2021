using Godot;
using System;

public class Menu : CanvasLayer
{

	public string backMenu;
    
    public override void _Ready()
    {
    }

	public void loadMenu(string name){
		clear();
		AddChild(ResourceLoader.Load<PackedScene>("res://scenes/menu/"+name+".tscn").Instance());
	}

    public void clear(){
		foreach (Node child in GetChildren())
		{
			RemoveChild(child);
			child.QueueFree();
		}
	}
}
