using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.OldNetworking
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class LobbyUIManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public void JoinLANGame()
        {
            NetworkingManager.Instance.JoinLANGame();
        }

        public void CreateLANGame()
        {
            NetworkingManager.Instance.CreateLANGameAsHost();
        }
    }
}