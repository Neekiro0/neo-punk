using System;
using System.Collections;
using System.Collections.Generic;
using tooltips;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /*
     * Zmienna listowa przechowująca aktualne typy obrażeń
     */
    [Serializable]
    public class ElementalType
    {
        [SerializeField]
        public string name;
        public Sprite icon;
        public Color elementalColor;
    }
    
    [Header("Elemental Types")]
    public List<ElementalType> ElementalTypes = new List<ElementalType>();
  
    public int UsedElementalTypeId = 0;
    [ReadOnly]
    public String UsedElementalName = "Normal";

    /*
     * Zmienne dostępne w edytorze
     */
    [Header("Player variables")]
    public float jumpForce = 6f;

    public Animator animator;
    public int attackState = 0; // Aktualny stan ataku
    public bool isAttacking = false; // Czy trwa atak
    public bool isGrounded = false; // Czy dotyka ziemii

    /*
     * Zmienne lokalne
     */
    private Rigidbody2D playerBody;
    private PlayerInventory playerEq;
    private bool isPassingThrough = false;
    private Collider2D ignoredObject;
    private CapsuleCollider2D boxCollider;
    private GameObject swordHitbox;
    private Vector3 previousPosition;
    private float attackTimeout = 0.6f; // Czas na zakończenie sekwencji ataku
    private float lastAttackTime = 0; // aktualny pomiędzy atakami
    private float attackCooldown = 0.3f; // cooldown ponmiędzy atakami
    private Coroutine attackCoroutine;
    private FloorDetector FloorDetector;
    private bool isChargingAttack = false;
    private float keyHoldTime = 0.0f;
    float holdTimeThreshold = 1.7f;
    private EntityStatus playerStatus;
    [SerializeField] private float parryWindow;
    public bool isBlocking;
    public bool isParrying;
    public float cooldownBetweenBlocks;
    private bool canBlock = true;
    private PauseMenuBehaviour pauseMenu;
    private GameObject elementalIconObject;
    public Vector3 lastSafePosition;
    public float playerVoidLevel;
    
    
    private void Awake()
    {
        // pobieranie rigidbody
        playerBody = GetComponent<Rigidbody2D>();
        playerEq = gameObject.GetComponent<PlayerInventory>();
        FloorDetector = transform.Find("FloorDetector").gameObject.GetComponent<FloorDetector>();
        boxCollider = GetComponent<CapsuleCollider2D>();
        playerStatus = GetComponent<EntityStatus>();
        swordHitbox = transform.Find("SwordHitbox").gameObject;
        animator = GetComponent<Animator>();
        
        pauseMenu = GameObject.Find("Pause Menu Interface").GetComponent<PauseMenuBehaviour>();

        elementalIconObject = GameObject.Find("Main User Interface").transform.Find("Elemental").transform.Find("ElementalIcon").gameObject;
    }

    private void Start()
    {
        TooltipsController controller = GameObject.Find("UserInterface").transform.Find("Tooltips")
            .GetComponent<TooltipsController>();
        controller.ShowTooltip(0);
    }

    private void Update()
    {
        isGrounded = ( boxCollider.GetContacts(new ContactPoint2D[16]) > 0 ) && 0 == Mathf.Abs(playerBody.velocity.y);

        /*
         * Zapisywanie bezpiecznej lokacji do skakania
         */
        if (isGrounded)
        {
            lastSafePosition = gameObject.transform.position;
        }
        else
        {
            if (gameObject.transform.position.y <= playerVoidLevel)
            {
                lastSafePosition.y += 2.0f;
                gameObject.transform.position = lastSafePosition;
            }
        }
            
        float horizontalInput = Input.GetAxis("Horizontal");
        
        /*
         * Przesyłanie odpowiednich zmiennych do animatora
         */
        animator.SetFloat("PlayerSpeed", Mathf.Abs(playerBody.velocity.x));
        animator.SetFloat("PlayerVelocity", playerBody.velocity.y );
        animator.SetInteger("PlayerAttackState", attackState );
        animator.SetBool("IsPlayerAttacking", isAttacking );
        animator.SetBool("IsGrounded", isGrounded );
        animator.SetBool("IsChargingAttack", isChargingAttack );
        animator.SetFloat("ChargingTime", keyHoldTime );
        animator.SetBool("IsBlocking", isBlocking );
        
        /*
         * Blokowanie chodzenia do tyłu, gdy gracz atakuje, lub blokuje
         */
        if ( isAttacking && 
             ( (horizontalInput < 0 && playerStatus.isFacedRight) || 
               (horizontalInput > 0 && !playerStatus.isFacedRight) ) )
        {
            horizontalInput = 0;
        } 
        else if (isBlocking) horizontalInput = 0;
        
        /*
         * przemieszczanie w osi x, prędkość poruszania się zależna od tego czy gracz atakuje
         */
        if (!isAttacking)
        {
            playerBody.velocity = new Vector2(horizontalInput * playerStatus.GetMovementSpeed(), playerBody.velocity.y);
        }
        else
        {
            playerBody.velocity = new Vector2(horizontalInput * playerStatus.GetMovementSpeed() * 0.6f, playerBody.velocity.y);
        }
        
        /*
         * Nieznaczne wydłużanie hitboxa ataków podczas biegu
         */
        if ( (Mathf.Abs(playerBody.velocity.x) >= playerStatus.GetMovementSpeed()) || ( horizontalInput != 0 && isChargingAttack) )
        {
            BoxCollider2D swordCollider = swordHitbox.GetComponent<BoxCollider2D>();
            swordCollider.size = new Vector2(playerStatus.attackRange * 1.5f, 0.3f);
            swordCollider.offset = new Vector2(playerStatus.attackRange, 0f);
        }
        else
        {
            BoxCollider2D swordCollider = swordHitbox.GetComponent<BoxCollider2D>();
            swordCollider.size = new Vector2(playerStatus.attackRange, 0.3f);
            swordCollider.offset = new Vector2(playerStatus.attackRange / 2, 0f);
        }

        /*
         * skakanie
         */
        if ( (Input.GetKeyDown(InputManager.JumpKey) || Input.GetButtonDown(InputManager.PadButtonJump) ) && !isAttacking && !isBlocking)
        {
            Jump();
        }

        /*
         * Zmiana kierunku gracza
         */
        if ( Input.GetKey(InputManager.MoveLeftKey) && playerStatus.isFacedRight && (Time.timeScale != 0 ) && !isAttacking && !isBlocking )
        {
            playerStatus.isFacedRight = false;
            transform.Rotate(new Vector3(0f, 180f, 0f));
        }
        if (  Input.GetKey(InputManager.MoveRightKey) && !playerStatus.isFacedRight && (Time.timeScale != 0 ) && !isAttacking && !isBlocking )
        {
            playerStatus.isFacedRight = true;
            transform.Rotate(new Vector3(0f, 180f, 0f));
        }
        
        /*
         * Atak, oraz charge attack
         */
        if ( !playerEq.isEquipmentShown && Input.GetKey(InputManager.AttackKey) && !isAttacking && !isBlocking && !pauseMenu.IsGamePaused )
        {
            if (keyHoldTime < holdTimeThreshold)
            {
                keyHoldTime += Time.deltaTime;
                isChargingAttack = true;
            }
        }
        if ( !playerEq.isEquipmentShown && Input.GetKeyUp(InputManager.AttackKey) && !pauseMenu.IsGamePaused)
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

        if ( !playerEq.isEquipmentShown && canBlock)
        {
            /*
             * Parowanie
             */
            if ( !isAttacking && !isChargingAttack && Input.GetKeyDown(InputManager.BlockKey) && !pauseMenu.IsGamePaused)
            {
                isParrying = true;
                StartCoroutine(Parry());
            }

            /*
             * Blokowanie
             */
            if (Input.GetKey(InputManager.BlockKey) && !pauseMenu.IsGamePaused)
            {
                isBlocking = true;
                canBlock = false;
                StartCoroutine(EnableBlockingAfterDuration(cooldownBetweenBlocks));
            }
            else isBlocking = false;
        
            /*
             * Przełączanie animacji blokowania
             */
            if (Input.GetKeyDown(InputManager.BlockKey) && !pauseMenu.IsGamePaused) animator.Play("blockAttack");
        }
        
        /*
         * Przejście przez podłoże
         */
        if ( !playerEq.isEquipmentShown && FloorDetector.isFloorPassable && isGrounded && Input.GetKeyDown(InputManager.MoveDownKey) )
        {
            DisableCollisionForDuration(0.3f);
        }
    }

    private IEnumerator EnableBlockingAfterDuration(float duration)
    {
        // małe okno pomiędzy parowaniami
        yield return new WaitForSeconds(duration);
        canBlock = true;
    }

    private void Jump()
    {
        Vector2 jumpVector = new Vector2(0, jumpForce * 10);
        float playerBodyVelocity = playerBody.GetPointVelocity(jumpVector).y;
        
        if (playerBodyVelocity == 0 && !playerEq.isEquipmentShown) // Sprawdzamy, czy postać jest na ziemi
        {
            playerBody.AddForce(Vector2.up * jumpForce * 10, ForceMode2D.Impulse);
        }
    }

    private IEnumerator Parry()
    {
        yield return new WaitForSeconds(parryWindow);
        isParrying = false;        
        
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
        if (!playerEq.isPickingItem)
        {
            ignoredObject = FloorDetector.collidingObject.GetComponent<Collider2D>();
        
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), ignoredObject, true);
            Invoke("EnableCollision", duration);
        }
    }

    // Włączenie kolizji ponownie.
    private void EnableCollision()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), ignoredObject, false);
        ignoredObject = null;
    }

    private void DealDamage(float damageToDeal)
    {
        // sprawdzanie czy gracz atakuje przeciwnika
        GameObject collidingObject = swordHitbox.gameObject.GetComponent<HitboxBehaviour>().collidingEnemyObject;
        if (collidingObject)
        {
            // zadawanie obrażeń
            collidingObject.GetComponent<EntityStatus>().DealDamage(damageToDeal);
            swordHitbox.gameObject.GetComponent<ParticleSystem>().Play();
        }
    }
    

    private void StartAttack()
    {
        attackCoroutine = StartCoroutine(AttackTimeout());
        isAttacking = true;
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
            if (attackState == 3)
            {
                // Gracz zaczyna nową sekwencję ataku
                attackState = 1;
            }
            else
            {
                // Kontynuuj sekwencję ataku
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

    public void ChangeElementalType(int TypeId)
    {
        if (TypeId >= 0 && TypeId <= ElementalTypes.Count)
        {
            String elementalName = ElementalTypes[TypeId].name;
            UsedElementalTypeId = TypeId;
            UsedElementalName = elementalName;
            
            elementalIconObject.GetComponent<Image>().sprite = ElementalTypes[TypeId].icon;
        }
    }
}
#if UNITY_EDITOR

[CustomEditor(typeof(Player))]
public class TwojaKlasaEditor : Editor
{
    private SerializedProperty selectedElementalTypeProp;

    private void OnEnable()
    {
        selectedElementalTypeProp = serializedObject.FindProperty("UsedElementalTypeId");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Sprawdzenie, czy edytowany obiekt jest jednym obiektem
        if (serializedObject.isEditingMultipleObjects)
        {
            EditorGUILayout.HelpBox("Multi-object editing not supported", MessageType.Error);
            return;
        }

        // Wyświetlanie listy rozwijanej z numerami od 0 do 5
        selectedElementalTypeProp.intValue = EditorGUILayout.Popup("Choose elemental type", selectedElementalTypeProp.intValue, new string[] { "0", "1", "2", "3", "4", "5" });

        // Przycisk do zmiany rodzaju elementu
        if (GUILayout.Button("Change elemental"))
        {
            Player script = (Player)target;
            script.ChangeElementalType(selectedElementalTypeProp.intValue);
        }

        serializedObject.ApplyModifiedProperties();

        // Reszta standardowego interfejsu
        DrawDefaultInspector();
    }
}
#endif
