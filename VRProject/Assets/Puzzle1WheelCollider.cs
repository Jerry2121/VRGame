﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1WheelCollider : MonoBehaviour
{
    public int Spins;
    public bool Pos1;
    public bool Pos2;
    public bool Pos3;
    public GameObject Pos1c;
    public GameObject Pos2c;
    public GameObject Pos3c;
    public GameObject Pos4c;
    // Start is called before the first frame update
    void Start()
    {
        Pos1 = false;
        Pos2 = false;
        Pos3 = false;
        Pos1c.SetActive(true);
        Pos2c.SetActive(false);
        Pos3c.SetActive(false);
        Pos4c.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "P1HandlePos1" && GetComponent<Puzzle1WheelController>().grabbed)
        {
            Pos1 = true;
            Pos2 = false;
            Pos3 = false;
            Pos1c.SetActive(false);
            Pos2c.SetActive(true);
            Pos3c.SetActive(false);
            Pos4c.SetActive(false);
        }
        if (other.gameObject.tag == "P1HandlePos2" && GetComponent<Puzzle1WheelController>().grabbed && Pos1 == true)
        {
            Pos1 = true;
            Pos2 = true;
            Pos3 = false;
            Pos1c.SetActive(false);
            Pos2c.SetActive(false);
            Pos3c.SetActive(true);
            Pos4c.SetActive(false);
        }
        if (other.gameObject.tag == "P1HandlePos3" && GetComponent<Puzzle1WheelController>().grabbed && Pos1 == true && Pos2 == true)
        {
            Pos1 = true;
            Pos2 = true;
            Pos3 = true;
            Pos1c.SetActive(false);
            Pos2c.SetActive(false);
            Pos3c.SetActive(false);
            Pos4c.SetActive(true);
        }
        if (other.gameObject.tag == "P1HandlePos4" && GetComponent<Puzzle1WheelController>().grabbed && Pos1 == true && Pos2 == true && Pos3 == true)
        {
            Pos1 = false;
            Pos2 = false;
            Pos3 = false;
            Pos1c.SetActive(true);
            Pos2c.SetActive(false);
            Pos3c.SetActive(false);
            Pos4c.SetActive(false);
            Spins++;
        }
    }
}