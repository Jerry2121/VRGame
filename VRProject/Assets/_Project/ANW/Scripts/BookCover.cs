using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCover : MonoBehaviour
{
    public Material[] bookCovers;

    // Start is called before the first frame update
    void Start()
    {
        Material[] tempMaterials = gameObject.GetComponent<MeshRenderer>().materials;
        tempMaterials[1] = bookCovers[Random.Range(1, bookCovers.Length)];
        gameObject.GetComponent<MeshRenderer>().materials = tempMaterials;
    }
}
