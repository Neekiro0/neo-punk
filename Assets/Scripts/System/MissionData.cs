using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "NeoPunk/Mission")]
public class MissionData : ScriptableObject
{
    public string MissionName;
    public string Objective;
    public string Description;
    public string SceneName;
    
    [Header("Time")]
    public double OneStarTime;
    public double TwoStarTime;
    public double ThreeStarTime;
    
    [Header("Takedowns")]
    public int OneStarTakedowns;
    public int TwoStarTakedowns;
    public int ThreeStarTakedowns;
    
    [Header("Damage taken")]
    public int OneStarDamageTaken;
    public int TwoStarDamageTaken;
    public int ThreeStarDamageTaken;
    
    public MissionScript missionScript;

    public interface MissionScript
    {
        void Script(){}
    }
}
