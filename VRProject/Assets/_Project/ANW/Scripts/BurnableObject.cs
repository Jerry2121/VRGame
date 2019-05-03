using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{
    // I think we'll need a referenced particle effect? The server doesn't like that though.
    public float burnTimer;
    public float burnPoint;

    bool burningDown;
    float burnDownTimer;
    public float burnDownTime;

    public float resetTime;
    float resetTimer;
    bool resetTimerStarted;

    public GameObject destroyedVersion;
    public GameObject particleEffects;

    private void Update()
    {
        if (burningDown)
        {
            burnDownTimer += Time.deltaTime;
            if (burnDownTimer > burnDownTime)
            {
                Instantiate(destroyedVersion, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
            return;
        }

        if (burnTimer > 0 && !resetTimerStarted)
        {
            resetTimer = resetTime;
            resetTimerStarted = true;
            Debug.Log(burnTimer);
        }

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
            burningDown = true;
            Burn();
        }
    }
    public void Burn()
    {
        particleEffects.SetActive(true);
    }
}
