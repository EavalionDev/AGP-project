using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{
    public GameObject fireballManager;
    public float rotateSpeed;
    private Transform player;

    private GameObject fireball;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        StartCoroutine(ThowFireball());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        transform.LookAt(player);
    }

    void LookAtPlayer()
    {
        
    }

    //Wait 4 seconds then launch a new fireball from the list and move it to the used list
    IEnumerator ThowFireball()
    {
        yield return new WaitForSeconds(4);
        fireball = fireballManager.GetComponent<ProjectileManager>().availableFireballs[Random.Range(0, fireballManager.GetComponent<ProjectileManager>().availableFireballs.Count)];
        fireball.transform.position = gameObject.transform.position;
        fireball.transform.LookAt(player);
        fireballManager.GetComponent<ProjectileManager>().availableFireballs.Remove(fireball);
        fireballManager.GetComponent<ProjectileManager>().usedFireballs.Add(fireball);
        fireball.SetActive(true);
        StartCoroutine(ThowFireball());
    }
}
