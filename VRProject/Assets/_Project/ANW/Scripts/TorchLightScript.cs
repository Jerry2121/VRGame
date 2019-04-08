using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLightScript : MonoBehaviour
{
    public ParticleSystem fireParticle;
    public ParticleSystem fireParticleAdd;
    public ParticleSystem fireParticleGlow;
    public Light spotLight;
    public Light pointLight;

    Color32 originColor;

    int colorChange;
    bool colorChanging;

    bool lightFlicker = false;
    public float lightFlickerDuration;
    float lightFlickerTimer;
    public float lightFlickerCooldown;

    public float lightFlickerCheck;
    public float lightFlickerCheckTimer;


    void Start()
    {
        originColor = new Color32(240, 155, 25, 255);
    }

    void Update()
    {
        if (lightFlicker && lightFlickerTimer >= lightFlickerDuration)
        {
            spotLight.intensity += 0.50f;
            pointLight.intensity += 0.50f;
            lightFlicker = false;
        }

        else if (!lightFlicker && lightFlickerTimer >= lightFlickerCooldown)
        {
            lightFlickerCheckTimer += Time.deltaTime;

            if (lightFlickerCheckTimer >= 1.0f)
            {
                int chance = Random.Range(1, 101);
                if (chance > 90)
                {
                    lightFlickerTimer = 0;
                    lightFlicker = true;
                    spotLight.intensity -= 0.50f;
                    pointLight.intensity -= 0.50f;
                    Debug.Log("flicker");
                }

                lightFlickerCheckTimer = 0;
            }
        }

        else
        {
            lightFlickerTimer += Time.deltaTime;
        }
    }
}
