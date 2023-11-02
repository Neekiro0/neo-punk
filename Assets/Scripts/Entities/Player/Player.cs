using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    /*
     * Zmienne dostępne w edytorze
     */
    [Tooltip("Set player jump force, between 0 and 10000.")]
    [Range(0, 30)]
    public float jumpForce = 6f;

    public Animator animator;
    public int attackState = 0; // Aktualny stan ataku
    public bool isAttacking = false; // Czy trwa atak
    public bool isGrounded = false; // Czy dotyka ziemii

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
    private bool isPassingThrough = false;
    private Collider2D ignoredObject;
    private CapsuleCollider2D boxCollider;
    private GameObject rightSwordHitbox;
    private GameObject leftSwordHitbox;
    private float playerCurrentSpeed = 1;
    private Vector3 previousPosition;
    private float attackTimeout = 0.8f; // Czas na zakończenie sekwencji ataku
    private float lastAttackTime = 0; // aktualny pomiędzy atakami
    private float attackCooldown = 0.4f; // cooldown ponmiędzy atakami
    private Coroutine attackCoroutine;
    private AttackSequence currentAttack = AttackSequence.First;
    private FloorDetector FloorDetector;
    private bool isChargingAttack = false;
    private float keyHoldTime = 0.0f;
    float holdTimeThreshold = 1.7f;
    private EntityStatus playerStatus;
    
    private void Awake()
    {
        // pobieranie rigidbody
        playerBody = GetComponent<Rigidbody2D>();
        playerEq = gameObject.GetComponent<PlayerInventory>();
        camera = transform.Find("Main Camera").gameObject;
        FloorDetector = transform.Find("FloorDetector").gameObject.GetComponent<FloorDetector>();
        boxCollider = GetComponent<CapsuleCollider2D>();
        playerStatus = GetComponent<EntityStatus>();
        GameObject swordHitboxes = transform.Find("SwordHitboxes").gameObject;
        if (swordHitboxes)
        {
            rightSwordHitbox = swordHitboxes.transform.Find("RightHitbox").gameObject;
            leftSwordHitbox = swordHitboxes.transform.Find("LeftHitbox").gameObject;
        }
    }


    private void Update()
    {

        isGrounded = ( boxCollider.GetContacts(new ContactPoint2D[16]) > 0 );
        
        float horizontalInput = Input.GetAxis("Horizontal");
        
        /*
         * Przesyłanie odpowiednich zmiennych do animatora
         */
        animator.SetFloat("PlayerSpeed", Mathf.Abs(playerCurrentSpeed));
        animator.SetFloat("PlayerVelocity", playerBody.velocity.y );
        animator.SetInteger("PlayerAttackState", attackState );
        animator.SetBool("IsPlayerAttacking", isAttacking );
        animator.SetBool("IsGrounded", isGrounded );
        animator.SetBool("IsChargingAttack", isChargingAttack );
        animator.SetFloat("ChargingTime", keyHoldTime );
        
        if (isAttacking && (horizontalInput < 0 && playerStatus.isFacedRight))
        {
            horizontalInput = 0;
        }
        if (isAttacking && (horizontalInput > 0 && !playerStatus.isFacedRight) )
        {
            
            horizontalInput = 0;
        }
        
        /*
         * przemieszczanie w osi x
         */
        if ( keyHoldTime < 0.3f )
        {
            transform.Translate(new Vector3(horizontalInput, 0, 0) * playerStatus.GetMovementSpeed() * Time.deltaTime);
        }

        // Oblicz prędkość poruszania się na podstawie zmiany pozycji między klatkami.
        Vector3 currentPosition = transform.position;
        Vector3 displacement = transform.position - previousPosition;
        Vector3 speedVector = displacement / Time.deltaTime;
        playerCurrentSpeed = speedVector.x;
        previousPosition = currentPosition;
        
        /*
         * Nieznaczne wydłużanie hitboxa ataków podczas biegu
         */
        if ( (Mathf.Abs(playerCurrentSpeed) > 3) || ( horizontalInput != 0 && isChargingAttack) )
        {
            rightSwordHitbox.GetComponent<BoxCollider2D>().size = new Vector2(1.4f, 0.25f);
            leftSwordHitbox.GetComponent<BoxCollider2D>().size = new Vector2(1.4f, 0.25f);
        }
        else
        {
            rightSwordHitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.35f, 0.25f);
            leftSwordHitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.35f, 0.25f);
        }

        /*
         * skakanie
         */
        if ( (Input.GetKeyDown(InputManager.JumpKey) || Input.GetButtonDown(InputManager.PadButtonJump) ) && !isAttacking)
        {
            Jump();
        }

        /*
         * Zmiana kierunku gracza
         */
        if (  playerCurrentSpeed > 0.1 )
        {
            playerStatus.isFacedRight = true;
        }
        else if (  playerCurrentSpeed < -0.1 )
        {
            playerStatus.isFacedRight = false;
        }
        
        gameObject.GetComponent<SpriteRenderer>().flipX = !playerStatus.isFacedRight;
        
        
        if (Input.GetKey(InputManager.AttackKey) && !isAttacking)
        {
            if (keyHoldTime < holdTimeThreshold)
            {
                keyHoldTime += Time.deltaTime;
                isChargingAttack = true;
            }
        }

        if (Input.GetKeyUp(InputManager.AttackKey))
        {
            if (isChargingAttack)
            {
                if (keyHoldTime >= holdTimeThreshold)
                {
                    PerformChargeAttack();
                }
                else
                {
                    // Rozpocznij atak od początku sekwencji
                    StartAttack();
                }
            }
            else if (isGrounded && isAttacking)
            {
                // Gracz kontynuuje sekwencję ataku
                ContinueAttack();
            }
            isChargingAttack = false;
            keyHoldTime = 0.0f;
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
    private void PerformChargeAttack()
    {
        if (isChargingAttack && ( keyHoldTime >= holdTimeThreshold ) )
        {
            //Debug.Log(keyHoldTime.ToString() + "....." + holdTimeThreshold.ToString());
            if ( Input.GetAxis("Horizontal") != 0 )
            {
                DealDamage( playerStatus.GetAttackDamageCount() * 2 );
            }
            else
            {
                DealDamage( playerStatus.GetAttackDamageCount() * 3 );
            }
            animator.Play("heavyAttack_2");
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

    private void DealDamage(float damageToDeal)
    {
        if (playerStatus.isFacedRight)
        {
            // sprawdzanie czy gracz atakuje przeciwnika
            GameObject collidingObject = rightSwordHitbox.gameObject.GetComponent<HitboxBehaviour>().collidingEnemyObject;
            if (collidingObject)
            {
                // zadawanie obrażeń
                collidingObject.GetComponent<EntityStatus>().DealDamage(damageToDeal);
                rightSwordHitbox.gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
        else if (!playerStatus.isFacedRight)
        {
            // sprawdzanie czy gracz atakuje przeciwnika
            GameObject collidingObject = leftSwordHitbox.gameObject.GetComponent<HitboxBehaviour>().collidingEnemyObject;
            if (collidingObject)
            {
                // zadawanie obrażeń
                collidingObject.GetComponent<EntityStatus>().DealDamage(damageToDeal);
                leftSwordHitbox.gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private void StartAttack()
    {
        attackCoroutine = StartCoroutine(AttackTimeout());
        isAttacking = true;
        currentAttack = AttackSequence.First;
        attackState = 1;
        animator.Play("Attack_1");
        DealDamage(playerStatus.GetAttackDamageCount());
        movePlayerOnAttack(0.5f);
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
                
                if ( attackState != 0 ) animator.Play("Attack_"+attackState.ToString());
                
                DealDamage(playerStatus.GetAttackDamageCount() * 1.2f);
                movePlayerOnAttack(0.5f);
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
        if (playerStatus.isFacedRight)
        {
            Vector3 movement = new Vector3(1.0f * howFar * 1000, 0, 0);
            playerBody.AddForce(movement);
        }
        else
        {
            Vector3 movement = new Vector3(-1.0f * howFar * 1000, 0, 0);
            playerBody.AddForce(movement);
        }
    }
}