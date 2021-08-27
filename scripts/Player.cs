using Godot;
using System;


public class Player : KinematicBody2D
{
    [Export]
    public float speed = 1f;
    [Export]
    public float jumpForce = 1f;
    [Export]
    public float gravity = 1f;

    private bool onGravityCell = false;
    private bool sticky = false;
    private float actualSpeed, actualJumpForce;
    private int maxSpeed = 300;
    private int minSpeed = 100;
    private float maxJumpForce = 500;
    private int minJumpForce = 210;
    private int gravityScale = 10;
    private float energy = 0f;

    private Vector2 velocity = new Vector2(0,0);
    private Vector2 floorDirection = new Vector2(0,-1);

    //private Weapon weapon;
    private Laser weapon;
    private PackedScene JumpParticles;
    private TileMap platforms;

    public override void _Ready()
    {
        JumpParticles = ResourceLoader.Load<PackedScene>("res://scenes/JumpParticles.tscn");
        weapon = GetNode<Laser>("Weapon");
        platforms = GetParent().GetNode<TileMap>("Platforms");
        //Temp
        actualSpeed = minSpeed;
        actualJumpForce = minJumpForce;
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 mousePos = GetGlobalMousePosition();
        weapon.Rotation = Position.AngleToPoint(mousePos)-Mathf.Pi;

        if (Input.IsActionPressed("left")){
            velocity.x = -actualSpeed;
        }
        if (Input.IsActionPressed("right")){
            velocity.x = actualSpeed;
        }
        if (Input.IsActionPressed("fire")){
            weapon.toggleCasting(true);
        }else{
            weapon.toggleCasting(false);
        }
        if (Input.IsActionJustPressed("secondary")){
            weapon.changeWeapon();
        }
        if (Input.IsActionJustPressed("down")){
            if (gravity > 0) sticky = false;
            else jump();
        }
        if (Input.IsActionPressed("jump")){
            if (gravity > 0) jump();
            else sticky = false;
        }
        if (!sticky) velocity.y = velocity.y + gravity*gravityScale;


        velocity = MoveAndSlide(velocity, floorDirection, false, 4, (float)Math.PI/4, false);
        velocity.x = Lerp(velocity.x, 0, 0.3f);
        
        Surface surface = GetSurface(delta);
        if (IsOnFloor()){
            if (surface != Surface.Speed){
                actualSpeed = Lerp(actualSpeed, speed*minSpeed, 0.2f);
            }
            if (surface != Surface.Jump) {
                actualJumpForce = minJumpForce;
            }
            if (surface != Surface.Gravity){
                onGravityCell = false;
            }
            energy = 0;
        } else {
            if (surface != Surface.Sticky){
                sticky = false;
            }
            if (gravity > 0 && velocity.y > 0){
                energy+=velocity.y*delta;
            }else if (gravity < 0 && velocity.y < 0){
                energy-=velocity.y*delta;
            }else{
                energy = 0;
            }
        }

    }

    private Surface GetSurface(float delta) {
        if (IsOnFloor()) {
            Vector2 pos = platforms.WorldToMap(Position);
            int id = platforms.GetCellv(pos-floorDirection);
            if (id >= 6 && id <= 11) {
                speedCell();
                return Surface.Speed;
            }else if (id >= 12 && id <= 17) {
                jumpCell(delta);
                return Surface.Jump;                        
            }else if (id >= 18 && id <= 23) {
                gravityCell();
                return Surface.Gravity;     
            }else {
                return Surface.None;
            }
        } else {
            Vector2 pos = platforms.WorldToMap(Position);
            int id = platforms.GetCellv(pos+floorDirection);
            if (id >= 24 && id <= 29){
                stickyCell();
                return Surface.Sticky;
            }else {
                return Surface.None;
            }
        }
    }

    private void stickyCell(){
        sticky = true;
    }

    private void speedCell(){
        if (Input.IsActionPressed("left") || Input.IsActionPressed("right")){
            actualSpeed = Lerp(actualSpeed, speed*maxSpeed, 0.02f);
        }else{
            actualSpeed = Lerp(actualSpeed, speed*minSpeed, 0.2f);
        }
    }

    private void jumpCell(float delta){
        if (actualJumpForce == minJumpForce) { //Premier saut
            if (actualSpeed >= minSpeed){ //On bouge
                actualJumpForce = actualJumpForce + 50 * Math.Abs(velocity.x)/100;
            }else{ //on bouge pas ou presque pas
                actualJumpForce = actualJumpForce + 50;
            }

        } 
        actualJumpForce = Lerp(actualJumpForce, maxJumpForce, energy/200);
        GD.Print(actualJumpForce);
        jump();
    }

    private void gravityCell(){
        onGravityCell = true;
    }

    private void jump(){
        if (IsOnFloor()){
            Particles2D particles = (Particles2D)JumpParticles.Instance();
            particles.GlobalPosition = Position; 
            GetParent().AddChild(particles);
            velocity.y = -actualJumpForce*jumpForce*gravity;
            if (onGravityCell){
                gravity = -gravity;
                floorDirection = -floorDirection;   
                onGravityCell = false;
            }
        }
    }

    float Lerp(float firstFloat, float secondFloat, float by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }


}
