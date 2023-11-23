using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float speed = 500;

    [SerializeField]
    private float jumpForce = 500;

    [SerializeField]
    private float gravity = 10;

    private float horizontalMovement = 0;
    private float verticalMovement = 0;

    private bool onGround = false;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = Vector3.down * gravity;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = 0;
        if (onGround)
        {
            verticalMovement = Input.GetAxis("Jump");
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + new Vector3(horizontalMovement, 0, 0) * speed * Time.fixedDeltaTime);
        rb.velocity += new Vector3(0, verticalMovement, 0) * jumpForce;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            onGround = false;
        }
    }
}
