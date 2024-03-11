using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoneOverwriting : MonoBehaviour
{
    public BoneCorrectionHandler[] corrections;

    // Update is called once per frame
    void LateUpdate()
    {
        ExecuteCorrection();
    }

    public void ExecuteCorrection()
    {
        if (corrections == null || corrections.Length == 0)
        {
            return;
        }

        foreach (BoneCorrectionHandler correct in corrections)
        {
            correct.Execute(this.gameObject.GetComponent<Animator>());
        }
    }
}
