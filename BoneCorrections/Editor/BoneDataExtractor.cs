using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BoneDataExtractor : EditorWindow
{
    private GameObject selectedGameObject;
    private string assetFolderPath = "Assets/";

    [MenuItem("Tools/Bone Data Extractor")]
    public static void ShowWindow()
    {
        GetWindow<BoneDataExtractor>("Bone Data Extractor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a GameObject with an Animator", EditorStyles.boldLabel);
        selectedGameObject = EditorGUILayout.ObjectField("GameObject", selectedGameObject, typeof(GameObject), true) as GameObject;

        GUILayout.Label("Asset Folder Path (must exist)", EditorStyles.boldLabel);
        assetFolderPath = EditorGUILayout.TextField("Path", assetFolderPath);

        if (GUILayout.Button("Extract Bone Data"))
        {
            ExtractAndCreate();
        }
    }

    private void ExtractAndCreate()
    {
        if (selectedGameObject != null)
        {
            // Extract bone data
            var boneData = ExtractBoneData(selectedGameObject);

            // Create and populate BoneCorrectionHandler instance
            var boneCorrectionHandler = CreateInstance<BoneCorrectionHandler>();
            boneCorrectionHandler.corrections = boneData.ToArray();

            // Save the created ScriptableObject
            string validAssetName = SanitizeAssetName(selectedGameObject.name + "_Corrections");
            string assetPath = $"{assetFolderPath.TrimEnd('/')}/{validAssetName}.asset";
            AssetDatabase.CreateAsset(boneCorrectionHandler, assetPath);
            AssetDatabase.SaveAssets();

            // Focus on the created asset in the Project window
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = boneCorrectionHandler;
        }
        else
        {
            Debug.LogError("No GameObject selected!");
        }
    }

    private string SanitizeAssetName(string name)
    {
        // Regex to remove invalid characters
        return Regex.Replace(name, "[\\/:*?\"<>|]", "");
    }

    private List<BoneCorrections> ExtractBoneData(GameObject gameObject)
    {
        List<BoneCorrections> boneCorrections = new List<BoneCorrections>();

        Animator animator = FindAnimatorInParent(gameObject);
        if (animator == null || !animator.isHuman)
        {
            Debug.LogError("No humanoid Animator found in the parent hierarchy of the selected GameObject.");
            return boneCorrections;
        }

        AddBoneDataRecursive(gameObject.transform, animator, ref boneCorrections);

        return boneCorrections;
    }

    private void AddBoneDataRecursive(Transform currentTransform, Animator animator, ref List<BoneCorrections> boneCorrections)
    {
        HumanBodyBones bone;
        if (TryGetHumanBodyBone(currentTransform, animator, out bone))
        {
            BoneCorrections correction = new BoneCorrections
            {
                bone = bone,
                rotation = currentTransform.localRotation
            };
            boneCorrections.Add(correction);
        }

        foreach (Transform child in currentTransform)
        {
            AddBoneDataRecursive(child, animator, ref boneCorrections);
        }
    }

    private bool TryGetHumanBodyBone(Transform transform, Animator animator, out HumanBodyBones bone)
    {
        foreach (HumanBodyBones b in System.Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (b == HumanBodyBones.LastBone) continue;

            if (animator.GetBoneTransform(b) == transform)
            {
                bone = b;
                return true;
            }
        }

        bone = HumanBodyBones.LastBone;
        return false;
    }

    private Animator FindAnimatorInParent(GameObject gameObject)
    {
        Transform current = gameObject.transform;
        while (current != null)
        {
            Animator animator = current.GetComponent<Animator>();
            if (animator != null && animator.isHuman)
            {
                return animator;
            }
            current = current.parent;
        }
        return null;
    }
}