using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float playerWalkingSpeed = 5f;
    [SerializeField] private float playerRunningSpeed = 15f;
    [SerializeField] private float jumpStrength = 20f;
    
    private CharacterController controller;
    private float forwardMovement;
    private float sideAwaysMovement;
    private Vector3 playerMovement;
    private float verticalVelocity;

    private void Awake ()
    {
        controller = this.GetComponent<CharacterController>();
	}

    private void Start()
    {
        playerMovement = Vector3.zero;
    }

    private void Update()
    {
        if (ManagerScreen.instance.IsPaused()) return;
        ApplyGravity();
        CheckInputs();
        ApplyMovement();
    }

    private void ApplyGravity()
    {
        verticalVelocity += Physics.gravity.y * Time.deltaTime;
    }

    private void CheckInputs()
    {
        // Calculating the movement based on input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            forwardMovement = Input.GetAxis("Vertical") * playerRunningSpeed;
            sideAwaysMovement = Input.GetAxis("Horizontal") * playerRunningSpeed;
        }
        else
        {
            forwardMovement = Input.GetAxis("Vertical") * playerWalkingSpeed;
            sideAwaysMovement = Input.GetAxis("Horizontal") * playerWalkingSpeed;
        }

        // Jump
        if (Input.GetButton("Jump") && controller.isGrounded)
        {
            verticalVelocity = jumpStrength;
        }
    }

    private void ApplyMovement()
    {
        playerMovement.x = sideAwaysMovement;
        playerMovement.y = verticalVelocity;
        playerMovement.z = forwardMovement;
        controller.Move(transform.rotation * playerMovement * Time.deltaTime);
    }
}
