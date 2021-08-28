using Godot;
using System;

public class Global : Node
{
	private bool[] weapons = {false,false,false,false};
	private bool weaponUnlocked;
	public UI ui;
	public SceneLoader sceneLoader;
	public override void _Ready(){
		ui = GetNode<UI>("UI");
		sceneLoader = GetNode<SceneLoader>("SceneLoader");
	}

	public void swapWeapon(int weapon){
		ui.setWeapon(weapon);
	}

	public void toggleWeapon(int weapon, bool status = true){
		if (status) weaponUnlocked = true;
		weapons[weapon-1] = status;
	}

	public bool isWeaponEnabled(int weapon){
		return weapons[weapon-1];
	}

	public bool isWeaponUnlocked(){
		return weaponUnlocked;
	}

}
