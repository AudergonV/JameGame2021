using Godot;
using System;

public class UI : CanvasLayer
{
	private TextureRect weaponIcon;
	private Control root;

	[Export]
	public Texture speedIcon;
	[Export]
	public Texture jumpIcon;
	[Export]
	public Texture gravityIcon;
	[Export]
	public Texture stickyIcon;

	private TextBox textBox;

	public override void _Ready()
	{
		root = GetNode<Control>("Control");
		weaponIcon = root.GetNode<TextureRect>("WeaponIcon");
		textBox = root.GetNode<TextBox>("TextBox");
	}

	public void setWeapon(int weapon){
		switch(weapon){
			case 2:
				weaponIcon.Texture = jumpIcon;
			break;
			case 3:
				weaponIcon.Texture = gravityIcon;
			break;
			case 4:
				weaponIcon.Texture = stickyIcon;
			break;
			default:
				weaponIcon.Texture = speedIcon;
			break;
		}
	}

	public void showText(string text){
		textBox.showText(text);
	}
}
