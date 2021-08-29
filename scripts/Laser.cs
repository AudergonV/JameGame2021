using Godot;
using System;

public class Laser : RayCast2D
{
    private Line2D blueLaser;
    private Line2D greenLaser;
    private Line2D purpleLaser;
    private Line2D redLaser;
    private Line2D currentLaser;

    private Sprite blueArm;
    private Sprite greenArm;
    private Sprite purpleArm;
    private Sprite redArm;
    private Sprite currentArm;

    private Tween tween;

    [Export]
    public PackedScene Particles;
    [Export]
    public PackedScene SplatSound;
    [Export]
    public Material blueMaterial;
    [Export]
    public Material greenMaterial;
    [Export]
    public Material purpleMaterial;
    [Export]
    public Material redMaterial;

    private Material[] materials;

    private bool isCasting;
    private int bulletType = 1;

    [Export]
    public float laserWidth = 1.5f;
    [Export]
    public float length = 300f;

    private TileMap tmap_platforms;
    private AudioStreamPlayer2D audio;

    private bool hflipped = false;
    private bool vflipped = false;


    private Global global;

    public override void _Ready()
    {
        SetPhysicsProcess(false);
        audio = GetNode<AudioStreamPlayer2D>("Audio");

        global = GetNode<Global>("/root/Global");

        blueLaser = GetNode<Line2D>("BlueLaser");
        greenLaser = GetNode<Line2D>("GreenLaser");
        purpleLaser = GetNode<Line2D>("PurpleLaser");
        redLaser = GetNode<Line2D>("RedLaser");
        currentLaser = blueLaser;


        blueArm = GetNode<Sprite>("BlueArm");
        greenArm = GetNode<Sprite>("GreenArm");
        purpleArm = GetNode<Sprite>("PurpleArm");
        redArm = GetNode<Sprite>("RedArm");
        currentArm = blueArm;

        blueLaser.Points[1] = Vector2.Zero;
        greenLaser.Points[1] = Vector2.Zero;
        purpleLaser.Points[1] = Vector2.Zero;
        redLaser.Points[1] = Vector2.Zero;

        materials = new Material[4];
        materials[0] = blueMaterial;
        materials[1] = greenMaterial;
        materials[2] = purpleMaterial;
        materials[3] = redMaterial;

        tween = GetNode<Tween>("Tween");
        tmap_platforms = global.GetNode<Node2D>("SceneLoader").GetNode<Node2D>("Node2D").GetNode<TileMap>("Platforms");
    }

    
    public void flip(bool flip, bool gravity){
        if (flip != hflipped){
            hflipped = flip;
            ZIndex = -ZIndex;
        }
        if (gravity != vflipped){
            vflipped = gravity;
            Position = new Vector2(Position.x, -Position.y);
        }
        
    }

