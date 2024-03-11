using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class BoneCorrectionMirrorer : EditorWindow
{
    private BoneCorrectionHandler originalAsset;

    [MenuItem("Tools/Bone Correction Mirrorer")]
    public static void ShowWindow()
    {
        GetWindow<BoneCorrectionMirrorer>("Bone Correction Mirrorer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a BoneCorrectionHandler Asset", EditorStyles.boldLabel);
        originalAsset = EditorGUILayout.ObjectField("Asset", originalAsset, typeof(BoneCorrectionHandler), false) as BoneCorrectionHandler;

        if (GUILayout.Button("Create Mirrored Asset"))
        {
            if (originalAsset != null)
            {
                CreateMirroredAsset(originalAsset);
            }
            else
            {
                Debug.LogError("No BoneCorrectionHandler asset selected!");
            }
        }
    }

    private void CreateMirroredAsset(BoneCorrectionHandler asset)
    {
        List<BoneCorrections> mirroredData = new List<BoneCorrections>();

        Debug.Log($"Original Asset has {asset.corrections.Length} corrections.");

        // Iterate through each correction in the original asset
        foreach (var correction in asset.corrections)
        {
            Debug.Log($"Processing bone: {correction.bone}");

            HumanBodyBones mirroredBone = GetMirroredBone(correction.bone);
            if (mirroredBone != HumanBodyBones.LastBone)
            {
                mirroredData.Add(new BoneCorrections
                {
                    bone = mirroredBone,
                    rotation = correction.rotation // Modify this as needed for correct mirroring
                });

                //Debug.Log($"Added mirrored bone: {mirroredBone}");
            }
            else
            {
                Debug.Log($"No mirror found for bone: {correction.bone}");
            }
        }

        Debug.Log($"Mirrored Asset will have {mirroredData.Count} corrections.");

        // Create a new BoneCorrectionHandler instance for the mirrored data
        BoneCorrectionHandler mirroredAsset = ScriptableObject.CreateInstance<BoneCorrectionHandler>();
        mirroredAsset.corrections = mirroredData.ToArray();

        // Determine the path for the new mirrored asset
        string originalPath = AssetDatabase.GetAssetPath(asset);
        string directory = Path.GetDirectoryName(originalPath);
        string mirroredAssetPath = Path.Combine(directory, asset.name + "Mirrored.asset");

        // Create and save the mirrored asset
        AssetDatabase.CreateAsset(mirroredAsset, mirroredAssetPath);
        AssetDatabase.SaveAssets();

        // Focus on the created asset in the Unity Editor
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = mirroredAsset;
    }

    private HumanBodyBones GetMirroredBone(HumanBodyBones originalBone)
    {
        switch (originalBone)
        {
            // Upper body
            case HumanBodyBones.LeftUpperArm: return HumanBodyBones.RightUpperArm;
            case HumanBodyBones.RightUpperArm: return HumanBodyBones.LeftUpperArm;
            case HumanBodyBones.LeftLowerArm: return HumanBodyBones.RightLowerArm;
            case HumanBodyBones.RightLowerArm: return HumanBodyBones.LeftLowerArm;
            case HumanBodyBones.LeftHand: return HumanBodyBones.RightHand;
            case HumanBodyBones.RightHand: return HumanBodyBones.LeftHand;

            // Lower body
            case HumanBodyBones.LeftUpperLeg: return HumanBodyBones.RightUpperLeg;
            case HumanBodyBones.RightUpperLeg: return HumanBodyBones.LeftUpperLeg;
            case HumanBodyBones.LeftLowerLeg: return HumanBodyBones.RightLowerLeg;
            case HumanBodyBones.RightLowerLeg: return HumanBodyBones.LeftLowerLeg;
            case HumanBodyBones.LeftFoot: return HumanBodyBones.RightFoot;
            case HumanBodyBones.RightFoot: return HumanBodyBones.LeftFoot;

            // Fingers 
            // Thumb
            case HumanBodyBones.LeftThumbProximal: return HumanBodyBones.RightThumbProximal;
            case HumanBodyBones.RightThumbProximal: return HumanBodyBones.LeftThumbProximal;
            case HumanBodyBones.LeftThumbIntermediate: return HumanBodyBones.RightThumbIntermediate;
            case HumanBodyBones.RightThumbIntermediate: return HumanBodyBones.LeftThumbIntermediate;
            case HumanBodyBones.LeftThumbDistal: return HumanBodyBones.RightThumbDistal;
            case HumanBodyBones.RightThumbDistal: return HumanBodyBones.LeftThumbDistal;

            // Index
            case HumanBodyBones.LeftIndexProximal: return HumanBodyBones.RightIndexProximal;
            case HumanBodyBones.RightIndexProximal: return HumanBodyBones.LeftIndexProximal;
            case HumanBodyBones.LeftIndexIntermediate: return HumanBodyBones.RightIndexIntermediate;
            case HumanBodyBones.RightIndexIntermediate: return HumanBodyBones.LeftIndexIntermediate;
            case HumanBodyBones.LeftIndexDistal: return HumanBodyBones.RightIndexDistal;
            case HumanBodyBones.RightIndexDistal: return HumanBodyBones.LeftIndexDistal;

            // Middle
            case HumanBodyBones.LeftMiddleProximal: return HumanBodyBones.RightMiddleProximal;
            case HumanBodyBones.RightMiddleProximal: return HumanBodyBones.LeftMiddleProximal;
            case HumanBodyBones.LeftMiddleIntermediate: return HumanBodyBones.RightMiddleIntermediate;
            case HumanBodyBones.RightMiddleIntermediate: return HumanBodyBones.LeftMiddleIntermediate;
            case HumanBodyBones.LeftMiddleDistal: return HumanBodyBones.RightMiddleDistal;
            case HumanBodyBones.RightMiddleDistal: return HumanBodyBones.LeftMiddleDistal;

            // Ring
            case HumanBodyBones.LeftRingProximal: return HumanBodyBones.RightRingProximal;
            case HumanBodyBones.RightRingProximal: return HumanBodyBones.LeftRingProximal;
            case HumanBodyBones.LeftRingIntermediate: return HumanBodyBones.RightRingIntermediate;
            case HumanBodyBones.RightRingIntermediate: return HumanBodyBones.LeftRingIntermediate;
            case HumanBodyBones.LeftRingDistal: return HumanBodyBones.RightRingDistal;
            case HumanBodyBones.RightRingDistal: return HumanBodyBones.LeftRingDistal;

            // Little
            case HumanBodyBones.LeftLittleProximal: return HumanBodyBones.RightLittleProximal;
            case HumanBodyBones.RightLittleProximal: return HumanBodyBones.LeftLittleProximal;
            case HumanBodyBones.LeftLittleIntermediate: return HumanBodyBones.RightLittleIntermediate;
            case HumanBodyBones.RightLittleIntermediate: return HumanBodyBones.LeftLittleIntermediate;
            case HumanBodyBones.LeftLittleDistal: return HumanBodyBones.RightLittleDistal;
            case HumanBodyBones.RightLittleDistal: return HumanBodyBones.LeftLittleDistal;

            // ... add additional mappings for other bones as needed

            default: return HumanBodyBones.LastBone;
        }
    }
}