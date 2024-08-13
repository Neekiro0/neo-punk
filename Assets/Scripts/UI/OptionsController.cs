using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public bool OptionsShown = false;
    public String objectToHideName;
    
    private GameObject ObjectToHide;
    private GameObject SettingsPagesContent;
    
    // Start is called before the first frame update
    void Start()
    {
        ObjectToHide = GameObject.Find(objectToHideName);
        SettingsPagesContent =
            gameObject.transform.Find("Scroll View").gameObject.transform.Find("Viewport").gameObject.transform.Find("Content").gameObject;
        gameObject.SetActive(false);
        if ( null != ObjectToHide) ObjectToHide.SetActive(true);
        
        GetSettingsFromMemory();
        ShowSettingsPage("Sound");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HideOptions()
    {
        OptionsShown = false;
        gameObject.SetActive(false);
        if ( null != ObjectToHide) ObjectToHide.SetActive(true);
    }
    
    public void ShowOptions()
    {
        OptionsShown = true;
        gameObject.SetActive(true);
        if ( null != ObjectToHide) ObjectToHide.SetActive(false);
    }

    public void SettingShowTips()
    {
        OptionsManager.SetShowTips(!OptionsManager.GetShowTips());
    }

    public void ShowSettingsPage(string name)
    {
        GameObject pageToActivate = SettingsPagesContent.transform.Find(name).gameObject;
        
        for (int i = 0; i < SettingsPagesContent.transform.childCount; i++)
        {
            var child = SettingsPagesContent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                if (child.name == name)
                {
                    child.SetActive(true);
                }
                else
                {
                    child.SetActive(false);
                }
            }
        }

    }

    private void GetSettingsFromMemory()
    {
        GameObject GameplayPage = SettingsPagesContent.transform.Find("Gameplay").gameObject;
        if (null != GameplayPage)
        {
            GameObject tips = GameplayPage.transform.Find("Tips").gameObject;
            if (null != tips)
            {
                tips.transform.Find("Toggle").gameObject.GetComponent<Toggle>().isOn = OptionsManager.GetShowTips();
            }
        }
    }

    public void SaveTips()
    {
        GameObject GameplayPage = SettingsPagesContent.transform.Find("Gameplay").gameObject;
        if (null != GameplayPage)
        {
            GameObject tips = GameplayPage.transform.Find("Tips").gameObject;
            if (null != tips)
            {
                OptionsManager.SetShowTips(tips.transform.Find("Toggle").gameObject.GetComponent<Toggle>().isOn);
            }
        }
    }
}
