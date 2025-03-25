using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockController : MonoBehaviour
{
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _fastFall;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _fallSpeed = 2f;
    [SerializeField] private float _fastFallSpeed = 10f;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private bool _isFastFalling;

    void Awake()
    {
        _move.action.performed += ctx => _moveDirection = ctx.ReadValue<Vector2>();
        _move.action.canceled += ctx => _moveDirection = Vector2.zero;
        _fastFall.action.performed += ctx => FastFall(ctx);
        _fastFall.action.canceled += ctx => _isFastFalling = false;      

        _moveDirection = Vector2.zero;

        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        MoveBlock();
    }

    private void HandleInput() {
        _moveDirection = _move.action.ReadValue<Vector2>();
    }

    private void MoveBlock() {
        Vector2 move = new Vector2(_moveDirection.x * _moveSpeed, _rb.linearVelocity.y);
        _rb.linearVelocity = move;

        float currentFallSpeed = _isFastFalling ? _fastFallSpeed : _fallSpeed;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -currentFallSpeed);
    }

    private void OnEnable() {
        _move.action.Enable();
        _fastFall.action.Enable();
    }

    private void OnDisable() {
        _move.action.Disable();
        _fastFall.action.Disable();
    }

    public void FastFall(InputAction.CallbackContext ctx) {
        _isFastFalling = ctx.ReadValueAsButton();    
    }
}
