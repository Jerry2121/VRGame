using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using VRGame.Networking;

public class PowerBreakerBehavior : NetworkObjectComponent
{
    public GameObject[] wires;

    public GameObject[] buttonsNumeric;
    public int[] buttonIDsNumeric;
    public GameObject[] buttonsColored;
    public int[] buttonIDsColored;

    public int correctMatchesCompleted;

    public GameObject numericButtonPressed = null;
    public int numericButtonPressedID;
    public GameObject coloredButtonPressed;
    public int coloredButtonPressedID;

    public Material powerBreakerMaterial;
    public Material[] buttonLightsMaterials;
    public GameObject[] buttonLights;
    public Color32[] buttonLightsColors;

    public bool puzzleCompleted;

    public override NetworkObject m_NetworkObject { get; protected set; }
    public override int ID { get => m_ID; protected set => m_ID = value; }
    [HideInNormalInspector]
    [SerializeField]
    int m_ID;

    private void Start()
    {
        m_NetworkObject = GetNetworkObjectForObject(this.transform);
        RegisterSelf();
    }

    private void Update()
    {
        if (correctMatchesCompleted == 5)
        {
            if (!puzzleCompleted)
            {
                CompletePuzzle();
            }   
            return;
        }

        if (numericButtonPressed != null && coloredButtonPressed != null)
        {
            CheckButtonMatch();
        }
    }
    public void CheckButtonMatch()
    {
        if (numericButtonPressedID == coloredButtonPressedID)
        {
            correctMatchesCompleted++;
            InstantiateWire(numericButtonPressed, coloredButtonPressed);

            numericButtonPressed.GetComponent<ButtonBehavior>().matchFound = coloredButtonPressed;
            coloredButtonPressed.GetComponent<ButtonBehavior>().matchFound = numericButtonPressed;

            numericButtonPressed = null;
            coloredButtonPressed = null;

            SendNetworkMessage(NetworkTranslater.CreatePuzzleProgressMessage(NetworkingManager.ClientID(), m_NetworkObject.m_ObjectID, m_ID, numericButtonPressedID));
        }

        else if (numericButtonPressedID != coloredButtonPressedID)
        {
            numericButtonPressed = null;
            coloredButtonPressed = null;
        }
    }

    public void PaintLights()
    {
        Material[] tempMaterials = gameObject.GetComponent<MeshRenderer>().materials;

        for (int i = 1; i < gameObject.GetComponent<MeshRenderer>().materials.Length - 1; i++)
        {
            int tempInt = buttonIDsColored[i - 1];
            tempMaterials[i] = buttonLightsMaterials[tempInt];
            buttonLights[i - 1].SetActive(true);
            buttonLights[i - 1].GetComponent<Light>().color = buttonLightsColors[tempInt];
        }

        tempMaterials[0] = buttonLightsMaterials[0];
        tempMaterials[6] = powerBreakerMaterial;
        System.Array.Reverse(tempMaterials);

        buttonLights[5].GetComponent<Light>().color = buttonLightsColors[0];

        for (int i = 0; i < buttonsNumeric.Length; i++)
        {
            buttonsNumeric[i].GetComponent<ButtonBehavior>().buttonID = buttonIDsNumeric[i];
        }

        for (int i = 0; i < buttonsColored.Length; i++)
        {
            buttonsColored[i].GetComponent<ButtonBehavior>().buttonID = buttonIDsColored[i];
        }

        gameObject.GetComponent<MeshRenderer>().materials = tempMaterials;
    }

    public void CompletePuzzle()
    {
        Material[] tempMaterial = gameObject.GetComponent<MeshRenderer>().materials;
        tempMaterial[6].color = buttonLightsColors[3];
        buttonLights[5].GetComponent<Light>().color = buttonLightsColors[3];
        for (int i = 1; i < gameObject.GetComponent<MeshRenderer>().materials.Length - 1; i++)
        {
            buttonLights[i - 1].GetComponent<Light>().color = buttonLightsColors[3];
            tempMaterial[i].color = buttonLightsColors[3];
        }
        gameObject.GetComponent<MeshRenderer>().materials = tempMaterial;
        puzzleCompleted = true;
    }

    public void ResetPowerBreaker()
    {
        SendNetworkMessage(NetworkTranslater.CreatePuzzleFailedMessage(NetworkingManager.ClientID(), m_NetworkObject.m_ObjectID, m_ID));
        ResetPuzzle();
    }

