using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class LeverScript : MonoBehaviour
{
    public float MaxZ;
    public float MinZ;
    Vector3 LevelPos;
    // Start is called before the first frame update
    void Start()
    {
        LevelPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float3 pos = transform.position;

        pos.z = Mathf.Clamp(pos.z, MinZ, MaxZ);

        transform.position = pos;
        //if (this.transform.position.z >= MaxZ)
        //{
        //    this.transform.position = new Vector3(LevelPos.x, LevelPos.y, MaxZ);
        //}
        //if (this.transform.position.z <= MinZ)
        //{
        //    this.transform.position = new Vector3(LevelPos.x, LevelPos.y, MinZ);
        //}
    }
}
