using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OptionsManager
{
    /*
     * Sounds
     */
    private static double MainSoundVolume = 1.00;
    private static double DialogsVolume = 1.00;
    private static double MusicVolume = 1.00;
    private static double EffectsVolume = 1.00;

    public static void SetMainSoundVolume(double volume) { if (volume >= 0.00 && volume <= 1.00) MainSoundVolume = volume; }
    public static void SetDialogsVolume(double volume) { if (volume >= 0.00 && volume <= 1.00) DialogsVolume = volume; }
    public static void SetMusicVolume(double volume) { if (volume >= 0.00 && volume <= 1.00) MusicVolume = volume; }
    public static void SetEffectsVolume(double volume) { if (volume >= 0.00 && volume <= 1.00) EffectsVolume = volume; }
    
    public static double GetMainSoundVolume() { return MainSoundVolume; }
    public static double GetDialogsVolume() { return DialogsVolume; }
    public static double GetMusicVolume() { return MusicVolume; }
    public static double GetEffectsVolume() { return EffectsVolume; }
    
    /*
     * Gameplay
     */
    private static bool ShowTips = true;
    
    public static void SetShowTips(bool value) { ShowTips = value; }
    
    public static bool GetShowTips() { return ShowTips; }
}
