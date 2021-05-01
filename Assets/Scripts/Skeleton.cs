using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed;
    public float thrust;

    private Transform player;
    private float dis;
    private bool stalking;
    private bool attacking;
    

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        stalking = true;
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        //print("The distance to the player is " + dis);
        dis = Vector3.Distance(transform.position, player.transform.position);
        if (dis < 15f)
        {
            //charges up attack for 1s then lunges forward towards player 
            if (!attacking)
            {
                rb.drag = 0f;
                rb.mass = 1f;
                rb.isKinematic = true;
                stalking = false;
                StartCoroutine(Attack());
            }
            if (attacking)
            {
                rb.isKinematic = false;
            }  
        }
        else if (dis > 15)
        {
            rb.drag = 0f;
            rb.mass = 1f;
            stalking = true;
        }
    }
    private void FixedUpdate()
    {
        if (!attacking)
        {
            transform.LookAt(player);
        }
        
        if (stalking)
        {
            //move towards player using vector2.movetowards
           transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (attacking)
        {
            rb.AddForce(transform.forward * thrust * Time.fixedDeltaTime, ForceMode.Impulse);
            StartCoroutine(StopLunge());
        }
    }


    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);
        attacking = true;
        //add force to the object to lunge it towards the player
        
    }
    IEnumerator StopLunge()
    {
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(-rb.velocity * rb.mass, ForceMode.Impulse);
        attacking = false;
        rb.isKinematic = true;
    }
}
