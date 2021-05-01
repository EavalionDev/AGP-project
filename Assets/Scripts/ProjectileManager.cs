using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public List<GameObject> availableFireballs = new List<GameObject>();
    public List<GameObject> usedFireballs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If all the firreballs available have been used re-populate the list
        if (usedFireballs.Count == 0)
        {
            foreach(GameObject fireballs in usedFireballs)
            {
                usedFireballs.Remove(fireballs);
                availableFireballs.Add(fireballs);
            }
        }
    }
}
