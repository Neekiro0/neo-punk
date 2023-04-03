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

    public bool isPlayerFacedRight = true;
    public Animator animator;
    
    /*
     * Zmienne lokalne
     */
    private Rigidbody2D playerBody;
    private PlayerInventory playerEq;
    private GameObject camera;
    
    private void Awake()
    {
        // pobieranie rigidbody
        playerBody = GetComponent<Rigidbody2D>();
        playerEq = gameObject.GetComponent<PlayerInventory>();
        isPlayerFacedRight = true;
        camera = transform.Find("Main Camera").gameObject;
    }
    
    private void Update()
    {
        /*
         * przemieszczanie w osi x
         */
        float horizontalInput = Input.GetAxis("Horizontal");
        Debug.Log(horizontalInput);
        transform.Translate(new Vector3(horizontalInput, 0, 0) * playerSpeed * Time.deltaTime);

        /*
         * skakanie
         */
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            jump();
        }
        
        /*
         * Przesyłanie odpowiednich zmiennych do animatora
         */
        animator.SetFloat("PlayerSpeed", Math.Abs(horizontalInput));
        animator.SetFloat("PlayerVelocity", gameObject.GetComponent<Rigidbody2D>().velocity.y );
        
        /*
         * Zmiana kierunku gracza
         */
        if ( Input.GetKeyDown( KeyCode.A ) && isPlayerFacedRight)
        {
            isPlayerFacedRight = false;
        }
        if ( Input.GetKeyDown( KeyCode.D ) && !isPlayerFacedRight)
        {
            isPlayerFacedRight = true;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = !isPlayerFacedRight;
    }

    private void jump()
    {
        Vector2 jumpVector = new Vector2(0, jumpForce * 10);
        float playerBodyVelocity = playerBody.GetPointVelocity(jumpVector).y;
        

        if ( playerBodyVelocity == 0 && !playerEq.isEquipmentShown)
        {
            playerBody.AddForce(jumpVector, ForceMode2D.Impulse);
        }
    }
}