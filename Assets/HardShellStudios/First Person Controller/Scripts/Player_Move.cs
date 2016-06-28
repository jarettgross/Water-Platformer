using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {

    hardInput hInput = null;

    public bool canMove;
    public float moveSpeed;
    public float sprintMultiplier;
    public float jumpForce;
    float distToGround = 0.5f;

    // Private Variables
    Rigidbody rigid;

	// Use this for initialization
	void Start ()
    {
        hInput = GameObject.FindObjectOfType<hardInput>();
        rigid = GetComponent<Rigidbody>();
        //distToGround = GetComponent<SphereCollider>().bounds.extents.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (canMove)
        {
            var localVel = transform.InverseTransformDirection(rigid.velocity);
            float speed = moveSpeed;

            //if (Input.GetButton("Sprint"))
            //{
            //    speed *= sprintMultiplier;
            //}

            //localVel = new Vector3(Input.GetAxis("Horizontal") * speed, rigid.velocity.y, Input.GetAxis("Vertical") * speed);
            localVel = new Vector3(hInput.GetAxis("Right", "Left", 7) * speed, rigid.velocity.y, hInput.GetAxis("Forward", "Backward", 7) * speed);
            //print(hInput.GetAxis("Forward", "Backward", 1));

            rigid.velocity = transform.TransformDirection(localVel);

            IsGrouneded();

            if (hInput.GetKeyDown("Jump") && IsGrouneded())
            {
                rigid.AddForceAtPosition(Vector3.up * jumpForce, Vector3.up);
            }
        }
	}

    bool IsGrouneded()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), -Vector3.up, Color.red);
        return Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), -Vector3.up, distToGround + 0.1f);
    }
}
