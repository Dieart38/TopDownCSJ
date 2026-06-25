using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{

    [System.Serializable]
    public enum idioma
    {
        portuguese,
        english,
        spanish
    }

    public idioma language = idioma.portuguese; // Define o idioma padrão como português

    [Header("Components")]
    public GameObject dialogueObj; // janela de diálogo
    public Image profileSprite; // sprite do perfil do personagem
    public Text speechText; // fala do personagem
    public Text actorName; //  nome do personagem

    [Header("Settings")]
    public float typingSpeed = 0.05f; // velocidade de digitação do texto

    // Variáveis privadas
    public bool isShowing; // indica se o diálogo está sendo exibido
    private int index; // índice das sentenças
    private string[] sentences; // sentenças do diálogo

    // criando um singleton para o controle do diálogo
    public static DialogueControl instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsDialogueActive()
    {
        return isShowing;
    }

    // Start is called before the first frame update
    IEnumerator TypeSentence()
    {
        speechText.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // pular para a próxima sentença
    public void NextSentence()
    {
        if (speechText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                //speechText.text = "";
                StopAllCoroutines(); // Garante que a digitação anterior parou
                StartCoroutine(TypeSentence());
            }
            else
            {
                speechText.text = "";
                index = 0;
                isShowing = false;
                dialogueObj.SetActive(false);
                sentences = null;
            }
        }

        else // Se o jogador clicou antes de terminar de digitar, completa o texto
        {
            StopAllCoroutines();
            speechText.text = sentences[index];
        }

    }



    // Chamar o diálogo do npc
    public void Speech(string[] txt)
    {
        if (!isShowing) // se o diálogo não estiver sendo exibido, iniciar o diálogo
        {
            dialogueObj.SetActive(true);// ativar a janela de diálogo
            sentences = txt; // atribuir as sentenças do diálogo           
            StartCoroutine(TypeSentence()); // iniciar a digitação da primeira sentença
            isShowing = true; // indicar que o diálogo está sendo exibido
            index = 0; // resetar o índice das sentenças
        }

    }
}
