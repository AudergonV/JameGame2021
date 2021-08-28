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
    [Export]
    public Vector2 respawnPoint = Vector2.Zero;

    private bool onGravityCell = false;
    private bool sticky = false;
    private bool running = false;
    private bool facingLeft;
    private float actualSpeed, actualJumpForce;
    private int maxSpeed = 250;
    private int minSpeed = 75;
    private float maxJumpForce = 500;
    private int minJumpForce = 170;
    private int gravityScale = 10;
    private float energy = 0f;

    private Vector2 velocity = new Vector2(0,0);
    private Vector2 floorDirection = new Vector2(0,-1);

    //private Weapon weapon;
    private Laser weapon;
    private PackedScene JumpParticles;
    private TileMap platforms;
    private AnimatedSprite animatedSprite;
    private AudioStreamPlayer2D audio;
    private PackedScene DeathParticles;
    public override void _Ready()
    {
        JumpParticles = ResourceLoader.Load<PackedScene>("res://scenes/JumpParticles.tscn");
        DeathParticles = ResourceLoader.Load<PackedScene>("res://scenes/DeathParticles.tscn");
        audio = GetNode<AudioStreamPlayer2D>("Audio");
        weapon = GetNode<Laser>("Weapon");
        animatedSprite = GetNode<AnimatedSprite>("Sprite");
        platforms = GetParent().GetNode<TileMap>("Platforms");
        //Temp
        actualSpeed = minSpeed;
        actualJumpForce = minJumpForce;
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 mousePos = GetGlobalMousePosition();
        weapon.Rotation = Position.AngleToPoint(mousePos)-Mathf.Pi;
        running = false;
        if (Input.IsActionPressed("left")){
            velocity.x = -actualSpeed;
            facingLeft = true;
            running = true;
        } 
        if (Input.IsActionPressed("right")){
            velocity.x = actualSpeed;
            facingLeft = false;
            running = true;
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
        animate();
        velocity = MoveAndSlide(velocity, floorDirection, false, 4, (float)Math.PI/4, false);
        
        Surface surface = GetSurface(delta);
        if (IsOnFloor()){
            if (surface != Surface.Speed){
                actualSpeed = Lerp(actualSpeed, speed*minSpeed, 0.2f);
                velocity.x = Lerp(velocity.x, 0, 0.3f);
            }else{
                velocity.x = Lerp(velocity.x, 0, 0.05f);
            }
            if (surface != Surface.Jump) {
                actualJumpForce = minJumpForce;
            }
            if (surface != Surface.Gravity){
                onGravityCell = false;
            }
            energy = 0;
        } else {
            velocity.x = Lerp(velocity.x, 0, 0.3f);
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

        if (energy == 0){
            if (actualSpeed >= minSpeed){ //On bouge
                actualJumpForce = actualJumpForce + 50 * Math.Abs(velocity.x)/100;
            }else{ //on bouge pas ou presque pas
                actualJumpForce = actualJumpForce + 50;
            }
        }else{
             actualJumpForce = minJumpForce + energy*5;
             if (actualJumpForce > maxJumpForce) actualJumpForce = maxJumpForce;
            GD.Print(actualJumpForce);
        }
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

    private void animate(){
        weapon.flip(facingLeft, gravity < 0);
        animatedSprite.FlipH = facingLeft;
        animatedSprite.FlipV = gravity < 0;
        if (running){
            animatedSprite.SpeedScale = Math.Abs(velocity.x)/150;
        }else{
            animatedSprite.SpeedScale = 1;
        }
        if (IsOnFloor()){
            if (running) {
                if (velocity.x > 0){
                    animatedSprite.Play("Running");
                } else {
                    animatedSprite.Play("RunningLeft");
                }
            } else {
                if (velocity.x >= 0){
                    animatedSprite.Play("Idle");
                } else {
                    animatedSprite.Play("IdleLeft");
                }
            }
        } else {
            if (velocity.y*gravity > 0){
                if (velocity.x > 0){
                    animatedSprite.Play("Falling");
                } else {
                    animatedSprite.Play("FallingLeft");
                }
            }else{
                if (velocity.x > 0){
                    animatedSprite.Play("Jumping");
                } else {
                    animatedSprite.Play("JumpingLeft");
                }
            }
        }
    }

    async public void die(){
        Particles2D particles = (Particles2D)DeathParticles.Instance();
        particles.GlobalPosition = Position; 
        GetParent().AddChild(particles);
        if (velocity.x >= 0){
                animatedSprite.Play("Idle");
            } else {
                animatedSprite.Play("IdleLeft");
        }
        audio.Play();
        SetPhysicsProcess(false);
        weapon.flash();
        ((ShaderMaterial)animatedSprite.Material).SetShaderParam("hit_strength", 1);
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        ((ShaderMaterial)animatedSprite.Material).SetShaderParam("hit_strength", 0);
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        ((ShaderMaterial)animatedSprite.Material).SetShaderParam("hit_strength", 1);
        await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        ((ShaderMaterial)animatedSprite.Material).SetShaderParam("hit_strength", 0);
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        Position = respawnPoint;
        gravity = 1f;
        velocity = Vector2.Zero;
        SetPhysicsProcess(true);
    }


}
