using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rollSpeed;

    private float currentSpeed;
    private bool _isRunning;
    private bool _isRolling;

    private Rigidbody2D rb;
    private Vector2 _direction;

    public Vector2 direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public bool isRunning
    {
        get { return _isRunning; }
        set { _isRunning = value; }
    }

    public bool isRolling
    {
        get { return _isRolling; }
        set { _isRolling = value; }
    }

    void Start()
    {
        currentSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        OnInput();
        OnRun();
        OnRoll();
    }
    void FixedUpdate()
    {
        OnMove();
    }

    #region Movement

    void OnInput()
    {
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void OnMove()
    {
        //rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
        rb.MovePosition(rb.position + _direction * speed * Time.fixedDeltaTime);
    }

    void OnRun()
    {
        // ele so corre se estiver andando parado não pode correr
        if (Input.GetKeyDown(KeyCode.LeftShift) && _direction != Vector2.zero)
        {
            Debug.Log("Running");
            speed = runSpeed;
            _isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = currentSpeed;
            _isRunning = false;
        }
    }

    void OnRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed = rollSpeed;
            Debug.Log("Rolling");
            _isRolling = true;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            speed = currentSpeed;
            _isRolling = false;

        }

    }
    #endregion
}