using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterAnimationController : MonoBehaviour
{
    //public GameObject P1Wheel;
    public Puzzle1WheelController m_WheelController;
    private bool ran;

    // Start is called before the first frame update
    void Start()
    {
        //P1Wheel = GameObject.Find("P1Wheel");
    }

    // Update is called once per frame
    void Update()
    {
        //if(P1Wheel == null)
        //{
        //    P1Wheel = GameObject.Find("P1Wheel(Clone)");
        //    return;
        //}
        if (ran)
            return;

        if (m_WheelController.spins == 1)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin1", true);
        }
        if (m_WheelController.spins == 2)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin2", true);
        }
        if (m_WheelController.spins == 3)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin3", true);
        }
        if (m_WheelController.spins == 4)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin4", true);
        }
        if (m_WheelController.spins == 5)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin5", true);
        }
        if (m_WheelController.spins == 6)
        {
            this.gameObject.GetComponent<Animator>().SetBool("Spin6", true);
            ran = true;
        }

    }
}
