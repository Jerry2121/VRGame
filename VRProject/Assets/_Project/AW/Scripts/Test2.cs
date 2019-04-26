using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using VRGame.Networking;
using Unity.Mathematics;

public class Test2 : MonoBehaviour
{
    public GameObject spawn;

    // Start is called before the first frame update
    void Start()
    {
        Logger.Instance.Init();

        //quaternion quat = transform.rotation;

        //Debug.Log(string.Format("A: x={0} y={1} z={2} w={3}", quat.value.x, quat.value.y, quat.value.z, quat.value.w));

        //float3 roundedRot = new float3();

        //roundedRot.x = quat.value.x;
        //roundedRot.y = quat.value.y;
        //roundedRot.z = quat.value.z;

        //quaternion quatTwo = quaternion.Euler(roundedRot);

        //Debug.Log(string.Format("B: x={0} y={1} z={2} w={3}", quatTwo.value.x, quatTwo.value.y, quatTwo.value.z, quatTwo.value.w));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn();
        }
    }
    
    void Spawn()
    {
        float foo = UnityEngine.Random.Range(-10, 10);
        float boo = UnityEngine.Random.Range(-10, 10);
        NetworkingManager.s_Instance.InstantiateOverNetwork(spawn.GetComponent<NetworkObject>().m_ObjectName, foo, 0, boo);
    }

    [ExecuteAlways]
    private void OnValidate()
    {
        Debug.Log("OnValidate");
    }

}

