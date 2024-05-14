using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tooltips
{
    public class tipMoving : MonoBehaviour
    {
        public TooltipsController tooltipsController;
        public int tooltipNumber;
        private bool wasTooltipShown;

        private void Awake()
        {
            wasTooltipShown = false;
            tooltipsController = GameObject.Find("UserInterface").transform.Find("Tooltips")
                .GetComponent<TooltipsController>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!wasTooltipShown && collision.CompareTag("Player"))
            {
                tooltipsController.ShowTooltip(tooltipNumber);
                wasTooltipShown = true;
            }
        }
    }
}

// 
