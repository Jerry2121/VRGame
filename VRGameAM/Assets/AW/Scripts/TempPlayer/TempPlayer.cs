using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete
[RequireComponent(typeof(TempPlayerSetup))]
public class TempPlayer : NetworkBehaviour
{

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    Behaviour[] disableOnDeath;

    [SerializeField]
    GameObject[] disableGameObjectsOnDeath;

    private bool[] wasEnabled;

    [SyncVar]
    private bool isDead = false;
    public bool IsDead { get { return isDead; } protected set { isDead = value; } } //Done this way instead of "public bool isDead {get; protected set}" so that it can be set as a SyncVar

    [SerializeField]
    public GameObject PlayerUI;
    [HideInInspector]
    public bool isTheLocalPlayer;

    public PlayerNetworkInteractions networkInteractions;

    public int deaths;

    private bool firstSetup = true;

    public void SetupPlayer()
    {
        isTheLocalPlayer = true;
        networkInteractions = GetComponent<PlayerNetworkInteractions>();
        //PlayerUI = GetComponent<PlayerSetup>().playerUIInstance;
        //GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        //Tell they server a player needs to be setup on all clients
        CmdBroadcastNewPlayerSetup();
    }


    [Command] //will be called on the server
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }

    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                CmdTakeDamage(20, "Dev");
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                CmdHeal(20);
            }
        }
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        //Enable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //set gameobjects to active
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }
        //Enable collider
        Collider col = GetComponent<Collider>(); //colliders arent behaviours, so they can't be added to the array
        if (col != null)
            col.enabled = true;

        //create spawn effect
        //GameObject gfxInstance = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        //Destroy(gfxInstance, 3f);
    }

    [Command]
    public void CmdHeal(int _amount)
    {
        RpcHeal(_amount);
    }

    [ClientRpc]
    public void RpcHeal(int _amount)
    {
        currentHealth += _amount;
        if (Debug.isDebugBuild)
            Debug.Log(transform.name + " healed " + _amount + " health");
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    [Command]
    void CmdTakeDamage(int _damage, string _sourceID) //only call on player, other scripts like shoot have their own Command to call RpcTakeDamage
    {
        RpcTakeDamage(_damage, _sourceID);
    }

    [ClientRpc] //Called on all clients from the server
    public void RpcTakeDamage(int _damage, string _sourceID)
    {
        if (isDead)
            return;

        currentHealth -= _damage;

        if (Debug.isDebugBuild)
            Debug.Log(transform.name + " took damage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        deaths++;

        //disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        //Disable graphics
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }
        //disable collider
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
        //spawn death effect
        //GameObject gfxInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Destroy(gfxInstance, 3f);

        if (Debug.isDebugBuild)
            Debug.Log(transform.name + " is dead");
        Destroy(this.gameObject, 2f);
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

}
#pragma warning restore CS0618 // Type or member is obsolete
