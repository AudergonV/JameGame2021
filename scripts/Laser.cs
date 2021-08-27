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

    private bool isCasting;
    private int bulletType = 1;

    [Export]
    public float laserWidth = 1.5f;

    private TileMap tmap_platforms;

    public override void _Ready()
    {
        SetPhysicsProcess(false);
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

        tween = GetNode<Tween>("Tween");
        tmap_platforms = GetTree().GetRoot().GetNode<Node2D>("Node2D").GetNode<TileMap>("Platforms");
    }

    public void toggleCasting(bool casting){
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

    public override void _PhysicsProcess(float delta)
    {
        Vector2 cast_point = CastTo;
        ForceRaycastUpdate();

        if (IsColliding()){
            cast_point = ToLocal(GetCollisionPoint());
            Vector2 pos = tmap_platforms.WorldToMap(GetCollisionPoint());
            Vector2 nor = GetCollisionNormal();
            Vector2 cellPos = nor.y < 0 ? pos : pos-nor;
            int id = tmap_platforms.GetCellv(cellPos);
           // GD.Print("HIT " + id + "   " + (cellPos) + "    " + GetCollisionPoint());
            if (id >= 0 && id < 30 && (id-id%6)/6 != bulletType) { 
                tmap_platforms.SetCellv(cellPos, bulletType*6+id%6);
                /*Particles2D particles = (Particles2D)Particles.Instance();
                if (direction.y < 0) particles.Rotation = (float)Math.PI;
                particles.SetProcessMaterial(material);
                particles.GlobalPosition = Position; 
                GetParent().AddChild(particles);*/
            }
            //  }
            
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
        tween.Start();
    }

    private void disappear(){
        tween.StopAll();
        tween.InterpolateProperty(blueLaser, "width", laserWidth, 0, 0.2f);
        tween.InterpolateProperty(greenLaser, "width", laserWidth, 0, 0.2f);
        tween.InterpolateProperty(purpleLaser, "width", laserWidth, 0, 0.2f);
        tween.InterpolateProperty(redLaser, "width", laserWidth, 0, 0.2f);
        tween.Start();
    }

    public void changeWeapon(){
        if (bulletType+1 > 4) bulletType = 1;
        else bulletType++;
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
}
