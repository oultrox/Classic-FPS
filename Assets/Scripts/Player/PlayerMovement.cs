using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float playerWalkingSpeed = 5f;
    [SerializeField] private float playerRunningSpeed = 15f;
    [SerializeField] private float jumpStrength = 20f;
    [SerializeField] private float verticalRotationLimit = 80f;
    
    private CharacterController controller;
    private float verticalRotation;
    private float forwardMovement;
    private float sideAwaysMovement;
    private Vector3 playerMovement;
    private float verticalVelocity;

    // Use this for initialization
    private void Awake ()
    {
        controller = this.GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        verticalRotation = 0;
        playerMovement = Vector3.zero;
	}

    private void Update()
    {

        float horizontalRotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y");
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation,0, 0);
  
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

        // Gravity
        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        // Jump
        if (Input.GetButton("Jump") && controller.isGrounded)
        {
            verticalVelocity = jumpStrength;
        }

        // Applying the movement
        playerMovement.x = sideAwaysMovement;
        playerMovement.y = verticalVelocity;
        playerMovement.z = forwardMovement;
        controller.Move(transform.rotation * playerMovement * Time.deltaTime);
    }
}
