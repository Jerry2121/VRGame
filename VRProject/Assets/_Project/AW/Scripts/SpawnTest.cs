﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnTest : MonoBehaviour
{

    public GameObject pref;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrefabUtility.InstantiatePrefab(pref);
        }
#endif
        if (Input.GetKeyDown(KeyCode.O))
        {
            Instantiate(pref);
        }
    }
}
