using Godot;
using System;

public class SceneLoader : Node2D
{
	
	public override void _Ready()
	{
	}


	public void loadLevel(string id){
		clear();
		AddChild(ResourceLoader.Load<PackedScene>("res://scenes/levels/Level"+id+".tscn").Instance());
	}

	public void clear(){
		foreach (Node child in GetChildren())
		{
			RemoveChild(child);
			child.QueueFree();
		}
	}


}
