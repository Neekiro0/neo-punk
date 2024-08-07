using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public Animator animator;
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        animator = GameObject.Find("Main Camera").gameObject.transform.Find("Background").GetComponent<Animator>();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ExitHover(){ animator.SetBool("exitHover", true); }
    public void ExitHoverExit(){ animator.SetBool("exitHover", false); }
    public void PlayHover(){ animator.SetBool("playHover", true); }
    public void PlayHoverExit(){ animator.SetBool("playHover", false); }
}
