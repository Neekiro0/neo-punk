using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 3, 0);
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject entity = col.gameObject;
        if (entity.CompareTag("Player"))
        {
            entity.GetComponent<EntityStatus>().AddXp(1000);
            entity.GetComponent<EntityStatus>().SetGold( entity.GetComponent<EntityStatus>().GetGold()+1 );
            
        }
    }
}
