using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Dialogue : MonoBehaviour
{
    public float dialogueRange = 3f; // É a distância máxima que o jogador pode estar do NPC para iniciar o diálogo
    public LayerMask Player; // Layer do jogador para detectar colisões
    public DialogueSettings dialogue; // Referência ao ScriptableObject de diálogo
    bool playerHit; // Variável para verificar se o jogador está dentro do alcance do diálogo

    private List<string> sentences = new List<string>(); // Lista de sentenças do diálogo
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHit)
    {
        // Se o diálogo NÃO está aparecendo, inicia ele
        if (!DialogueControl.instance.IsDialogueActive()) 
        {
            DialogueControl.instance.Speech(sentences.ToArray());
        }
        // Se já está aparecendo, avança para a próxima frase ou fecha
        else
        {
            DialogueControl.instance.NextSentence();
        }
    }
    }

    void GetNPCInfo()
    {
        
        for (int i = 0; i < dialogue.dialogues.Count; i++)
        {
            switch (DialogueControl.instance.language)
            {
                case DialogueControl.idioma.portuguese:
                    sentences.Add(dialogue.dialogues[i].sentence.portuguese); // Adiciona a sentença em portugues à lista de sentenças
                    break;
                case DialogueControl.idioma.english:
                    sentences.Add(dialogue.dialogues[i].sentence.english); // Adiciona a sentença em ingles à lista de sentenças
                    break;
                case DialogueControl.idioma.spanish:
                    sentences.Add(dialogue.dialogues[i].sentence.spanish); // Adiciona a sentença em espanhol à lista de sentenças
                    break;
            }
        }
    }

    void Start()
    {
        GetNPCInfo(); // Chama o método para obter as informações do NPC
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        ShowDialogue();
    }

    void ShowDialogue()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, dialogueRange, Player); // Verifica se o jogador está dentro do alcance do diálogo
        if (hit != null)
        {
            playerHit = true;
        }
        else
        {
            playerHit = false;
            
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Define a cor do gizmo como vermelho
        Gizmos.DrawWireSphere(transform.position, dialogueRange); // Desenha um círculo vermelho ao redor do NPC para indicar o alcance do diálogo
    }
}
