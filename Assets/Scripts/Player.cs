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
    public bool isGrounded = false; // Czy dotyka ziemii
    private float attackTimeout = 1f; // Czas na zakończenie sekwencji ataku
    private float lastAttackTime = 0; // aktualny pomiędzy atakami
    private float attackCooldown = 0.5f; // cooldown ponmiędzy atakami
    private Coroutine attackCoroutine;
    private AttackSequence currentAttack = AttackSequence.First;
    private FloorDetector FloorDetector;
    public LayerMask passThroughLayer;
    private bool isPassingThrough = false;
    private Collider2D ignoredObject;

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
        FloorDetector = transform.Find("FloorDetector").gameObject.GetComponent<FloorDetector>();
    }


    private void Update()
    {
        isGrounded = FloorDetector.isPlayerNearGround;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        
        /*
         * Przesyłanie odpowiednich zmiennych do animatora
         */
        animator.SetFloat("PlayerSpeed", Math.Abs(horizontalInput));
        animator.SetFloat("PlayerVelocity", gameObject.GetComponent<Rigidbody2D>().velocity.y );
        animator.SetInteger("PlayerAttackState", attackState );
        animator.SetBool("IsPlayerAttacking", isAttacking );
        animator.SetBool("IsGrounded", isGrounded );
        
        /*
         * przemieszczanie w osi x
         */
        if (!isAttacking)
        {
            transform.Translate(new Vector3(horizontalInput, 0, 0) * playerSpeed * Time.deltaTime);
        }

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
        if (  Input.GetKeyDown(InputManager.MoveRightKey) && !isPlayerFacedRight && (Time.timeScale != 0 ))
        {
            isPlayerFacedRight = true;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = !isPlayerFacedRight;
        
        /*
         * Atak
         */
        if ( Input.GetKeyDown(InputManager.AttackKey) && isGrounded/*|| Input.GetKeyDown(InputManager.PadButtonAttack)*/ )
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
        /*
         * Przejście przez podłoże
         */
        if ( FloorDetector.isFloorPassable && isGrounded && Input.GetKeyDown(InputManager.MoveDownKey) )
        {
            DisableCollisionForDuration(0.3f);
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
    
    private void DisableCollisionForDuration(float duration)
    {
        ignoredObject = FloorDetector.collidingObject.GetComponent<Collider2D>();
        
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignoredObject, true);
        Invoke("EnableCollision", duration);
    }

    // Włączenie kolizji ponownie.
    private void EnableCollision()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignoredObject, false);
        ignoredObject = null;
    }

    private void StartAttack()
    {
        attackCoroutine = StartCoroutine(AttackTimeout());
        isAttacking = true;
        currentAttack = AttackSequence.First;
        attackState = 1;
        movePlayerOnAttack(0.3f);
    }

    private void ContinueAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackTimeout());

        // Sprawdź, czy minęło wystarczająco dużo czasu między atakami
        if (Time.time - lastAttackTime >= attackCooldown)
        {
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
                movePlayerOnAttack(0.3f);
            }

            // Aktualizuj czas ostatniego ataku
            lastAttackTime = Time.time;
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

    private void movePlayerOnAttack(float howFar)
    {
        // delikatne przesunięcie gracza po ataku
        if (isPlayerFacedRight)
        { transform.Translate(new Vector3(1.0f, 0, 0) * howFar * 100 * Time.deltaTime); }
        else { transform.Translate(new Vector3(-1.0f, 0, 0) * howFar * 100 * Time.deltaTime); }
    }
}