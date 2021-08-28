using Godot;
using System;

public class Stand : Node2D
{
    public enum StandColor { 
        Blue,
        Green,
        Purple,
        Red
    }
    [Export]
    public StandColor color;
    [Export]
    public Texture blueBall;
    [Export]
    public Texture greenBall;
    [Export]
    public Texture purpleBall;
    [Export]
    public Texture redBall;
    [Export]
    public Texture blueStand;
    [Export]
    public Texture greenStand;
    [Export]
    public Texture purpleStand;
    [Export]
    public Texture redStand;

    private Sprite stand;
    private Sprite ball;
    private Color red = new Color(1.4f,1,1,1);
    private Color redOff = new Color(0.8f,1,1,1);
    private Color blue = new Color(1,1,1.4f,1);
    private Color blueOff = new Color(1,1,0.8f,1);
    private Color green = new Color(1,1.4f,1,1);
    private Color greenOff = new Color(1,0.8f,1,1);
    private Color purple = new Color(1.4f,1,1.4f,1);
    private Color purpleOff = new Color(0.8f,1,0.8f,1);

    private Global global;
    private AnimationPlayer animationPlayer;

    private bool took;
    public override void _Ready()
    {
        stand = GetNode<Sprite>("Stand");
        ball = GetNode<Sprite>("Ball");
        global = GetNode<Global>("/root/Global");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Play("default");
        loadTextures();
    }

    private void loadTextures(){
        switch(color){
            case StandColor.Blue:
                stand.Texture = blueStand;
                stand.Modulate = blue;
                ball.Texture = blueBall;
                ball.Modulate = blue;
            break;
            case StandColor.Green:
                stand.Texture = greenStand;
                stand.Modulate = green;
                ball.Texture = greenBall;
                ball.Modulate = green;
            break;
            case StandColor.Purple:
                stand.Texture = purpleStand;
                stand.Modulate = purple;
                ball.Texture = purpleBall;
                ball.Modulate = purple;
            break;
            case StandColor.Red:
                stand.Texture = redStand;
                stand.Modulate = red;
                ball.Texture = redBall;
                ball.Modulate = red;
            break;
        }
    }

    public void OnBodyEntered(Node2D body){
        if (body.IsInGroup("player") &&!took){
            took = true;
            animationPlayer.Play("Pickup");
            switch(color){
            case StandColor.Blue:
                stand.Modulate = blueOff;
                global.toggleWeapon(1);
                ((Player)body).weapon.changeWeapon(1);
                global.ui.showText("You picked up the blue laser. Color the platforms in blue to run faster!");
            break;
            case StandColor.Green:
                stand.Modulate = greenOff;
                global.toggleWeapon(2);
                ((Player)body).weapon.changeWeapon(2);
                global.ui.showText("You picked up the green laser. Color the platforms in green to jump higher!");
            break;
            case StandColor.Purple:
                stand.Modulate = purpleOff;
                global.toggleWeapon(3);
                ((Player)body).weapon.changeWeapon(3);
                global.ui.showText("You picked up the purple laser. Color the platforms in purple and jump to reverse gravity!");
            break;
            case StandColor.Red:
                stand.Modulate = redOff;
                global.toggleWeapon(4);
                ((Player)body).weapon.changeWeapon(4);
                global.ui.showText("You picked up the red laser. Color the ceiling in red and get sticked to it!");
            break;
            }
        }
    }
}
