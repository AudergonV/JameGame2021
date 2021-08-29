using Godot;
using System;

public class Settings : Control
{
    private Global global;
    private Slider musicSlider;
    private Slider sfxSlider;
    private Slider globalSlider;

    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
        musicSlider = GetNode<Slider>("GridContainer/PanelContainer/MusicSlider");
        sfxSlider = GetNode<Slider>("GridContainer/PanelContainer2/SFXSlider");
        globalSlider = GetNode<Slider>("GridContainer/PanelContainer3/GlobalSlider");
        musicSlider.Value = global.musicVolume;
        sfxSlider.Value = global.sfxVolume;
        globalSlider.Value = global.globalVolume;
    }

    public void OnBackButtonPressed(){
        global.menu.loadMenu(global.menu.backMenu);
    }

    public void OnFullscreenToggled(bool fullscreen){
        global.setFullscreen(fullscreen);
    }

    public void OnMusicSliderValueChanged(float value){
        global.setMusicVolume(value);
    }

    public void OnSFXSliderValueChanged(float value){
        global.setSfxVolume(value);
    }

    public void OnGlobalSliderValueChanged(float value){
        GD.Print(value);
        global.setGlobalVolume(value);
    }


}
