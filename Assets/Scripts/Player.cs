using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public int attackState = 0; // Aktualny stan ataku
    public bool isAttacking = false; // Czy trwa atak
    private float attackTimeout = 0.8f; // Czas na zakończenie sekwencji ataku
    private Coroutine attackCoroutine;
    private AttackSequence currentAttack = AttackSequence.First;

    private enum AttackSequence
    {
        First,
        Second,
        Third
    }
    
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
        //Debug.Log(horizontalInput);
        transform.Translate(new Vector3(horizontalInput, 0, 0) * playerSpeed * Time.deltaTime);
        
        /*
         * Przesyłanie odpowiednich zmiennych do animatora
         */
        animator.SetFloat("PlayerSpeed", Math.Abs(horizontalInput));
        animator.SetFloat("PlayerVelocity", gameObject.GetComponent<Rigidbody2D>().velocity.y );

        /*
         * skakanie
         */
        if (Input.GetKeyDown(InputManager.JumpKey) || Input.GetButtonDown(InputManager.PadButtonJump))
        {
            Jump();
        }

        /*
         * Zmiana kierunku gracza
         */
        if ( Input.GetKeyDown(InputManager.MoveLeftKey) && isPlayerFacedRight && (Time.timeScale != 0 ))
        {
            isPlayerFacedRight = false;
        }
        if ( Input.GetKeyDown(InputManager.MoveRightKey) && !isPlayerFacedRight && (Time.timeScale != 0 ))
        {
            isPlayerFacedRight = true;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = !isPlayerFacedRight;
        
        /*
         * Atak
         */
        if ( Input.GetKeyDown(InputManager.AttackKey) /*|| Input.GetKeyDown(InputManager.PadButtonAttack)*/ )
        {
            if (!isAttacking)
            {
                // Rozpocznij atak od początku sekwencji
                StartAttack();
            }
            else
            {
                // Gracz kontynuuje sekwencję ataku
                ContinueAttack();
            }
        }
    }

    private void Jump()
    {
        Vector2 jumpVector = new Vector2(0, jumpForce * 10);
        float playerBodyVelocity = playerBody.GetPointVelocity(jumpVector).y;

        if ( playerBodyVelocity == 0 && !playerEq.isEquipmentShown)
        {
            playerBody.AddForce(jumpVector, ForceMode2D.Impulse);
        }
    }

    private void StartAttack()
    {
        attackCoroutine = StartCoroutine(AttackTimeout());
        isAttacking = true;
        currentAttack = AttackSequence.First;
        attackState = 1;
    }

    private void ContinueAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackTimeout());

        if (currentAttack == AttackSequence.Third)
        {
            // Gracz zaczyna nową sekwencję ataku
            currentAttack = AttackSequence.First;
            attackState = 1;
        }
        else
        {
            // Kontynuuj sekwencję ataku
            currentAttack++;
            attackState++;
        }
    }

    private IEnumerator AttackTimeout()
    {
        yield return new WaitForSeconds(attackTimeout);

        // Przerwanie ataku po timeout
        isAttacking = false;
        attackState = 0;
        currentAttack = AttackSequence.First;
    }
}