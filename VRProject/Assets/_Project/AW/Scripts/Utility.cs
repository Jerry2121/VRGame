using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Utility
{
    /// <summary>
    /// Will set all children of an object to a specified layer, along with the children's children.
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_newLayer"></param>
    public static void SetLayerRecursively(GameObject _obj, int _newLayer)
    {
        if (_obj == null)
            return;

        _obj.layer = _newLayer;

        foreach (Transform child in _obj.transform)
        {
            if (child == null)
                continue;
            SetLayerRecursively(child.gameObject, _newLayer);
        }

    }

    public static bool IsObjectGrabbed(GameObject obj)
    {
        OVRGrabbable ovrGrabbable = obj.GetComponent<OVRGrabbable>();
        if (ovrGrabbable != null)
            return ovrGrabbable.isGrabbed;
        else return false;
    }

}
