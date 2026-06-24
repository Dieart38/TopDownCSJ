using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue/Dialogue")]
public class NewBehaviourScript : ScriptableObject
{
    [Header("Settings")]
    // public float textSpeed = 0.05f;
    // public Color textColor = Color.white;
    public GameObject Actor;

    [Header("Dialogue")]
    public Sprite speakerSprite;
    public string sentence;

    public List<Sentence> dialogues = new List<Sentence>();
}

[System.Serializable]
public class Sentence
{
    public string actorName;
    public Sprite profile;
    public Languages sentence;
}

[System.Serializable]
public class Languages
{
    public string portuguese;

    public string english;
    public string spanish;

}


