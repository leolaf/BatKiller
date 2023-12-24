using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PlayerClass
{
    KNIGHT,
    MAGE,
    ROGUE,
    WARRIOR
}

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float speed = 500;
    [SerializeField] private float jumpForce = 500;
    [SerializeField] private float gravity = 10;


    [SerializeField] private GameObject visuals;
    [SerializeField] private List<GameObject> knightVisualsPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> mageVisualsPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> rogueVisualsPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> warriorVisualsPrefabs = new List<GameObject>();

    [SerializeField] private PlayerClass playerClass;

    [SerializeField][Range(1,3)] private int playerLevel;

    private Animator animator;
    private ClassState classState;

    private float horizontalMovement = 0;
    private float verticalMovement = 0;

    private bool onGround = false;
    private bool OnGround
    {
        get 
        { 
            return onGround; 
        } 
        set
        {
            onGround = value;
            animator.SetBool("onGround", value);
        }
    }

    private Rigidbody rb;

    private void OnValidate()
    {
        if(EditorApplication.isPlaying)
        {
            AdaptVisuals();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        classState = gameObject.AddComponent<WeakWarriorState>();
        Physics.gravity = Vector3.down * gravity;
        AdaptVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        if(horizontalMovement != 0)
        {
            visuals.transform.rotation = Quaternion.Euler(0, 180 * (horizontalMovement < 0 ? 1 : 0), 0);
            animator.SetFloat("speed", Mathf.Abs(horizontalMovement)*speed);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }

        verticalMovement = 0;
        if (OnGround)
        {
            verticalMovement = Input.GetAxis("Jump");
        }
    }

    private void FixedUpdate()
    {
        Vector3 currentVelo = Vector3.right * horizontalMovement * speed * Time.fixedDeltaTime;
        rb.velocity = new Vector2(currentVelo.x, rb.velocity.y);
        if(verticalMovement > 0)
        {
            rb.AddForce(new Vector3(0, verticalMovement, 0) * jumpForce);
            animator.SetTrigger("jump");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            OnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            OnGround = false;
        }
    }

    public void PickUp(PlayerClass pickupType)
    {
        if(playerClass == pickupType)
        {
            playerLevel = Mathf.Clamp(playerLevel+1,1,3);
        }
        else
        {
            playerClass = pickupType;
        }
        TransformPlayer(pickupType);
    }

    void TransformPlayer(PlayerClass playerClass)
    {
        Type newStateType = classState.Transition(playerClass);
        if (newStateType == null)
            return;
        Destroy(classState);
        classState = (ClassState) gameObject.AddComponent(newStateType);
        AdaptVisuals();
    }

    void AdaptVisuals()
    {
        for (int i = 0; i < visuals.transform.childCount; i++)
        {
            Destroy(visuals.transform.GetChild(i).gameObject);
        }
        visuals.transform.localScale = Vector3.one * (0.75f + (1 - 0.75f) / 3f * playerLevel);
        GameObject visualPrefab = null;
        switch (playerClass)
        {
            case PlayerClass.KNIGHT:
                visualPrefab = knightVisualsPrefabs[playerLevel - 1];
                break;
            case PlayerClass.MAGE:
                visualPrefab = mageVisualsPrefabs[playerLevel - 1];
                break;
            case PlayerClass.ROGUE:
                visualPrefab = rogueVisualsPrefabs[playerLevel - 1];
                break;
            case PlayerClass.WARRIOR:
                visualPrefab = warriorVisualsPrefabs[playerLevel - 1];
                break;
        }
        if (visualPrefab != null)
        {
            GameObject newVisual = Instantiate(visualPrefab, visuals.transform.position, visuals.transform.rotation, visuals.transform);
            animator = newVisual.GetComponentInChildren<Animator>();
        }
    }
}
