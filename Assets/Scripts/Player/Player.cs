// using System.Collections;
// using System.Collections.Generic;
// using System.Runtime.Serialization;
// using UnityEngine;

// public class Player : MonoBehaviour
// {
//     [SerializeField] private float speed;
//     [SerializeField] private float runSpeed;
//     [SerializeField] private float rollSpeed;

//     private float currentSpeed;
//     private bool _isRunning;
//     private bool _isRolling;

//     // Variável de controle para garantir que o jogador soltou o botão
//     private bool _canRollAgain = true;

//     private Rigidbody2D rb;
//     private Vector2 _direction;

//     public Vector2 direction
//     {
//         get { return _direction; }
//         set { _direction = value; }
//     }

//     public bool isRunning
//     {
//         get { return _isRunning; }
//         set { _isRunning = value; }
//     }

//     public bool isRolling
//     {
//         get { return _isRolling; }
//         set { _isRolling = value; }
//     }

//     void Start()
//     {
//         currentSpeed = speed;
//         rb = GetComponent<Rigidbody2D>();
//     }
//     void Update()
//     {
//         OnInput();
//         OnRun();
//         OnRoll();
//     }
//     void FixedUpdate()
//     {
//         OnMove();
//     }

//     #region Movement

//     void OnInput()
//     {
//         _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
//     }

//     void OnMove()
//     {
//         //rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
//         rb.MovePosition(rb.position + _direction * speed * Time.fixedDeltaTime);
//     }

//     void OnRun()
//     {
//         // ele so corre se estiver andando parado não pode correr
//         if (Input.GetKeyDown(KeyCode.LeftShift) && _direction != Vector2.zero)
//         {
//             Debug.Log("Running");
//             speed = runSpeed;
//             _isRunning = true;
//         }
//         if (Input.GetKeyUp(KeyCode.LeftShift))
//         {
//             speed = currentSpeed;
//             _isRunning = false;
//         }
//     }

//     void OnRoll()
//     {
//         if (Input.GetMouseButtonDown(1) && _direction != Vector2.zero && _canRollAgain)
//         {
//             speed = rollSpeed;
//             Debug.Log("Rolling");
//             _isRolling = true;
//             _canRollAgain = false;

//         }
//         if (_isRolling)
//         {
//             // Exemplo lógico: Você deve voltar a velocidade ao normal após o tempo do rolamento terminar,
//             // e não baseando-se no GetMouseButtonUp do jogador.
//             speed = currentSpeed;
//             _isRolling = false;
//         }
//         if (Input.GetMouseButtonUp(1))
//         {

//             _canRollAgain = true;

//         }

//     }
//     #endregion
// }

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rollSpeed;
    [SerializeField] private float rollDuration = 0.5f; // Tempo de duração do rolamento em segundos

    private float currentSpeed;
    private bool _isRunning;
    private bool _isRolling;
    private bool _isCutting;

    // Variável de controle para garantir que o jogador soltou o botão
    private bool _canRollAgain = true;

    private Rigidbody2D rb;
    private Vector2 _direction;

    private PlayerAnim anim; // Referência ao PlayerAnim para controlar animações

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
    public bool isCutting
    {
        get { return _isCutting; }
        set { _isCutting = value; }
    }
    void Start()
    {
        currentSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnim>();
    }

    void Update()
    {
        OnInput();
        OnRun();
        OnRoll();
        OnCutting();
    }

    void FixedUpdate()
    {
        OnMove();
    }

    #region Movement

    void OnCutting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCutting = true;
            speed = 0f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isCutting = false;
            speed = currentSpeed;
        }
    }

    void OnInput()
    {
        // Se estiver rolando, podemos travar o input para ele não mudar de direção no meio do rolamento (opcional)
        if (!_isRolling)
        {
            _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
    }

    void OnMove()
    {
        rb.MovePosition(rb.position + _direction * speed * Time.fixedDeltaTime);
    }

    void OnRun()
    {
        // Não deixa correr se já estiver rolando
        if (_isRolling) return;

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
        // Só inicia o rolamento se apertar o botão, estiver se movendo, não estiver rolando E puder rolar de novo
        if (Input.GetMouseButtonDown(1) && _direction != Vector2.zero && _canRollAgain && !_isRolling)
        {
            StartCoroutine(RollRoutine());
        }

        // Se soltar o botão do mouse, libera para poder rolar no próximo clique
        if (Input.GetMouseButtonUp(1))
        {
            _canRollAgain = true;
        }
    }

    // Corrotina que controla o tempo do rolamento de forma independente do clique do mouse
    private IEnumerator RollRoutine()
    {
        _isRolling = true;
        _canRollAgain = false; // Bloqueia novos comandos
        _isRunning = false;    // Cancela a corrida se estivesse correndo

        speed = rollSpeed;     // Aplica a velocidade do rolamento
        Debug.Log("Rolling Started");

        anim.TriggerRollAnimation();

        // Aguarda a duração estipulada do rolamento
        yield return new WaitForSeconds(rollDuration);

        // O rolamento acabou: volta o estado original baseado se o Shift está pressionado ou não
        if (Input.GetKey(KeyCode.LeftShift) && _direction != Vector2.zero)
        {
            speed = runSpeed;
            _isRunning = true;
        }
        else
        {
            speed = currentSpeed;
        }

        _isRolling = false;
        Debug.Log("Rolling Ended");
    }

    #endregion
}