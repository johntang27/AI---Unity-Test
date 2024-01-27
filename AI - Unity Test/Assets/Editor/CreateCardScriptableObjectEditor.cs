using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateCardScriptableObject))]
public class CreateCardScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Create Card ScriptableObjects", GUILayout.MaxWidth(200)))
        {
            CreateAsset();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
    
    public void CreateAsset()
    {
        string mainSpritePath = "", savePath = "";
        CardSpriteScriptableObject tempSO = null;

        tempSO = ((CreateCardScriptableObject)target).GetCardSpriteSourceSO;

        if (tempSO != null)
        {
            mainSpritePath = string.Format("{0}/{1}", tempSO.GetSpriteSourcePath, tempSO.GetSpriteName);
            savePath = ((CreateCardScriptableObject)target).GetSaveLocation;
        }

        if (!File.Exists(mainSpritePath))
        {
            Debug.LogErrorFormat("Cannot find sprite at path {0}", mainSpritePath);
        }
        else
        {
            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(mainSpritePath);
            Debug.LogError("All sprites count: " + sprites.Length);
            string[] mainSpriteName = tempSO.GetSpriteName.Split('.');
            if (sprites.Length > 0)
            {
                for(int i = 0; i < sprites.Length; i++)
                {
                    Debug.LogError(sprites[i].name);
                    if (sprites[i].name == mainSpriteName[0]) continue;
                    CardScriptableObject asset = ScriptableObject.CreateInstance<CardScriptableObject>();

                    string assetSave = string.Format("{0}/{1}.asset", ((CreateCardScriptableObject)target).GetSaveLocation, sprites[i].name);
                    AssetDatabase.CreateAsset(asset, assetSave);
                    asset.UpdateData((Sprite)sprites[i], tempSO);
                    AssetDatabase.SaveAssets();

                    EditorUtility.FocusProjectWindow();
                    EditorUtility.SetDirty(asset);

                    Selection.activeObject = asset;
                }
            }
        }
    }
}
