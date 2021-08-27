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
    private int bulletType = 1;

    private Sprite weapon;
    private PackedScene BlueBullet;    
    private PackedScene GreenBullet;
    private PackedScene PurpleBullet;
    private PackedScene RedBullet;
    private PackedScene JumpParticles;
    private TileMap platforms;

    #region old
    /*public override void _Ready()
    {
        JumpParticles = ResourceLoader.Load<PackedScene>("res://scenes/JumpParticles.tscn");
        Bullet = ResourceLoader.Load<PackedScene>("res://scenes/Bullet.tscn");
        weapon = GetNode<Sprite>("Weapon");
    }


    public override void _PhysicsProcess(float delta)
    {

        GD.Print(energy + "      " +  actualJumpForce);
        if (Input.IsActionPressed("left")){
            velocity.x = -actualSpeed;
        }
        if (Input.IsActionPressed("right")){
            velocity.x = actualSpeed;
        }
        if (Input.IsActionPressed("fire")){
            shoot();
        }
        if (Input.IsActionJustPressed("secondary")){
            changeWeapon();
        }
        Vector2 mousePos = GetGlobalMousePosition();
        weapon.Rotation = Position.AngleToPoint(mousePos)-Mathf.Pi;
        if (Input.IsActionPressed("jump")){
            jump();
        }
        

        if (GetSlideCount() == 0){
            grounded = false;
        }else{
            for (int i = 0; i < GetSlideCount(); i++){
                KinematicCollision2D collision = GetSlideCollision(i);
                checkCollision(collision, delta);
            }
        } 

        if (!grounded){
            if (gravity > 0 && velocity.y > 0){
                energy+=velocity.y*delta;
            }else if (gravity < 0 && velocity.y < 0){
                energy-=velocity.y*delta;
            }else{
                energy = 0;
            }
        }else{
            energy = 0;
        }
        /*if (velocity.y > 0 && !grounded){
            energy+=velocity.y*delta;
        }else{
            energy = 0;
        }*//*
        MoveAndSlide(velocity, Vector2.Zero,false, 4, (float)Math.PI/4, false);
        velocity.x = Lerp(velocity.x, 0, 0.3f);
        if (!grounded) {
            velocity.y = velocity.y + gravity*gravityScale;
        }

    }

    private void jump(){
        if (grounded){
            Particles2D particles = (Particles2D)JumpParticles.Instance();
            particles.GlobalPosition = Position; 
            GetParent().AddChild(particles);
            velocity.y = -actualJumpForce*jumpForce*gravity;
            grounded = false;
            if (onGravityCell){
                gravity = -gravity;
            }
        }
    }

    private void changeWeapon(){
        if (bulletType+1 > 4) bulletType = 1;
        else bulletType++;
    }

    private void shoot(){
        Bullet bullet = (Bullet) Bullet.Instance();
        bullet.Transform = weapon.Transform;
        bullet.fire(GlobalPosition, new Vector2((float)Math.Cos(weapon.Rotation), (float)Math.Sin(weapon.Rotation)), bulletType);
        GetParent().AddChild(bullet);
    }

    private void checkCollision(KinematicCollision2D collision, float delta){
        if (collision != null){
            if (collision.Collider is TileMap){
                TileMap tmap = (TileMap) collision.Collider;
                Vector2 pos = tmap.WorldToMap(Position);
                int id = tmap.GetCellv(pos-collision.Normal);
                if (collision.Normal.x == 0 && collision.Normal.y == -1){ // En dessous des pieds
                    checkTile(id, delta, false);
                    if (gravity > 0){
                        grounded = true;
                    } else {
                        velocity.y = 0;
                    }
                } else if (collision.Normal.x == 0 && collision.Normal.y == 1){ // Au plafond
                    checkTile(id, delta, true);
                    if (gravity > 0){
                        velocity.y = 0;
                    }else{
                        grounded = true;
                    }
                } else { // On frotte un mur
                    velocity.x = 0;
                }
            }else if (collision.Collider is PhysicsBody2D && ((PhysicsBody2D) collision.Collider).IsInGroup("death_body")){
                GD.Print("MOORT");
            }
        }       
    }

    private void checkTile(int id, float delta, bool onTop){
        if (id >= 6 && id <= 11) {
            speedCell();
        }else if (id >= 12 && id <= 17) {
            jumpCell(delta);                        
        }else if (id >= 18 && id <= 23) {
            stickyCell(onTop);                        
        }else if (id < 6) {
            neutralCell();
        }
    }

    private void neutralCell(){
        actualSpeed = Lerp(actualSpeed, speed*minSpeed, 0.2f);
        actualJumpForce = jumpForce*minJumpForce;
        onGravityCell = false;
    }

    private void speedCell(){
        onGravityCell = false;
        actualJumpForce = jumpForce*minJumpForce;
        if (Input.IsActionPressed("left") || Input.IsActionPressed("right")){
            actualSpeed = Lerp(actualSpeed, speed*maxSpeed, 0.02f);
        }else{
            actualSpeed = speed*minSpeed;
        }
    }

    private void jumpCell(float delta){
        onGravityCell = false;
        actualJumpForce=Lerp(actualJumpForce, maxJumpForce, energy/200+Math.Abs(velocity.x)/20*delta);
        jump();
    }

    private void stickyCell(bool onTop){
        actualJumpForce = jumpForce*minJumpForce;
        onGravityCell = true;
    }

    float Lerp(float firstFloat, float secondFloat, float by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }*/
    #endregion
    public override void _Ready()
    {
        JumpParticles = ResourceLoader.Load<PackedScene>("res://scenes/JumpParticles.tscn");
        BlueBullet = ResourceLoader.Load<PackedScene>("res://scenes/BlueBullet.tscn");
        GreenBullet = ResourceLoader.Load<PackedScene>("res://scenes/GreenBullet.tscn");
        PurpleBullet = ResourceLoader.Load<PackedScene>("res://scenes/PurpleBullet.tscn");
        RedBullet = ResourceLoader.Load<PackedScene>("res://scenes/RedBullet.tscn");
        weapon = GetNode<Sprite>("Weapon");
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
            shoot();
        }
        if (Input.IsActionJustPressed("secondary")){
            changeWeapon();
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

    private void changeWeapon(){
        if (bulletType+1 > 4) bulletType = 1;
        else bulletType++;
    }

    private void shoot(){
        Bullet bullet;
        switch (bulletType){
            case 1:
                bullet = (Bullet) BlueBullet.Instance();
            break;
            case 2:
                bullet = (Bullet) GreenBullet.Instance();
            break;
            case 3:
                bullet = (Bullet) PurpleBullet.Instance();
            break;
            default:
                bullet = (Bullet) RedBullet.Instance();
            break;
        }
        bullet.Transform = weapon.Transform;
        bullet.fire(GlobalPosition, new Vector2((float)Math.Cos(weapon.Rotation), (float)Math.Sin(weapon.Rotation)), bulletType);
        GetParent().AddChild(bullet);
    }
}
