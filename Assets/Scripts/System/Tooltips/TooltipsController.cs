using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace tooltips
{
    
    [System.Serializable]
    public class Tooltip
    {
        public Sprite image = null;
        
        [TextArea(3, 20)]
        public String text = null;
    }

    public class TooltipsController : MonoBehaviour
    {
        public List<Tooltip> tooltips = new List<Tooltip>();
        
        private bool IsTooltipMenuShown;
        private Tooltip shownTooltip = null;
        private Image imageObject = null;
        private TextMeshProUGUI textObject = null;

        private void Awake()
        {
            IsTooltipMenuShown = false;
            imageObject = gameObject.transform.Find("TooltipImage").GetComponent<Image>();
            textObject = gameObject.transform.Find("TooltipText").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            gameObject.SetActive(IsTooltipMenuShown);

            if (IsTooltipMenuShown && null != shownTooltip)
            {
                Time.timeScale = 0;
                // closing tooltip
                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape))
                {
                    CloseTooltip();
                    Time.timeScale = 1;
                }
            }
        }

        // here provide the index of the tooltip you want to show
        public void ShowTooltip(int tooltipNumber)
        {
            
            if (tooltipNumber < tooltips.Count)
            {
                shownTooltip = tooltips[tooltipNumber];
                imageObject.sprite = shownTooltip.image;
                textObject.text = shownTooltip.text;
                
                IsTooltipMenuShown = true;
            }
            else
            {
                Debug.LogError("Tooltip with provided index does not exist.");
            }
        }

        public void CloseTooltip()
        {
            shownTooltip = null;
            imageObject.sprite = null;
            textObject.text = "";
            Time.timeScale = 1;
            IsTooltipMenuShown = false;
        }
    }
}
