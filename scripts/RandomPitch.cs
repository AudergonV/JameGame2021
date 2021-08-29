using Godot;
using System;

public class RandomPitch : AudioStreamPlayer2D
{

    public override void _Ready()
    {
        PitchScale = (float) new Random().NextDouble()+0.7f;
    }


}
