using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterAnimationController : MonoBehaviour
{
    public GameObject P1Wheel;
    private bool ran;
    // Start is called before the first frame update
    void Start()
    {
        P1Wheel = GameObject.Find("P1Wheel");
    }

    // Update is called once per frame
    void Update()
    {
        if(P1Wheel == null)
        {
            P1Wheel = GameObject.Find("P1Wheel(Clone)");
            return;
        }
        if (ran)
            return;

        if (P1Wheel.GetComponent<Puzzle1WheelCollider>().Spins == 1)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin1", true);
        }
        if (P1Wheel.GetComponent<Puzzle1WheelCollider>().Spins == 2)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin2", true);
        }
        if (P1Wheel.GetComponent<Puzzle1WheelCollider>().Spins == 3)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin3", true);
        }
        if (P1Wheel.GetComponent<Puzzle1WheelCollider>().Spins == 4)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin4", true);
        }
        if (P1Wheel.GetComponent<Puzzle1WheelCollider>().Spins == 5)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin5", true);
        }
        if (P1Wheel.GetComponent<Puzzle1WheelCollider>().Spins == 6 && !ran)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin6", true);
            ran = true;
        }

    }
}
