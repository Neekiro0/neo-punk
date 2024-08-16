using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionTracker : MonoBehaviour
{
    public string currentMissionName;

    public int NextHubNumber;
    public bool isWatchingScore = true;
    
    [Header("Mission stats tracker")]
    public float time = 0;
    public int takedowns = 0;
    public float damageTaken = 0;

    public void CalculateScoreAndProceed()
    {
        MissionData currentMissionData = Resources.Load<MissionData>("Missions/"+currentMissionName+"/MissionData");
        if (currentMissionData)
        {
            isWatchingScore = false;
        
            int CurrentSlot = PlayerPrefs.GetInt("SaveSlot");
            int CurrentHub = PlayerPrefs.GetInt("Save"+CurrentSlot.ToString()+"_CurrentHub");
            float BestTime = PlayerPrefs.GetFloat("Save" + CurrentSlot.ToString() + "_" + currentMissionName + "_BestTime", 0.0f);
            int BestTakedowns = PlayerPrefs.GetInt("Save" + CurrentSlot.ToString() + "_" + currentMissionName + "_BestTakedowns", 0);
            float BestDamageTaken = PlayerPrefs.GetFloat("Save" + CurrentSlot.ToString() + "_" + currentMissionName + "_BestDamageTaken", 0.0f);
        
            /*
             * Zapisywanie wyników
             */
            if (time < BestTime || BestTime == 0.0f )
            {
                // Zapisanie najlepszego czasu
                PlayerPrefs.SetFloat("Save" + CurrentSlot.ToString() + "_" + currentMissionName + "_BestTime", time);
            }
            if (takedowns > BestTakedowns)
            {
                // Zapisanie najlepszej ilości zabójstw
                PlayerPrefs.SetInt("Save" + CurrentSlot.ToString() + "_" + currentMissionName + "_BestTakedowns", takedowns);
            }

            if (damageTaken < BestDamageTaken || BestDamageTaken == 0.0f)
            {
                // zapisanie najmniejszych przyjętych obrażeń
                PlayerPrefs.SetFloat("Save" + CurrentSlot.ToString() + "_" + currentMissionName + "_BestDamageTaken", damageTaken);
            }
            // TODO: dorobić ekran końcowy misji
                
            // jeżeli nie odblokowano wcześnie tego huba
            if (CurrentHub < NextHubNumber)
            {
                PlayerPrefs.SetInt("Save"+CurrentSlot.ToString()+"_CurrentHub", NextHubNumber);
            }
            SceneManager.LoadScene("Hub-"+NextHubNumber.ToString());
        }
        else
        {
            Debug.LogError("Not found mission named: "+currentMissionName);
        }
        
    }

    private void Update()
    {
        if (isWatchingScore)
        {
            time = (float)Time.timeSinceLevelLoadAsDouble;
        }
    }

    public void AddTakedown()
    {
        if (isWatchingScore) takedowns += 1;
    }

    public void AddDamageTaken(float damage)
    {
        if (isWatchingScore) damageTaken += damage;
    }
}
