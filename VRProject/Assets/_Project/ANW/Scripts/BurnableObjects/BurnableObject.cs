using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class BurnableObject : MonoBehaviour
{
    // Is this a player?
    public bool playerObject;
    // How long the player will burn annoyingly for.
    public float playerBurnDuration;
    // how long the player has been burning for already.
    [SerializeField]
    float playerBurningDuration;

    // How long the object has been in contact with fire.
    public float burnTimer;
    // How long the object needs to stay in contact with fire to ignite.
    public float burnPoint;
    [SerializeField]
    // Is the object burning?
    bool burningDown;
    // How long has the object BEEN burning?    
    float burnDownTimer;
    // How long does the object need to burn until it destroys itself?
    public float burnDownTime;

    // If the object came in contact with fire, but the fire left, after this period of time reset the resetTimer
    public float resetTime;
    // How long has the object NOT been in contact with fire?
    float resetTimer;
    // Checks to see if the resetTimer has started
    bool resetTimerStarted;

    public GameObject destroyedVersion;
    public GameObject particleEffects;
    public GameObject burnRadius;

    private void Update()
    {
        // If the object is already burning, check to see if it's burned long enough to destroy itself, check nothing else.
        if (burningDown)
        {
            if (!playerObject)
            {
                burnDownTimer += Time.deltaTime;
                if (burnDownTimer > burnDownTime)
                {
                    burningDown = false;
                    Destroy(this.gameObject);
                    if (destroyedVersion != null)
                    {
                        Instantiate(destroyedVersion, transform.position, transform.rotation);
                    }
                }
            }

            else if (playerObject)
            {
                playerBurningDuration += Time.deltaTime;
                if (playerBurningDuration > playerBurnDuration)
                {
                    particleEffects.SetActive(false);
                    burnRadius.SetActive(false);
                    burningDown = false;
                    playerBurningDuration = 0;
                }
            }
                return;
        }


        // If the object has come into contact with fire, start the resetTimer.
        // If the object STAYS in contact with fire, reset the resetTimer so that it will continue to burn.
        if (burnTimer > 0 && !resetTimerStarted)
        {
            resetTimer = resetTime;
            resetTimerStarted = true;
            Debug.Log(burnTimer);
        }

        // Checks to see if the resetTimer has started,
        else if(resetTimerStarted)
        {
            resetTimer -= Time.deltaTime;
        }

        else if (resetTimer < 0)
        {
            resetTimer = 0;
            burnTimer = 0;
            resetTimerStarted = false;
        }

        if (burnTimer >= burnPoint)
        {
            burnTimer = 0;
            burningDown = true;
            Burn();
        }
    }

    // Sets burn particle effect to true
    public void Burn()
    {
        particleEffects.SetActive(true);
        burnRadius.SetActive(true);
    }
}