    public void toggleCasting(bool casting){
        if (global.isWeaponUnlocked()){
            if (casting != isCasting){        
                isCasting = !isCasting;
                if (isCasting){
                    appear();
                }else{
                    disappear();
                }

            }
            SetPhysicsProcess(isCasting);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 cast_point = CastTo;
        ForceRaycastUpdate();
        

        if (IsColliding()){
            cast_point = ToLocal(GetCollisionPoint());
            if (GetCollider() is WallButton){
                ((WallButton)GetCollider()).activate();
            }else if (GetCollider() is Slime){
                ((Slime)GetCollider()).LoseLife(10);
            }else {
                Vector2 pos = tmap_platforms.WorldToMap(GetCollisionPoint());
                Vector2 nor = GetCollisionNormal();
                Vector2 cellPos = nor.y < 0 ? pos : pos-nor;
                int id = tmap_platforms.GetCellv(cellPos);
                if (id >= 0 && id < 30 && (id-id%6)/6 != bulletType) { 
                    tmap_platforms.SetCellv(cellPos, bulletType*6+id%6);
                    #region particles
                    Particles2D particles = (Particles2D)Particles.Instance();
                    if (nor.y > 0) particles.Rotation = (float)Math.PI;
                    particles.ProcessMaterial = (materials[bulletType-1]);
                    particles.GlobalPosition = GetCollisionPoint(); 
                    GetTree().Root.AddChild(particles);
                    AudioStreamPlayer2D audio = (AudioStreamPlayer2D) SplatSound.Instance();
                    audio.GlobalPosition = GetCollisionPoint(); 
                    GetTree().Root.AddChild(audio);
                    #endregion
                }
            }
        }
        blueLaser.SetPointPosition(1, cast_point);
        greenLaser.SetPointPosition(1, cast_point);
        purpleLaser.SetPointPosition(1, cast_point);
        redLaser.SetPointPosition(1, cast_point);
    }

    private void appear(){
        tween.StopAll();
        tween.InterpolateProperty(blueLaser, "width", 0, laserWidth, 0.2f);
        tween.InterpolateProperty(greenLaser, "width", 0, laserWidth, 0.2f);
        tween.InterpolateProperty(purpleLaser, "width", 0, laserWidth, 0.2f);
        tween.InterpolateProperty(redLaser, "width", 0, laserWidth, 0.2f);
        tween.InterpolateProperty(audio, "volume_db", -40, 1, 0.2f);
        tween.Start();
        audio.Play();
    }

    async private void disappear(){
        tween.StopAll();
        tween.InterpolateProperty(blueLaser, "width", laserWidth, 0, 0.2f);
        tween.InterpolateProperty(greenLaser, "width", laserWidth, 0, 0.2f);
        tween.InterpolateProperty(purpleLaser, "width", laserWidth, 0, 0.2f);
        tween.InterpolateProperty(redLaser, "width", laserWidth, 0, 0.2f);
        tween.InterpolateProperty(audio, "volume_db", 1, -40, 0.2f);
        tween.Start();
        await ToSignal(tween, "tween_all_completed");
        audio.Stop();
    }

    public void changeWeapon(int id = -1){
        if (id < 1){
            do{
                bulletType++; 
                if (bulletType > 4) bulletType = 1;
            }while(!global.isWeaponEnabled(bulletType));
        } else {
            if (global.isWeaponEnabled(id)) bulletType = id;
        }
        global.ui.setWeapon(bulletType);
        currentLaser.Hide();
        currentArm.Hide();
        switch (bulletType){
            case 2:
                currentLaser = greenLaser;
                currentArm = greenArm;
            break;
            case 3:
                currentLaser = purpleLaser;
                currentArm = purpleArm;
            break;
            case 4:
                currentLaser = redLaser;
                currentArm = redArm;
            break;
            default:
                currentLaser = blueLaser;
                currentArm = blueArm;
            break;
        }
        currentLaser.Show();
        currentArm.Show();
    }

    async public void flash(){
        ((ShaderMaterial)blueArm.Material).SetShaderParam("hit_strength", 1);
        ((ShaderMaterial)greenArm.Material).SetShaderParam("hit_strength", 1);
        ((ShaderMaterial)purpleArm.Material).SetShaderParam("hit_strength", 1);
        ((ShaderMaterial)redArm.Material).SetShaderParam("hit_strength", 1);
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        ((ShaderMaterial)blueArm.Material).SetShaderParam("hit_strength", 0);
        ((ShaderMaterial)greenArm.Material).SetShaderParam("hit_strength", 0);
        ((ShaderMaterial)purpleArm.Material).SetShaderParam("hit_strength", 0);
        ((ShaderMaterial)redArm.Material).SetShaderParam("hit_strength", 0);
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        ((ShaderMaterial)blueArm.Material).SetShaderParam("hit_strength", 1);
        ((ShaderMaterial)greenArm.Material).SetShaderParam("hit_strength", 1);
        ((ShaderMaterial)purpleArm.Material).SetShaderParam("hit_strength", 1);
        ((ShaderMaterial)redArm.Material).SetShaderParam("hit_strength", 1);
        await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        ((ShaderMaterial)blueArm.Material).SetShaderParam("hit_strength", 0);
        ((ShaderMaterial)greenArm.Material).SetShaderParam("hit_strength", 0);
        ((ShaderMaterial)purpleArm.Material).SetShaderParam("hit_strength", 0);
        ((ShaderMaterial)redArm.Material).SetShaderParam("hit_strength", 0);
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
    }
}
