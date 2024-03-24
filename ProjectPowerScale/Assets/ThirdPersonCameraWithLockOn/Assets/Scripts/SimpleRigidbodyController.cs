using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif



public class SimpleRigidbodyController : MonoBehaviour
{

    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;
#if ENABLE_INPUT_SYSTEM

    private Vector2 _moveInput;
    private float _jumpInput;

    private Vector2 _inputVelocity;

    private void ReadRawInputFromInputSystem()
    {

        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        if (kb != null)
        {
            Vector2 newInput = Vector2.zero;

            if (kb.wKey.isPressed)
                newInput.y += 1.0f;
            if (kb.sKey.isPressed)
                newInput.y -= 1.0f;
            if (kb.dKey.isPressed)
                newInput.x += 1.0f;
            if (kb.aKey.isPressed)
                newInput.x -= 1.0f;

            _moveInput = Vector2.SmoothDamp(_moveInput, newInput, ref _inputVelocity, 0.05f);

            if (kb.spaceKey.wasPressedThisFrame)
                _jumpInput = 1.0f;
        }




        Gamepad gp = InputSystem.GetDevice<Gamepad>();
        if (gp != null)
        {
            Vector2 gamepadValue = gp.leftStick.ReadValue();
            if (gp.leftStick.IsActuated())
            {
                _moveInput = gamepadValue;
            }

            if (gp.buttonSouth.wasPressedThisFrame)
                _jumpInput = 1.0f;
        }


}
#endif

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;


#if ENABLE_INPUT_SYSTEM
        ReadRawInputFromInputSystem();
        _inputs.x = _moveInput.x;
        _inputs.z = _moveInput.y;
#else
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
#endif


        if (_inputs != Vector3.zero)
            transform.forward = _inputs;

        _inputs = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * _inputs;
#if ENABLE_INPUT_SYSTEM
        if (_jumpInput > 0.5f && _isGrounded)

#else
        if (Input.GetButtonDown("Jump") && _isGrounded)
#endif
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

    }

    void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }
}
