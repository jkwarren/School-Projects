using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Camera playerCam;

    [Header("Camera")]
    public float fov = 70f;
    public bool cameraCanMove = true;
    public float mouseSens = 2f;
    public float maxLookAngle = 90f;
    // can add a crosshair
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    [Header("Movement")]
    public float gravityMultiplier = 2;
    public bool canMove;
    public float walkSpeed = 5f;
    public float maxVelChange = 10f; // changes how quickly you accelerate?
    private bool isWalking = false;

    [Header("Jump")]
    public bool enableJump = true; // change this name later
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpForce = 5f;
    private bool isGrounded;

    [Header("View Bobbing")]
    public bool enableViewBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);
    private Vector3 jointOriginPos;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerCam.fieldOfView = fov;
        jointOriginPos = joint.localPosition;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb.useGravity = false;
    }

    private void Update()
    {
        if (canMove && !PauseMenu.isPaused)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxisRaw("Mouse X") * mouseSens;
            pitch -= Input.GetAxisRaw("Mouse Y") * mouseSens;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCam.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (enableViewBob)
        {
            ViewBob();
        }

        GroundCheck();
    }

    private void FixedUpdate()
    {
        Vector3 newGravity = new Vector3(0, Physics.gravity.y * gravityMultiplier, 0);
        if (canMove)
        {
            Vector3 targetVel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; // change to get axis raw and create custom smoothing

            if (targetVel.x != 0 || targetVel.z != 0 && isGrounded)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            targetVel = transform.TransformDirection(targetVel) * walkSpeed;

            Vector3 vel = rb.velocity;
            Vector3 velChange = (targetVel - vel) + newGravity;
            velChange.x = Mathf.Clamp(velChange.x, -maxVelChange, maxVelChange);
            velChange.z = Mathf.Clamp(velChange.z, -maxVelChange, maxVelChange);
            velChange.y = 0; // Physics.gravity.y * gravityMultiplier;

            rb.AddForce(velChange, ForceMode.VelocityChange);
            rb.AddForce(transform.up * newGravity.y);
        }
    }

    private void GroundCheck()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = 0.8f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            //Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        if (isGrounded && canMove)
        {
            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void ViewBob()
    {
        if (isWalking)
        {
            timer += Time.deltaTime * bobSpeed;
            joint.localPosition = new Vector3(jointOriginPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginPos.y + Mathf.Sin(timer) /* change this to cos? */ * bobAmount.y, jointOriginPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            timer = 0;
            float timeBob = Time.deltaTime * bobSpeed;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginPos.x, timeBob), Mathf.Lerp(joint.localPosition.y, jointOriginPos.y, timeBob), Mathf.Lerp(joint.localPosition.z, jointOriginPos.z, timeBob));

        }
    }

    public void enableMovement()
    {
        canMove = true;
    }

    public void disableMovement()
    {
        canMove = false;
        rb.velocity = new Vector3(0, 0, 0);
    }


}