    void ResetPuzzle()
    {
        for (int i = 0; i < wires.Length; i++)
        {
            correctMatchesCompleted = 0;
            wires[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < buttonsNumeric.Length; i++)
        {
            buttonsNumeric[i].GetComponent<ButtonBehavior>().matchFound = null;
        }

        for (int i = 0; i < buttonsColored.Length; i++)
        {
            buttonsColored[i].GetComponent<ButtonBehavior>().matchFound = null;
        }
    }

    public void InstantiateWire(GameObject buttonNumber, GameObject buttonColored)
    {
        if (buttonNumber.name == "Button1" && buttonColored.name == "ButtonA")
        {
            wires[0].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button1" && buttonColored.name == "ButtonB")
        {
            wires[1].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button1" && buttonColored.name == "ButtonC")
        {
            wires[2].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button1" && buttonColored.name == "ButtonD")
        {
            wires[3].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button1" && buttonColored.name == "ButtonE")
        {
            wires[4].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button2" && buttonColored.name == "ButtonA")
        {
            wires[5].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button2" && buttonColored.name == "ButtonB")
        {
            wires[6].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button2" && buttonColored.name == "ButtonC")
        {
            wires[7].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button2" && buttonColored.name == "ButtonD")
        {
            wires[8].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button2" && buttonColored.name == "ButtonE")
        {
            wires[9].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button3" && buttonColored.name == "ButtonA")
        {
            wires[10].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button3" && buttonColored.name == "ButtonB")
        {
            wires[11].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button3" && buttonColored.name == "ButtonC")
        {
            wires[12].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button3" && buttonColored.name == "ButtonD")
        {
            wires[13].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button3" && buttonColored.name == "ButtonE")
        {
            wires[14].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button4" && buttonColored.name == "ButtonA")
        {
            wires[15].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button4" && buttonColored.name == "ButtonB")
        {
            wires[16].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button4" && buttonColored.name == "ButtonC")
        {
            wires[17].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button4" && buttonColored.name == "ButtonD")
        {
            wires[18].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button4" && buttonColored.name == "ButtonE")
        {
            wires[19].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button5" && buttonColored.name == "ButtonA")
        {
            wires[20].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button5" && buttonColored.name == "ButtonB")
        {
            wires[21].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button5" && buttonColored.name == "ButtonC")
        {
            wires[22].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button5" && buttonColored.name == "ButtonD")
        {
            wires[23].gameObject.SetActive(true);
        }

        else if (buttonNumber.name == "Button5" && buttonColored.name == "ButtonE")
        {
            wires[24].gameObject.SetActive(true);
        }
    }

    public override void RecieveNetworkMessage(string recievedMessage)
    {
        Debug.Log("PowerBreakerBehaviour: RecievedMessage");

        if (NetworkTranslater.TranslatePuzzleProgressMessage(recievedMessage, out int clientID, out int objectID, out int componentID, out int numOne))
        {
            if(Debug.isDebugBuild)
                Debug.Log("PowerBreakerBehaviour: Recieved Progress. Button ID is " + numOne);

            GameObject buttonNumbered = null;
            GameObject buttonColored = null;

            foreach (var button in buttonsNumeric)
            {
                ButtonBehavior buttonBehavior = button.GetComponent<ButtonBehavior>();
                if (buttonBehavior.buttonID == numOne && buttonBehavior.matchFound == null)
                {
                    buttonNumbered = button;
                    break;
                }
            }
            foreach (var button in buttonsColored)
            {
                ButtonBehavior buttonBehavior = button.GetComponent<ButtonBehavior>();
                if (buttonBehavior.buttonID == numOne && buttonBehavior.matchFound == null)
                {
                    buttonColored = button;
                    break;
                }
            }

            if (buttonNumbered == null && buttonColored == null)
            {
                Debug.LogError("PowerBreakerBehaviour -- RecieveNetworkMessage: Did not find buttons matching sent button ID");
                return;
            }

            correctMatchesCompleted++;

            buttonNumbered.GetComponent<ButtonBehavior>().matchFound = buttonColored;
            buttonColored.GetComponent<ButtonBehavior>().matchFound = buttonNumbered;

            InstantiateWire(buttonNumbered, buttonColored);
        }
        else if (NetworkTranslater.TranslatePuzzleFailedMessage(recievedMessage, out clientID, out objectID, out componentID))
        {
            Debug.Log("PowerBreakerBehaviour: Recieved Failure");
            ResetPuzzle();
        }
        else
        {
            Debug.LogError(string.Format("PowerBreakerBehaviour -- RecieveNetworkMessage: Unable to translate message. Message was {0}", recievedMessage));
        }
    }

    public override void SendNetworkMessage(string messageToSend)
    {
        NetworkingManager.s_Instance.SendNetworkMessage(messageToSend);
    }

    public override void SetID(int newID)
    {
        if (newID > -1)
        {
            if (Debug.isDebugBuild)
                Debug.Log(string.Format("PowerBreakerBehaviour -- SetID: ID set to {0}", newID));
            ID = newID;
        }
    }

    public override void RegisterSelf()
    {
        base.RegisterSelf();
    }

    [ExecuteAlways]
    public override void SetNetworkObject(NetworkObject newNetworkObject)
    {
        base.SetNetworkObject(newNetworkObject);
    }

    public override void Reset()
    {
        base.Reset();
    }

}
