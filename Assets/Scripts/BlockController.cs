using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockController : MonoBehaviour
{
    [SerializeField] private InputActionReference _move;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _fallSpeed = 2f;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private bool _hasLanded = false;

    void Awake()
    {
        _hasLanded = false;
        
        _move.action.performed += ctx => _moveDirection = ctx.ReadValue<Vector2>();
        _move.action.canceled += ctx => _moveDirection = Vector2.zero;
        _moveDirection = Vector2.zero;

        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 2;
        _rb.freezeRotation = true;
    }

    void Update()
    {
        if (!_hasLanded) {
            HandleInput();    
        }
    }

    void FixedUpdate()
    {
        MoveBlock();
    }

    private void HandleInput() {
        _moveDirection = _move.action.ReadValue<Vector2>();
    }

    private void MoveBlock() {
        Vector2 move = new Vector2(_moveDirection.x * _moveSpeed, -_fallSpeed);
        _rb.linearVelocity = move;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (_hasLanded) return;

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Block")) {
            _hasLanded = true;
            StartCoroutine(HandleLanding());
        }
    }

    private IEnumerator HandleLanding() {
        yield return new WaitForFixedUpdate();
        _rb.linearVelocity = Vector2.zero;

        var gridManager = FindFirstObjectByType<GridManager>();
        bool added = gridManager.AddBlockToGrid(transform);

        if (!added) {
                FindFirstObjectByType<GameManager>().GameOver(0);
        }
        else {
            FindFirstObjectByType<GameManager>().OnBlockLanded();
        }
        this.enabled = false;
    }

    private void OnEnable() {
        _move.action.Enable();
    }

    private void OnDisable() {
        _move.action.Disable();
    }
}
