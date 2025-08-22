using DumbInjector;
using FPS.Scripts.DI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float playerWalkingSpeed = 5f;
    [SerializeField] private float playerRunningSpeed = 15f;
    [SerializeField] private float jumpStrength = 20f;
    
    private CharacterController controller;
    private float forwardMovement;
    private float sideAwayMovement;
    private Vector3 movementDirection;
    private float verticalVelocity;
    
    
    private void Awake ()
    {
        controller = this.GetComponent<CharacterController>();
	}

    private void Start()
    {
        movementDirection = Vector3.zero;
    }

    private void Update()
    {
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            
            sideAwayMovement = Input.GetAxisRaw("Horizontal") * playerRunningSpeed;
            forwardMovement = Input.GetAxisRaw("Vertical") * playerRunningSpeed;
        }
        else
        {
            sideAwayMovement = Input.GetAxisRaw("Horizontal") * playerWalkingSpeed;
            forwardMovement = Input.GetAxisRaw("Vertical") * playerWalkingSpeed;
        }
        
        if (Input.GetButton("Jump") && controller.isGrounded)
        {
            verticalVelocity = jumpStrength;
        }
    }
    
    private void ApplyMovement()
    {
        movementDirection.x = sideAwayMovement;
        movementDirection.z = forwardMovement;
        movementDirection.y = verticalVelocity;

        controller.Move(transform.rotation * movementDirection  * Time.deltaTime);
    }
}
  