using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinBehaviour : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(0, 3, 0);
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject entity = col.gameObject;
        if (entity.CompareTag("Player"))
        {
            entity.GetComponent<EntityStatus>().AddXp(13);
            entity.GetComponent<EntityStatus>().AddGold(1);
            Destroy(gameObject);
        }
    }
}
