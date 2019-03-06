using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(TempPlayer))]
[RequireComponent(typeof(TempPlayerController))]
public class TempPlayerSetup : NetworkBehaviour
{

    //components to disable on non-local players
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;
    [SerializeField]
    GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;

    // Use this for initialization
    void Start()
    {
        //if the object isn't controlled by the local player
        if (!isLocalPlayer && NetworkingManager.initialized)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            //Disable player graphics for local player
            Utility.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //Create player UI
            //playerUIInstance = Instantiate(playerUIPrefab);
            //playerUIInstance.name = playerUIPrefab.name;

            //Configure playerUI
            //PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            /*if (ui == null)
            {
                Debug.LogError("PlayerSetup -- Start: No PlayerUI on PlayerUI prefab");
                return;
            }
            ui.SetPlayer(GetComponent<Player>());
            */
            //GetComponent<Player>().SetupPlayer();
            
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        TempPlayer player = GetComponent<TempPlayer>();

        if (Debug.isDebugBuild)
            Debug.Log("PlayerSetup -- OnStartClient");

        NetworkingManager.Instance.RegisterPlayer(netID, player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void OnDisable()
    {
        Destroy(playerUIInstance);

        //if (isLocalPlayer)
            //GameManager.Instance.SetSceneCameraActiveState(true);

        //GameManager.UnregisterPlayer(transform.name);

    }

}

