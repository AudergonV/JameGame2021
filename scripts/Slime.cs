using Godot;
using System;

public class Slime : KinematicBody2D
{
    Vector2 velocity;
    Global global;
    AnimatedSprite animatedSprite;
    private bool prejump;
    [Export]
    public float speed = 40f;
    public int life = 300;
    [Export]
    public int damage = 20;

    private AnimationPlayer animationPlayer;
    private RayCast2D wallCast;
    private RayCast2D floorCast;
    private RayCast2D jumpCast;
    private Vector2 floorCastPoint, wallCastPoint, jumpCastPoint;
    [Export]
    public Vector2 direction = Vector2.Right;
    private Node2D target;
    private bool hflipped, triggered;
    private TextureProgress lifeBar;
    private AudioStreamPlayer2D audio;
    private Player player;
    
    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
        animatedSprite = GetNode<AnimatedSprite>("Sprite");
        wallCast = GetNode<RayCast2D>("WallCast");
        floorCast = GetNode<RayCast2D>("FloorCast");
        jumpCast = GetNode<RayCast2D>("JumpCast");
        lifeBar = GetNode<TextureProgress>("LifeBar");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        audio = GetNode<AudioStreamPlayer2D>("Audio");
        lifeBar.MaxValue = life;
    }

    public override void _Process(float delta){
        if (player != null){
            player.takeDamage(damage);
        }
    }

    public override void _PhysicsProcess(float delta){
        if (triggered){
            Vector2 dir = Position.DirectionTo(target.Position).Normalized();
            if (dir.x > 0) direction = Vector2.Right;
            else direction = Vector2.Left;
            speed = 60;
        }else{
            speed = 40;
        }
        #region raycasts
        wallCastPoint = wallCast.CastTo;
        floorCastPoint = floorCast.CastTo;
        jumpCastPoint = jumpCast.CastTo;
        wallCast.ForceRaycastUpdate();
        floorCast.ForceRaycastUpdate();
        jumpCast.ForceRaycastUpdate();
        if (wallCast.IsColliding()){
            if (wallCast.GetCollider() is TileMap){
                if (jumpCast.IsColliding()){
                    direction = -direction;
                }else{
                    preJump();
                }
            }
        }
        if (IsOnFloor() && !floorCast.IsColliding()){
            direction = -direction;
        }
        #endregion
        velocity.y += global.gravityScale;
        velocity.x = direction.x * speed;
        animate();
        velocity = MoveAndSlide(velocity, Vector2.Up, false, 4, (float)Math.PI/4, false);

    }

    private void preJump(){
        prejump = true;
    }

    private void jump(){
        if (IsOnFloor()){
        velocity.y = -200;
        }
    }

    public void OnAnimationFinished(){
        if (prejump){
            jump();
            prejump = false;
        }
    }

    private void animate(){
        flip(velocity.x > 0);
        if (IsOnFloor()){
            if (prejump){
                animatedSprite.Play("prejump" + (triggered ? "_triggered" : ""));
            }else{
                animatedSprite.Play("moving" + (triggered ? "_triggered" : ""));
            }
        }else {
            if (velocity.y > 0){
                animatedSprite.Play("falling" + (triggered ? "_triggered" : ""));
            }else{
                animatedSprite.Play("jumping" + (triggered ? "_triggered" : ""));
            }
        }
    }

    private void flip(bool hflip){
        if (hflipped != hflip){
            hflipped = hflip;
            animatedSprite.FlipH = hflip;
            wallCast.Position = new Vector2(-wallCast.Position.x, wallCast.Position.y);
            floorCast.Position = new Vector2(-floorCast.Position.x, floorCast.Position.y);
            jumpCast.Position = new Vector2(-jumpCast.Position.x, jumpCast.Position.y);
            wallCast.CastTo = new Vector2(-wallCast.CastTo.x, wallCast.CastTo.y);
            floorCast.CastTo = new Vector2(-floorCast.CastTo.x, floorCast.CastTo.y);
            jumpCast.CastTo = new Vector2(-jumpCast.CastTo.x, jumpCast.CastTo.y);
        }

    }

    public void OnBodyEntered(Node2D body){
        if (body.IsInGroup("player")){
            ((Player) body).takeDamage(damage);
           // ((Player) body).knockback((GlobalPosition-body.GlobalPosition).Normalized());
            player = (Player) body;
        }else if (body is Slime){
            direction = -direction;
        }
    }

    public void OnDeathAreaBodyExited(Node2D body){
        if (body.IsInGroup("player")){
            player = null;
        }
    }

    public void OnTriggerBodyEntered(Node2D body){
        if (body.IsInGroup("player")){
            target = body;
            triggered = true;
        }
    }

    public void OnTriggerBodyExited(Node2D body){
        if (body.IsInGroup("player")){
            triggered = false;
        }
    }

    public void LoseLife(int amount){
        life-= amount;
        if (life % (amount*10) == 0){
            flash();
        }
        lifeBar.Value = life;
        if (life <= 0){
            SetPhysicsProcess(false);
            animationPlayer.Play("death");
        }
    }

    async public void flash(){
        audio.Play();
        audio.PitchScale = (float) new Random().NextDouble()+0.7f;
        ((ShaderMaterial)animatedSprite.Material).SetShaderParam("hit_strength", 1);
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        ((ShaderMaterial)animatedSprite.Material).SetShaderParam("hit_strength", 0);
    }
}
