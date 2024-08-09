using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotController : MonoBehaviour
{
    private int SaveSlot = 0;
    public bool isShown = false;

    private void Start()
    {
        SaveSlot = 0;
    }

    public void SelectSave(int slot)
    {
        if (slot >= 0 && slot <= 3)
        {
            SaveSlot = slot;
            PlayerPrefs.SetInt("SaveSlot", slot);
            int CurrentHub = PlayerPrefs.GetInt("Save"+slot.ToString()+"_CurrentHub", 0);

            // loading tutorial if player didn't reach any hub
            if (CurrentHub == 0)
            {
                SceneManager.LoadScene("Tutorial");
            }
            else
            {
                SceneManager.LoadScene("Hub-"+CurrentHub.ToString());
            }
        }
    }

    public void CloseSlotPicker()
    {
        isShown = false;
        gameObject.SetActive(false);
    }
    
    public void ShowSlotPicker()
    {
        isShown = true;
        gameObject.SetActive(true);
    }
}
