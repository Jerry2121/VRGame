using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateClient : MonoBehaviour
{

    /// Whether to show the default control HUD at runtime.
    /// </summary>
    [SerializeField] public bool showGUI = true;
    /// <summary>
    /// The horizontal offset in pixels to draw the HUD runtime GUI at.
    /// </summary>
    [SerializeField] public int offsetX;
    /// <summary>
    /// The vertical offset in pixels to draw the HUD runtime GUI at.
    /// </summary>
    [SerializeField] public int offsetY;


    List<ClientBehaviour> localClients = new List<ClientBehaviour>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewClient()
    {
        ClientBehaviour client = gameObject.AddComponent<ClientBehaviour>();
        localClients.Add(client);
    }

    void DestroyFinishedClients()
    {
        foreach(var client in localClients)
        {
        }
    }

    private void OnGUI()
    {
        if (!showGUI)
            return;

        int xpos = 10 + offsetX;
        int ypos = 40 + offsetY;
        const int spacing = 24;

        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Create Client"))
        {
            CreateNewClient();
        }
        ypos += spacing; //will move the next element down

        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "TestButton"))
        {
        }
        string stringFromTextField = string.Empty;
        stringFromTextField = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), stringFromTextField);
        ypos += spacing;
    }

}
