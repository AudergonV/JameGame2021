using Godot;
using System;

public class Global : Node
{
	private bool[] weapons = {false,false,false,false};
	private bool weaponUnlocked;
	public Menu menu;
	public UI ui;
	public SceneLoader sceneLoader;

	public float sfxVolume = 100f; 
	public float globalVolume = 100f;
	public float musicVolume = 100f;
	public float currentSfxVolume;

	private AudioStreamPlayer music;
	

	public int gravityScale = 10;
	public override void _Ready(){
		ui = GetNode<UI>("UI");
		sceneLoader = GetNode<SceneLoader>("SceneLoader");
		menu = GetNode<Menu>("Menu");
		menu.loadMenu("TitleScreen");
		music = GetNode<AudioStreamPlayer>("Music");
		setFullscreen(true);
		setMusicVolume(0.3f);
		setGlobalVolume(0.7f);
		setSfxVolume(1f);
	}

	public override void _Process(float delta){
		if (Input.IsActionJustPressed("pause") && menu.GetChildCount() == 0){
			togglePause(true);
		}
	}

	public void setMusicVolume(float volume){
		musicVolume = volume;
		updateVolume();
	}

	public void setGlobalVolume(float volume){
		globalVolume = volume;
		updateVolume();
	}

	public void setSfxVolume(float volume){
		sfxVolume = volume;
		updateVolume();
	}

	private void updateVolume(){
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), LinearToDecibel(musicVolume));
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), LinearToDecibel(globalVolume));
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), LinearToDecibel(sfxVolume));
	}

	public void swapWeapon(int weapon){
		ui.setWeapon(weapon);
	}

	public void toggleWeapon(int weapon, bool status = true){
		if (status) weaponUnlocked = true;
		weapons[weapon-1] = status;
	}

	public bool isWeaponEnabled(int weapon){
		return weapons[weapon-1];
	}

	public bool isWeaponUnlocked(){
		return weaponUnlocked;
	}

	public void setFullscreen(bool fullscreen){
		OS.WindowFullscreen = fullscreen;
	}

	private float LinearToDecibel(float linear)
     {
         float dB;
         if (linear != 0)
             dB = 20.0f * (float)Math.Log10(linear);
         else
             dB = -144.0f;
         return dB;
     }

	 private float DecibelToLinear(float dB)
     {
         float linear = Mathf.Pow(10.0f, dB/20.0f);
 
         return linear;
     }

	public void togglePause(bool pause){
		GetTree().Paused = pause;
		if (pause) menu.loadMenu("Pause");
		else menu.clear();
	}

	public void restart(){
		sceneLoader.clear();
		menu.loadMenu("TitleScreen");
		weapons = new bool[]{false,false,false,false};
		weaponUnlocked = false;
	}

}
