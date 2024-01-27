using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeCardScriptObject
{
    private static string spritePath = "Assets/Sprites/playingCards.png";

    [MenuItem("Assets/Create/All Card ScriptableObjects")]
    public static void CreateAsset()
    {
        if (!File.Exists(spritePath))
        {
            Debug.LogErrorFormat("Cannot find sprite at path {0}", spritePath);
        }
        else
        {
            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
            Debug.LogError("All sprites count: " + sprites.Length);
            if (sprites.Length > 0)
            {
                for(int i = 1; i < sprites.Length; i++)
                {
                    Debug.LogError(sprites[i].name);
                    CardScriptableObject asset = ScriptableObject.CreateInstance<CardScriptableObject>();

                    string assetSave = string.Format("Assets/ScriptableObjects/Cards/{0}.asset", sprites[i].name);
                    AssetDatabase.CreateAsset(asset, assetSave);
                    asset.UpdateData((Sprite)sprites[i]);
                    AssetDatabase.SaveAssets();

                    EditorUtility.FocusProjectWindow();
                    EditorUtility.SetDirty(asset);

                    Selection.activeObject = asset;
                }
            }
        }
    }
}
