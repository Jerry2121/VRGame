using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNonNetworkedPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject sceneCamera;

    // Start is called before the first frame update
    void Start()
    {
        if(NetworkingManager.initialized == false && playerPrefab != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            if (sceneCamera != null)
                sceneCamera.SetActive(false);
        }
    }
}
