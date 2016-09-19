using UnityEngine;
using System.Collections;

public class MovementSystem : PlayerCore
{

    private float acceleration = 150f;
    private float maxSpeed = 50f;
    private float jumpStrength = 19000f;
    private float rollStrength = 8000f;
    private float flyStrength = 5000f;

    private Rigidbody rbLink;

    Bounds meshBounds;

    private bool isOnGround = true;
    private bool isJumping = false;
    private bool isRolling = false;
    private bool isFlying = false;

    InteractionSystem isLink;


    void Awake()
    {
        isLink = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionSystem>();
    }

    void Start()
    {
        rbLink = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        isOnGround = CheckGroundCollision();

        float xSpeed = Input.GetAxis("Horizontal");
        float zSpeed = Input.GetAxis("Vertical");

        Vector3 velocityYAxis = new Vector3(xSpeed, 0, zSpeed);

        rbLink.AddForce(velocityYAxis.normalized * acceleration);
        LimitVelocity();

        if (isOnGround)
        {
            if (isJumping)
            {
                Jump();
                isJumping = false;
            }
            else if (isRolling)
            {
                Roll();
                isRolling = false;
            }
        }
        else
        {
            if (isFlying)
            {
                Fly();
                isFlying = false;
            }
            else

                rbLink.AddForce(Vector3.down * 350);
        }






    }


    void Update()
    {
        if (isOnGround && Input.GetButtonDown("Jump") && isLink.currentForm == forms.frog)
            isJumping = true;
        else if (isOnGround && Input.GetButtonDown("Jump") && isLink.currentForm == forms.armadillo)
            isRolling = true;
        else if (!isOnGround && Input.GetButtonDown("Jump") && isLink.currentForm == forms.dragon)
            isFlying = true;
    }

    private void LimitVelocity()
    {
        Vector2 xzVel = new Vector2(rbLink.velocity.x, rbLink.velocity.z);

        if (xzVel.magnitude > maxSpeed)
        {
            xzVel = xzVel.normalized * maxSpeed;
            rbLink.velocity = new Vector3(xzVel.x, rbLink.velocity.y, xzVel.y);
        }
    }

    private bool CheckGroundCollision()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Collision");


        if (meshBounds != null)
        {
            if (Physics.Raycast(transform.position + meshBounds.center, Vector3.down, meshBounds.extents.y, layerMask))
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    private void Jump()
    {
        rbLink.AddForce(new Vector3(0, jumpStrength, 0));
    }

    private void Roll()
    {
        rbLink.AddForce(new Vector3(0, 0, rollStrength));
    }

    private void Fly()
    {
        rbLink.AddForce(new Vector3(0, flyStrength, flyStrength * 20));
    }

    public void SettingMeshBounds(string currentForm)
    {
        meshBounds = GameObject.FindGameObjectWithTag(currentForm).GetComponent<MeshFilter>().mesh.bounds;
    }

}
