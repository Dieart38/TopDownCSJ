using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue/Dialogue")]
public class DialogueSettings : ScriptableObject
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

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueSettings))]
public class BuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueSettings ds = (DialogueSettings)target;

        Languages lang = new Languages();
        lang.portuguese = ds.sentence;

        Sentence sentence = new Sentence();
        sentence.profile = ds.speakerSprite;
        sentence.sentence = lang;

        if (GUILayout.Button("Add Dialogue"))
        {
            ds.dialogues.Add(sentence);
            ds.speakerSprite = null;
            ds.sentence = "";
        }
    }
}
#endif


