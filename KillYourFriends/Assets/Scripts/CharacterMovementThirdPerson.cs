using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementThirdPerson : MonoBehaviour
{
    private CharacterController cc;

    private float playerSpeed = 6.5f;

    Vector3 inputDir;
    Vector3 copyInputVec;
    private bool isMoving = false;

    private float jumpSpeed = 9f;
    private float gravityWeight = 0f;
    Vector3 gravityDirection;

    private Vector3 moveDirection = Vector3.zero;

    private float pushPower = 2f;

    private bool run;
    private bool left;
    private bool right;
    private bool idle;

    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        JumpPlayer();


        Vector3 allMovementVectors = inputDir * playerSpeed + gravityDirection * gravityWeight + moveDirection;

        cc.Move(allMovementVectors * Time.deltaTime);
    }

    //movement for player
    private void MovePlayer()
    {
        //gets input from WASD using Unity's Vertical and Horizontal axis's
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //applies this gameobjects forward and right vector to a vector 3 depending on the axis's numbers
        // Example: w = 1, s = -1, a = -1, d = 1
        inputDir = transform.forward * v + transform.right * h;

        //this statement rotates the character to be facing away from camera if movement is applied
        if (h != 0 || v != 0)
        {

            Quaternion camRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);
            //transform.rotation = Quaternion.Lerp(transform.rotation, camRot, 0.1f);
            transform.rotation = Mymath.Slide(transform.rotation, camRot, 0.001f);
            isMoving = true;
        }
    }

    private void JumpPlayer()
    {
        //gravity appylied
        gravityDirection = Vector3.down;

        if (!cc.isGrounded)
        {
            gravityWeight += 25f * Time.deltaTime;
        }

        //checks if spacebar clicked and only triggable once
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpSpeed;
            gravityWeight = 0;
        }
    }

    //pushes block
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitBody = hit.collider.attachedRigidbody;

        if(hitBody == null || hitBody.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        hitBody.velocity = pushDir * pushPower;
    }
}
