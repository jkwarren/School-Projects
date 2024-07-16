using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StoryScript", menuName = "CustomScriptableObject/Story Text")]
public class StoryScript: ScriptableObject
{
    //list to hold all the text entries for each level
    public TextEntry[] allTextEntries;

    [System.Serializable]
    public class TextEntry
    {
        [TextArea(3, 10)]
        public string[] texts; // in inspector this affects the text window size
    }

    
}