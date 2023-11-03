using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    [SerializeField] public Transform[] Positions;
    private float EntitySpeed;

    private int NextPositionIndex;
    private Transform NextPosition;
    private EntityStatus entityStatus;
    private Vector3 playerVector3;
    private bool isChasingPlayer;
    private float previousPlayerDetectorRange;
    private CircleCollider2D playerDetector;
    private Vector3 previousPosition;
    public float currentSpeed;
    public bool isPlayerInAttackRange;
    public float distanceToPlayer;
    private Animator animator;
    public LayerMask warstwaPrzeszkod;
    private bool didRaycastFoundPlayer = false;
    public bool isAttacking = false;
    private bool hasTouchedPlayer = false; // Zmienna, która będzie przechowywać informację o dotknięciu gracza

    void Start()
    {
        if (Positions.Length > 0) NextPosition = Positions[0];
        entityStatus = gameObject.GetComponent<EntityStatus>();
        EntitySpeed = entityStatus.GetMovementSpeed();

        playerDetector = gameObject.transform.Find("PlayerDetector").gameObject.GetComponent<CircleCollider2D>();
        previousPlayerDetectorRange = playerDetector.radius;
        animator = gameObject.GetComponent<Animator>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasTouchedPlayer)
        {
            EntityStatus playerStatus = collision.gameObject.GetComponent<EntityStatus>();
            playerStatus.DealDamage(entityStatus.GetAttackDamageCount());
        }
    }
    
    void  Update()
    {
        if (!isAttacking) MoveZombie();
        
        /*
         * Obliczanie prędkości aktualnej, oraz kierunku ruchu zombie
         */
        var position = transform.position;
        Vector3 currentPosition = position;
        Vector3 displacement = position - previousPosition;
        Vector3 speedVector = displacement / Time.deltaTime;
        currentSpeed = speedVector.x;
        previousPosition = currentPosition;
        
        if (currentSpeed > 0) entityStatus.isFacedRight = true; 
        else if (currentSpeed < 0) entityStatus.isFacedRight = false; 
        
        /*
         * Obracanie 
         */
        if (!entityStatus.isFacedRight)
        {
            Vector3 newRotation = transform.eulerAngles;
            newRotation.y = 180;
            transform.eulerAngles = newRotation;
        }
        else
        {
            Vector3 newRotation = transform.eulerAngles;
            newRotation.y = 0;
            transform.eulerAngles = newRotation;
        }

        /*
         * Atak podstawowy na gracza
         */
        if (isChasingPlayer && entityStatus.detectedTarget )
        {
            distanceToPlayer = Vector3.Distance(transform.position, entityStatus.detectedTarget.transform.position);
            isPlayerInAttackRange = ( distanceToPlayer < entityStatus.attackRange );
            
            // Atak na gracza
            if (isPlayerInAttackRange && !isAttacking)
            {
                StartCoroutine(PerformAttack());
            }
        }
        else isPlayerInAttackRange = false; 
        
        /*
         * Raycast
         */
        if (entityStatus.detectedTarget)
        {
            Vector2 raycastOrigin = transform.position;

            // Pozycja gracza
            Vector2 playerPosition = entityStatus.detectedTarget.transform.position;

            // Kierunek raycasta od obiektu do gracza
            Vector2 raycastDirection =  playerPosition - raycastOrigin;

            // Długość raycasta
            float raycastDistance = raycastDirection.magnitude;

            // Normalizacja kierunku raycasta
            raycastDirection.Normalize();

            // Wykonaj raycast w kierunku gracza
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection, raycastDistance, warstwaPrzeszkod);

            // Jeśli raycast trafiał w przeszkodę
            if (hit.collider != null)
            {
                // Sprawdź, czy trafiony obiekt ma tag "Player"
                didRaycastFoundPlayer = hit.collider.CompareTag("Player");
            }
            else
            {
                didRaycastFoundPlayer = false;
            }
        }
        
    }

    void Attack()
    {
        Rigidbody2D zombieRigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (entityStatus.detectedTarget.transform.position.x < gameObject.transform.position.x)
        {
            Vector3 movement = new Vector3(-1 * entityStatus.attackRange * 500, 700, 0);
            zombieRigidbody.AddForce(movement);
        }
        else
        {
            Vector3 movement = new Vector3(1 * entityStatus.attackRange * 500, 700, 0);
            zombieRigidbody.AddForce(movement);
        }
        
        //if (zombieRigidbody.)
        animator.Play("Idle");
    }
    
    /*IEnumerator AttackCourutine(int direction)
    {
        isAttacking = true;
        
        
        animator.Play("Idle");
        yield return new WaitForSeconds(2.0f);
        
        isAttacking = false;
    }*/
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(2f); // Czas cooldownu (2 sekundy w przykładzie)

        isAttacking = false;
    }
    private IEnumerator PerformAttack()
    {
        while (isPlayerInAttackRange)
        {
            animator.Play("ZombieAttackAnimation");
            isAttacking = true;
            Attack(); // Wywołaj atak
            animator.Play("Idle");

            // Rozpocznij coroutine do obsługi cooldownu
            StartCoroutine(AttackCooldown());

            yield return new WaitForSeconds(2f); // Poczekaj na zakończenie ataku
            isAttacking = false;
        }
    }

    void MoveZombie()
    {
        if (isChasingPlayer && playerDetector && didRaycastFoundPlayer)
        {
            playerDetector.radius = previousPlayerDetectorRange * 1.6f;
        }
        
        // Niezakłócony ruch dopóki nie wykryto gracza
        if ( ( !didRaycastFoundPlayer && entityStatus.detectedTarget) || ( !entityStatus.detectedTarget && !entityStatus.detectedTarget ) )
        {
            isChasingPlayer = false;
            distanceToPlayer = 0;
            if (transform.position == NextPosition.position )
            {
                NextPositionIndex++;
                if (NextPositionIndex >= Positions.Length)
                {
                    NextPositionIndex = 0;
                }
                NextPosition = Positions[NextPositionIndex];
                entityStatus.isFacedRight = !entityStatus.isFacedRight;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, NextPosition.position, EntitySpeed * Time.deltaTime);
            }
        }
        else
        {
            // Ruch po wykryciu gracza
            isChasingPlayer = true;
            playerVector3 = entityStatus.detectedTarget.transform.position;
            var position = transform.position;
            position = Vector3.MoveTowards(position, new Vector3(playerVector3.x, position.y, position.z), EntitySpeed * Time.deltaTime);
            transform.position = position;
        }
    }
}
