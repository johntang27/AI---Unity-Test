using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardScriptableObject)), CanEditMultipleObjects]
public class CardScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //Read from file
        if (GUILayout.Button("Update Card Data", GUILayout.MaxWidth(200)))
        {
            UpdateData();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    void UpdateData()
    {
        Sprite cardSprite = null;
        CardSpriteScriptableObject tempSO = null;

        tempSO = ((CardScriptableObject)target).GetCardSpriteSourceSO;
        if (tempSO != null)
        {
            string spritePath = string.Format("{0}/{1}", tempSO.GetSpriteSourcePath, tempSO.GetSpriteName);

            if (!File.Exists(spritePath))
            {
                Debug.LogErrorFormat("Cannot find sprite at path {0}", spritePath);
            }
            else
            {
                Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
                if (sprites.Length > 0) cardSprite = sprites.FirstOrDefault(x => x.name == ((CardScriptableObject)target).name) as Sprite;
            }
        }               

        ((CardScriptableObject)target).UpdateData(cardSprite);
        EditorUtility.SetDirty((CardScriptableObject)target);
    }
}
