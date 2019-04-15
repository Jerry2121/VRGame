using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "VRGame/SceneAssets")]
public class NetworkScenes : ScriptableObject
{
    public Scene mainMenuScene;
    public Scene[] gameScenes;
}
