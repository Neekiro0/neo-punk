using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    public bool isPlayerNearGround;

    public bool isFloorPassable;

    public GameObject collidingObject;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        collidingObject = collision.gameObject;
        if (collision.CompareTag("impassableFloor"))
        {
            isPlayerNearGround = true;
            isFloorPassable = false;
        }
        else if (collision.CompareTag("passableFloor"))
        {
            isPlayerNearGround = true;
            isFloorPassable = true;
        }
        else
        {
            isPlayerNearGround = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isPlayerNearGround = false;
        isFloorPassable = false;
    }
}
