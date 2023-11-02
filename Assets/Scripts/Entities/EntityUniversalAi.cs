using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityUniversalAi : MonoBehaviour
{
    [SerializeField] public Transform[] Positions;
    public float EntitySpeed;

    public int NextPositionIndex;
    public Transform NextPosition;
    public EntityStatus entityStatus;
    // Start is called before the first frame update
    void Start()
    {
        if (Positions.Length > 0) NextPosition = Positions[0];
        entityStatus = gameObject.GetComponent<EntityStatus>();
        EntitySpeed = entityStatus.GetMovementSpeed();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Positions.Length > 0) MoveEntity();
    }

    public virtual void MoveEntity()
    {
        if (transform.position == NextPosition.position)
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
}
