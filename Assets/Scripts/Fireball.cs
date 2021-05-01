using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject fireballManager;
    public GameObject projectileStartPoint;
    public Rigidbody rb;
    public float force;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if object is active run coroutine and addforce
        if (gameObject.activeSelf)
        {
            StartCoroutine(LifeSpan());
            rb.AddForce(transform.forward * force * Time.deltaTime, ForceMode.Impulse);
        }
        
    }
    //Wait 3 seconds then reset position and move back to previous list, disable
    public IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(3);
        transform.position = projectileStartPoint.transform.position;
        fireballManager.GetComponent<ProjectileManager>().usedFireballs.Remove(gameObject);
        fireballManager.GetComponent<ProjectileManager>().availableFireballs.Add(gameObject);
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player has been hit with a fireball");
            transform.position = projectileStartPoint.transform.position;
            fireballManager.GetComponent<ProjectileManager>().usedFireballs.Remove(gameObject);
            fireballManager.GetComponent<ProjectileManager>().availableFireballs.Add(gameObject);
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
