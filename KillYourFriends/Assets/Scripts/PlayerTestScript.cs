using Mirror;
using Telepathy;
using UnityEngine;

public class PlayerTestScript : NetworkBehaviour
{
    //player movement vars
    private CharacterController cc;
    public Transform CharacterBody;
    private float playerSpeed = 6.5f;
    Vector3 inputDir;
    Vector3 copyInputVec;
    private bool isMoving = false;
    private float jumpSpeed = 9f;
    private float gravityWeight = 0f;
    Vector3 gravityDirection;
    private Vector3 moveDirection = Vector3.zero;
    public Transform cam;

    //camera rotation vars
    private Camera CameraMain;

    private float yawSensitivity = 8.5f;
    private float pitchSensitivity = 5.5f;

    public GameObject Snout;
    Quaternion SnoutFollowCam;
    Quaternion StartRotationSnout;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public Transform CamRig;

    //shoot bullet vars
    public GameObject bulletPrefab;
    public Transform SnoutTip;

    private float delayShot = 0.25f;
    private bool waitingDelay = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) return;
        cc = GetComponentInChildren<CharacterController>();

        CameraMain = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;

        StartRotationSnout = Snout.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        MovePlayer();
        JumpPlayer();

        Vector3 allMovementVectors = inputDir * playerSpeed + gravityDirection * gravityWeight + moveDirection;
        cc.Move(allMovementVectors * Time.deltaTime);

        //has rig set to focus on character
        CamRig.position = CharacterBody.position;

        RotateCamera();

        if (Input.GetButtonDown("Fire1") && !waitingDelay) CmdShootBullet();

        //shot delay
        if (waitingDelay)
        {
            delayShot -= 1 * Time.deltaTime;
        }
        else
        {
            delayShot = 0.25f;
        }

        if (delayShot <= 0)
        {
            waitingDelay = false;
        }
    }

    //movement for player
    private void MovePlayer()
    {
        //gets input from WASD using Unity's Vertical and Horizontal axis's
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //applies this gameobjects forward and right vector to a vector 3 depending on the axis's numbers
        // Example: w = 1, s = -1, a = -1, d = 1
        inputDir = CharacterBody.forward * v + CharacterBody.right * h;

        //this statement rotates the character to be facing away from camera if movement is applied
        if (h != 0 || v != 0)
        {

            Quaternion camRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);
            //CharacterBody.rotation = Quaternion.Lerp(CharacterBody.rotation, camRot, 0.1f);
            CharacterBody.rotation = Mymath.Slide(CharacterBody.rotation, camRot, 0.001f);
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

    private void RotateCamera()
    {
        //saves axis movement of x and y mouse movement
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // yaw and pitch values change determined on 
        // mousex and mousey movement and applied sensitivity to both
        yaw += mx * yawSensitivity;
        pitch -= my * pitchSensitivity;

        //clamp pitch, so camera doesn't rotate too far low or high to seem weird
        float pitch_clamped = Mathf.Clamp(pitch, -5f, 90f);

        // use the clamped pitch and yaw to rotate camera rig, entered in as euler angles through Quaternion class 
        CamRig.rotation = Quaternion.Euler(pitch_clamped, yaw, 0);

        Snout.transform.localRotation = Quaternion.Euler(pitch_clamped, CharacterBody.rotation.y, CharacterBody.rotation.z);
    }

    [Command]
    void CmdShootBullet() 
    {
        GameObject bullet = Instantiate(bulletPrefab, SnoutTip.position, SnoutTip.rotation);
        NetworkServer.Spawn(bullet);

        waitingDelay = true;
    }
}