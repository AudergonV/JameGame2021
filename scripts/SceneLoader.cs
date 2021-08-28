using Godot;
using System;

public class SceneLoader : Node2D
{
	
	public override void _Ready()
	{
		loadLevel(1);
	}

	public void loadLevel(int id){
		foreach (Node child in GetChildren())
		{
			RemoveChild(child);
			child.QueueFree();
		}
		AddChild(ResourceLoader.Load<PackedScene>("res://scenes/levels/Level"+id+".tscn").Instance());
	}


}
