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
    public float OneStarTime;
    public float TwoStarTime;
    public float ThreeStarTime;
    
    [Header("Takedowns")]
    public int OneStarTakedowns;
    public int TwoStarTakedowns;
    public int ThreeStarTakedowns;
    
    [Header("Damage taken")]
    public float OneStarDamageTaken;
    public float TwoStarDamageTaken;
    public float ThreeStarDamageTaken;
}
