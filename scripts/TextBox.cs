using Godot;
using System;

public class TextBox : ColorRect
{
    
    private Label label;
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        label = GetNode<Label>("Label");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    async public void showText(string text, float time = 5f){
        Show();
        label.Text = text;
        animationPlayer.Play("show");
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        Hide();

    }


}
