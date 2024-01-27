using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeckScriptableObject))]
public class DeckScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Shuffle Deck", GUILayout.MaxWidth(200)))
        {
            Shuffle();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void Shuffle()
    {
        ((DeckScriptableObject)target).ShuffleDeck();
    }
}
