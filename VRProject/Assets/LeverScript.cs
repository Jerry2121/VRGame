using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public float MaxY;
    public float MinY;
    Vector3 LevelPos;
    // Start is called before the first frame update
    void Start()
    {
        LevelPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y >= MaxY)
        {
            this.transform.position = new Vector3(LevelPos.x, MaxY, LevelPos.z);
        }
        if (this.transform.position.y <= MinY)
        {
            this.transform.position = new Vector3(LevelPos.x, MinY, LevelPos.z);
        }
    }
}
