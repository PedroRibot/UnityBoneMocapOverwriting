using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoneCorrections
{
    public HumanBodyBones bone;
    public Quaternion rotation;
}

[CreateAssetMenu(menuName = "BoneOverwrite/Bones that overwrite", fileName = "New bones to correct")]
public class BoneCorrectionHandler : ScriptableObject
{
    public BoneCorrections[] corrections;

    public void Execute(Animator animator)
    {
        Transform bone = null;

        foreach (BoneCorrections correction in corrections)
        {
            bone = animator.GetBoneTransform(correction.bone);
            if (bone != null)
            {
                bone.localRotation = correction.rotation;
            }

        }

    }


}
