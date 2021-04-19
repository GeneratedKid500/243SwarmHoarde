using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLoader : MonoBehaviour
{
    public Transform parentOverride;

    public void LoadWeaponModel(GameObject model, bool set)
    {

        if (!set)
        {
            //Unload
            model.SetActive(false);
            return;
        }

        //Load
        model.SetActive(true);

        if (parentOverride != null)
        {
            model.transform.parent = parentOverride;
        }

        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;
        model.transform.localScale = Vector3.one;
    }
}
