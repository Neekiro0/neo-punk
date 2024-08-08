using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBehaviour : MonoBehaviour
{
    public int NextHubNumber;
    
    public bool IsOpen = false;
    private GameObject player;
    private Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    public void OpenDoor()
    {
        IsOpen = true;
        animator.SetBool("IsOpen", IsOpen );
    }

    public void CloseDoor()
    {
        IsOpen = false;
        animator.SetBool("IsOpen", IsOpen );
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsOpen) OpenDoor();
        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(InputManager.InteractKey))  
            {
                int CurrentSlot = PlayerPrefs.GetInt("SaveSlot");
                int CurrentHub = PlayerPrefs.GetInt("Save"+CurrentSlot.ToString()+"_CurrentHub");
                
                // jeżeli nie odblokowano wcześnie tego huba
                if (CurrentHub < NextHubNumber)
                {
                    PlayerPrefs.SetInt("Save"+CurrentSlot.ToString()+"_CurrentHub", NextHubNumber);
                }
                SceneManager.LoadScene("Hub-"+NextHubNumber.ToString());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        this.CloseDoor();
    }
}
