using Godot;
using System;

public class UI : CanvasLayer
{
	private TextureRect weaponIcon;
	private Label weaponLabel;
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
	private TextureProgress lifeProgress;

	public override void _Ready()
	{
		root = GetNode<Control>("Control");
		weaponIcon = root.GetNode<TextureRect>("WeaponIcon");
		weaponLabel = root.GetNode<Label>("Label");
		textBox = root.GetNode<TextBox>("TextBox");
		lifeProgress = root.GetNode<TextureProgress>("HBoxContainer/Control/TextureProgress");
	}

	public void setWeapon(int weapon){
		switch(weapon){
			case 2:
				weaponIcon.Texture = jumpIcon;
				weaponLabel.Text = "Jump";
			break;
			case 3:
				weaponIcon.Texture = gravityIcon;
				weaponLabel.Text = "Gravity";
			break;
			case 4:
				weaponIcon.Texture = stickyIcon;
				weaponLabel.Text = "Sticky";
			break;
			default:
				weaponIcon.Texture = speedIcon;
				weaponLabel.Text = "Speed";
			break;
		}
	}

	public void showText(string text){
		textBox.showText(text);
	}

	public void setLife(int amount){
		lifeProgress.Value = amount;
	}

	public void Show(){
		root.Show();
	}

	public void Hide(){
		root.Hide();
	}
}
