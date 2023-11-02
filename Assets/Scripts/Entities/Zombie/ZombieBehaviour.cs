using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{[SerializeField] public Transform[] Positions;
    public float EntitySpeed;

    public int NextPositionIndex;
    public Transform NextPosition;
    public EntityStatus entityStatus;
    private Vector3 playerVector3;
    private bool isChasingPlayer;
    private float previousPlayerDetectorRange;
    private GameObject playerDetector;
    private Vector3 previousPosition;
    public float currentSpeed;
    public bool isPlayerInAttackRange;
    public float distanceToPlayer;
    private Animator Animator;
    
    void Start()
    {
        if (Positions.Length > 0) NextPosition = Positions[0];
        entityStatus = gameObject.GetComponent<EntityStatus>();
        EntitySpeed = entityStatus.GetMovementSpeed();

        playerDetector = gameObject.transform.Find("PlayerDetector").gameObject;
        previousPlayerDetectorRange = playerDetector.GetComponent<CircleCollider2D>().radius;
    }
    
    void  Update()
    {
        if (!isPlayerInAttackRange) MoveZombie();
        
        /*
         * Obliczanie prędkości aktualnej, oraz kierunku ruchu zombie
         */
        Vector3 currentPosition = transform.position;
        Vector3 displacement = transform.position - previousPosition;
        Vector3 speedVector = displacement / Time.deltaTime;
        currentSpeed = speedVector.x;
        previousPosition = currentPosition;
        if (currentSpeed > 0)
        {
            entityStatus.isFacedRight = true;
        }
        else if (currentSpeed < 0)
        {
            entityStatus.isFacedRight = false;
        }
        
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
        if (isChasingPlayer)
        {
            distanceToPlayer = Vector3.Distance(transform.position, entityStatus.detectedTarget.transform.position);
            isPlayerInAttackRange = ( distanceToPlayer < entityStatus.attackRange );
            if (isPlayerInAttackRange)
            {
                Debug.Log("Atakuje!!");
                //Animator.SetFloat("ChargingTime", keyHoldTime );
            }
        }
        else
        {
            isPlayerInAttackRange = false;
        }
    }

    void MoveZombie()
    {
        if (isChasingPlayer && playerDetector)
        {
            playerDetector.GetComponent<CircleCollider2D>().radius = previousPlayerDetectorRange * 1.8f;
        }
        /*else
        {
            playerDetector.GetComponent<CircleCollider2D>().radius = previousPlayerDetectorRange;
        }*/
        
        // Niezakłócony ruch dopóki nie wykryto gracza
        if (!entityStatus.detectedTarget)
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
