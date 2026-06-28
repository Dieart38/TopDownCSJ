using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Player player;
    private Animator anim;

    void Start()
    {

        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        // Se o jogador estiver rolando, nós evitamos que as animações de 
        // andar/correr fiquem se sobrepondo à animação de rolamento.
        if (player.isRolling)
        {
            // Opcional: Mantém a rotação do sprite mesmo rolando
            HandleSpriteFlip(); 
            return; 
        }

        OnMove();
        OnRun();

    }

    #region Movement
    void OnMove()
    {
        if (player.direction.sqrMagnitude > 0)
        {
            anim.SetInteger("transition", 1); // Andando
        }
        else
        {
            anim.SetInteger("transition", 0); // Parado (Idle)
        }

        if (player.isCutting)
        {
            anim.SetInteger("transition",3);
        }

        HandleSpriteFlip();
    }

    void OnRun()
    {
        if (player.isRunning)
        {
            anim.SetInteger("transition", 2); // Correndo
        }
    }

    // Isolamos a lógica de virar o sprite para ficar mais limpo
    void HandleSpriteFlip()
    {
        if (player.direction.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (player.direction.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    // ESTA FUNÇÃO DEVE SER CHAMADA PELO SCRIPT DO PLAYER APENAS UMA VEZ!
    public void TriggerRollAnimation()
    {
        anim.SetTrigger("isRoll");
    }
    #endregion
}

