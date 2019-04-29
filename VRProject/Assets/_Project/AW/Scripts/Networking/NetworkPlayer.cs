using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{
    [AddComponentMenu("VR Networking/Network Player")]
    public class NetworkPlayer : MonoBehaviour
    {
        [SerializeField] Camera playerCamera;
        [SerializeField] Behaviour[] m_ComponentsToDisableOnNetworkedPlayer;
        [SerializeField] Behaviour[] m_ComponentsToDisableOnLocalPlayer;

        bool m_IsLocalPlayer;
        int m_PlayerID;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DisableComponents());
        }

        // Update is called once per frame
        void Update()
        {
            if (m_IsLocalPlayer == false)
                return;
        }

        public void SetIsLocalPlayer()
        {
            if (m_IsLocalPlayer)
                Debug.LogWarning("TempPlayer -- SetIsLocalPlayer: isLocalPlayer already set! This should not be called on a setup player!");
            m_IsLocalPlayer = true;
        }

        public bool IsLocalPlayer()
        {
            return m_IsLocalPlayer;
        }

        public void SetPlayerID(int playerID)
        {
            m_PlayerID = playerID;
        }

        public void RecieveMoveMessage(float xMov, float zMov)
        {
            transform.Translate(xMov * 0.5f, 0, zMov * 0.5f);
        }

        public void RecievePositionMessage(float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

        IEnumerator DisableComponents()
        {
            yield return new WaitForSeconds(1f);

            if (NetworkingManager.s_Instance == null || NetworkingManager.s_Instance.IsConnected() == false)
                yield break;

            if (m_IsLocalPlayer)
            {
                foreach (var comp in m_ComponentsToDisableOnLocalPlayer)
                {
                    Debug.Log("Disabling " + comp.name);
                    comp.enabled = false;
                }
            }
            else
            {
                foreach (var comp in m_ComponentsToDisableOnNetworkedPlayer)
                {
                    Debug.Log("Disabling " + comp.name);
                    comp.enabled = false;
                }

                if(playerCamera != null)
                    playerCamera.targetDisplay = 8;

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                }
            }
        }

    }
}
