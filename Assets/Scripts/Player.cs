using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    /*
     * Zmienne dostępne w edytorze
     */
    [Tooltip("Set player speed, between 0 and 10000.")]
    [Range(0, 100)]
    public float playerSpeed = 20f;
    
    [Tooltip("Set player jump force, between 0 and 10000.")]
    [Range(0, 30)]
    public float jumpForce = 6f;

    /*
     * Zmienne lokalne
     */
    private Rigidbody2D playerBody;
    
    private void Awake()
    {
        // pobieranie rigidbody
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        /*
         * przemieszczanie w osi x
         */
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(horizontalInput, 0, 0) * playerSpeed * Time.deltaTime);

        /*
         * skakanie
         */
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            jump();
        }
    }

    private void jump()
    {
        Vector2 jumpVector = new Vector2(0, jumpForce * 10);
        float playerBodyVelocity = playerBody.GetPointVelocity(jumpVector).y;
        

        if ( playerBodyVelocity == 0 )
        {
            playerBody.AddForce(jumpVector, ForceMode2D.Impulse);
        }
    }
}