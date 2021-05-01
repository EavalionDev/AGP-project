using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject customShape;
    public GameObject centrePoint;
    public GameObject meshGenerator;
    public float thrust;
    public float slowDownSpeed;
    public float rotateSpeed;
    public GameObject trailObj;
    public Transform rear;
    public List<GameObject> avaliableCols = new List<GameObject>();
    public List<GameObject> usedCols = new List<GameObject>();
    public List<GameObject> avaliableLockInParticles = new List<GameObject>();
    public List<GameObject> usedLockInParticles = new List<GameObject>();

    private Collider[] hitColliders;
    private Transform tr;
    private Rigidbody rb;
    private bool forward;
    private bool right;
    private bool left;
    private bool trail;
    private bool collidedWithTrail;
    private bool getPlayerPos;
    private bool adjustTrail;
    private bool lockInParticlesEnabled;
    private Vector3 velocityMin;
    private GameObject chosenCol;
    private GameObject chosenTrailVisual;
    private GameObject chosenLockInVisual;
    private GameObject hitCol;
    private bool playerStatic;
    private Vector3 playerPos;
    private Vector3 playerOldPos;
    private int segmentValue;
    private int segmentCollidedWithNumber;
    private int lockInVisualSegmentValue;
    private int TrailLockInNumber;
    // Start is called before the first frame update
    void Start()
    {
        lockInVisualSegmentValue = 0;
        adjustTrail = false;
        segmentCollidedWithNumber = 0;
        segmentValue = 0;
        playerOldPos = transform.position;
        playerStatic = false;
        collidedWithTrail = false;
        trail = false;
        velocityMin = new Vector3(0, 0, 0);
        right = false;
        left = false;
        forward = false;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        //pooledObjects = new List<GameObject>();
        foreach (GameObject col in avaliableCols)
        {
            col.gameObject.SetActive(false);
        }
    }

    // FIND MID POINT OF ALL ON SCREEN POINTS (GIVE THEM A VALUE TO STATE THEY WAS USED/ MAYBE ASSIGN TAGS TO THE OBJECT)  - USE RAYCASTING TO COLLISION DETECT WITHIN THE SHAPE
       //NEW TO DO: MAKE IT SO THAT THE TRAIL GETS DISABLED EVEN IF SPACE IS HELD DOWN AFTER A SHAPE IS MADE
       // CONNECT ANY GAPS IN THE SHAPE
       // LOOK INTO CREATING A CUSTOM MESH COLLIDER WITHIN THE SHAPE

    // Update is called once per frame
    void Update()
    {
        //HOLDING AND RELEASING FORWARD KEY
        if (Input.GetKey(KeyCode.W))
        {
            forward = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            forward = false;
        }

        //LEFT AND RIGHT KEYS
        if (Input.GetKey(KeyCode.D))
        {
            right = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            right = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            left = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            left = false;
        }


        //HOLDING SPACE DOWN
        if (Input.GetKey(KeyCode.Space))
        {
            trail = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (lockInParticlesEnabled)
            {
                trail = false;
                TrailIsNowOff();
            }
            else
            {
                trail = false;
                TrailIsNowOff();
                ReturnLockedInParticles();
            } 
        }

        if (adjustTrail)
        {
            //Make a reference to the collided with trail number, for every trail number below that value get rid of and move it back into the avaliable list
            segmentCollidedWithNumber = hitCol.GetComponent<TrailSegment>().segmentNumber;
            foreach (GameObject cols in usedCols.ToArray())
            {
                if (cols.GetComponent<TrailSegment>().segmentNumber < segmentCollidedWithNumber)
                {
                    cols.gameObject.SetActive(false);
                    usedCols.Remove(cols);
                    avaliableCols.Add(cols);
                    
                }
                else
                {
                    //After the trail has been adjusted copy the used segments and add them to the polygon tester list to use as it's vertice points
                    //meshGenerator.GetComponent<PolygonTester>().usedTrailSegments.Add(cols);
                    //meshGenerator.GetComponent<PolygonTester>().UpdateVerticesList();
                    adjustTrail = false;
                    //break;
                }
                
            }

            //Do the same as above to the locked in trail visuals
            foreach (GameObject trailLockIn in usedLockInParticles.ToArray())
            {
                if (trailLockIn.GetComponent<LockInTrail>().LockInNumber < segmentCollidedWithNumber)
                {
                    trailLockIn.SetActive(false);
                    usedLockInParticles.Remove(trailLockIn);
                    avaliableLockInParticles.Add(trailLockIn);
                }
            }
        }
        
    }
    private void FixedUpdate()
    {

        if (trail)
        {
            //Check the distance from the previous position to new position and if its greater than 1 place a trail segment
            float dist = Vector3.Distance(playerOldPos, transform.position);
            if (dist > 1f)
            {
                playerStatic = false;
                GetPlayerPos();
            }
            else if (dist < 1f)
            {
                playerStatic = true;
            }
                
             //gameobject gets set to a random col in the list, moved, enabled then moved to other list
             //segments number value gets assigned
             if (avaliableCols.Count != 0 && !playerStatic)
             {
                 chosenCol = avaliableCols[Random.Range(0, avaliableCols.Count)];
                 chosenCol.transform.position = rear.transform.position;
                 chosenCol.GetComponent<TrailSegment>().segmentNumber = segmentValue++;
                 if (chosenCol.GetComponent<BoxCollider>().enabled == false)
                 {
                    chosenCol.GetComponent<BoxCollider>().enabled = true;
                 }

                //PLACE SPELL LOCKING VISUAL ON TRAIL POSITION WITH IT DISABLED AND ENABLE THEM AT THE END OF THE TRAIL USING BOOL
                //Line below causing some errors which may be related to the shape not fully forming sometimes
                chosenLockInVisual = avaliableLockInParticles[Random.Range(0, avaliableLockInParticles.Count)];
                chosenLockInVisual.transform.position = rear.transform.position;
                chosenLockInVisual.GetComponent<LockInTrail>().LockInNumber = lockInVisualSegmentValue++;
                avaliableLockInParticles.Remove(chosenLockInVisual);
                usedLockInParticles.Add(chosenLockInVisual);

                chosenCol.SetActive(true);
                 avaliableCols.Remove(chosenCol);
                 usedCols.Add(chosenCol);
             }

             //if list becomes empty trail is over, disable objects and put them all back in 1st list
             else if (avaliableCols.Count == 0 && !collidedWithTrail )
             {
                TrailIsNowOff();
                trail = false;
             }
             
             if (collidedWithTrail)
             {
                //FindCentrePoint();
                lockInParticlesEnabled = true;
                PlayLockInParticles();
                TrailIsNowOff();
                trail = false;
             }
            
        }
        

        if (forward)
        {
            rb.velocity = transform.forward * thrust * Time.fixedDeltaTime;
        }
        if (!forward)
        {
            if (rb.velocity != Vector3.zero)
            {
                rb.velocity = -transform.forward * slowDownSpeed * Time.fixedDeltaTime;
            } 
        }

        if (right && !left)
        {
            tr.Rotate(0, 1f * rotateSpeed, 0);
        }
        if (left && !right)
        {
            tr.Rotate(0,-1f * rotateSpeed, 0);
        }
    }
    /*private void FindCentrePoint()
    {
        float totalX = 0f;
        float totalY = 0f;
        int amount = 0;

        //This calculates the centre point of the shape, currently because I move all objs in usedCols back there is no value coming through
        foreach (GameObject g in usedCols)
        {
            
            totalX += g.transform.position.x;
            totalY += g.transform.position.y;
            amount++;

            float centreX = totalX / amount;
            float centreY = totalY / amount;

            centrePoint.transform.position = new Vector3(centreX, centreY, 0);
        }
    } */

    void GetPlayerPos()
    {
        playerOldPos = transform.position;
    }

    void TrailIsNowOff()
    {
        foreach (GameObject cols in usedCols.ToArray())
        {
            cols.gameObject.SetActive(false);
            usedCols.Remove(cols);
            avaliableCols.Add(cols);
        }
        
        collidedWithTrail = false;
    }


    void PlayLockInParticles()
    {
        //This will play the particles 
        foreach (GameObject particles in usedLockInParticles)
        {
            particles.SetActive(true);
        }
        StartCoroutine(WaitOutParticlePlayTime());
    }

    IEnumerator WaitOutParticlePlayTime()
    {
        //waits until particles are finished playing
        yield return new WaitForSeconds(1);
        ReturnLockedInParticles();
    }
    void ReturnLockedInParticles()
    {
        //Sends them back to first list
        foreach (GameObject lockedInTrail in usedLockInParticles.ToArray())
        {
            lockedInTrail.SetActive(false);
            usedLockInParticles.Remove(lockedInTrail);
            avaliableLockInParticles.Add(lockedInTrail);
        }
        lockInParticlesEnabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrailCollider"))
        {
            hitCol = other.gameObject;
            foreach (GameObject col in usedCols)
            {
                col.GetComponent<BoxCollider>().enabled = false;
            }
            adjustTrail = true;
            collidedWithTrail = true;
        }
    }
}
