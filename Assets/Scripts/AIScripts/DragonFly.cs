using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DragonFly : MonoBehaviour
{

    // the starting position of the dragon fly
    private Vector3 startingPosition;

    // The size of the bounds it can move within
    public Vector3 boundsSize = new Vector3(50.0f, 50.0f, 50.0f);
    public float FollowRadius = 50.0f;
    public float CheckRadius = 100.0f;
    // Determines whether to follow the player or not.
    public bool FollowingPlayer = false;
    // Player position variable
    public Transform player;

    // Movement speeds
    public float defaultSpeed;
    public float defaultRotationSpeed;
    private float speed;
    private float rotationSpeed;

    // Object Size
    private Vector3 objectSize;

    // the position to move to inside the box
    private Vector3 moveToPosition;

    // the random movement Timer
    private float randomMovementTimer;
    // the current direction of random movement
    private float randomMovement;

    // Children colliders
    private Collider[] colliders;

    public GameObject rubbishPile;

    // Start is called before the first frame update
    void Start()
    {

        this.startingPosition = this.transform.position;
        this.objectSize = this.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().bounds.max;
        //this.boundsSize -= objectSize;

        this.colliders = this.GetComponentsInChildren<Collider>();

        this.speed = defaultSpeed;
        this.rotationSpeed = rotationSpeed;

        player = GameObject.FindWithTag("Player").transform;

    }

    private void FixedUpdate()
    {
        // Check for surrounding objects
        CheckSurroundings();
    }

    // Update is called once per frame
    void Update()
    {

        // Check if we should follow the player
        CheckPlayer();

        if (FollowingPlayer && moveToPosition == Vector3.zero)
            FollowPlayer();
        else
            RandomlyMoveWithinBounds();


    }

    /// <summary>
    /// Checks the distance from the player.
    /// </summary>
    private void CheckPlayer()
    {

        // if the player is within the range then tell the fly to follow the player
        if (CheckDistance(player.position) < CheckRadius)
        {
            FollowingPlayer = true;
        }

    }

    /// <summary>
    /// Checks the distance from this object and target.
    /// </summary>
    /// <returns>The distance.</returns>
    /// <param name="position">Position.</param>
    private float CheckDistance(Vector3 position)
    {

        return Vector3.Distance(this.transform.position, position);

    }

    /// <summary>
    /// Follows the player.
    /// </summary>
    private void FollowPlayer()
    {

        // Stay within radius of the player
        if (CheckDistance(player.transform.position) > FollowRadius)
        {

            // if we're going to collide
            //if (GoingToCollide(this.transform.forward))
            //{
            //    randomMovementTimer = Time.time - 1.0f;
            //}
            //else
            //{
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
            //}

        }

        // always look at the player
        Quaternion direction = Quaternion.LookRotation(player.position - transform.position);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, rotationSpeed * Time.deltaTime);


        // Move around randomly
        if (Time.time > randomMovementTimer)
        {
            randomMovementTimer += Time.time + UnityEngine.Random.Range(2, 5);
            randomMovement = UnityEngine.Random.Range(0, 5);
        }
        else
        {

            Vector3 movementDirection = Vector3.zero;

            switch (randomMovement)
            {
                case 0:
                    // do nothing
                    break;
                case 1:
                    // move up
                    movementDirection = this.transform.up;
                    break;
                case 2:
                    // move down
                    movementDirection = -this.transform.up;
                    break;
                case 3:
                    // move right
                    movementDirection = this.transform.right;
                    break;
                case 4:
                    // move left
                    movementDirection = -this.transform.right;
                    break;
            }

            //// if we're going to collide
            //if (GoingToCollide(movementDirection))
            //{
            //    randomMovementTimer = Time.time - 1.0f;
            //}
            //else
            //{
                this.transform.position += movementDirection * Time.deltaTime * speed;
            //}

        }

    }

    /// <summary>
    /// Randomly the move within bounds.
    /// </summary>
    private void RandomlyMoveWithinBounds()
    {

        // if we don't have a moveTo position
        if (moveToPosition == Vector3.zero)
        {
            
            // Assign a moveTo position within the bounds
            float randomX = UnityEngine.Random.Range(startingPosition.x - boundsSize.x, startingPosition.x + boundsSize.x);
            float randomY = UnityEngine.Random.Range(startingPosition.y - boundsSize.y, startingPosition.y + boundsSize.y);
            float randomZ = UnityEngine.Random.Range(startingPosition.z - boundsSize.z, startingPosition.z + boundsSize.z);

            moveToPosition = new Vector3(randomX, randomY, randomZ);

        }
        // We have a moveTo position
        else
        {

            // Check if we're at the position
            if (CheckDistance(moveToPosition) < 2.0f)
            {
                moveToPosition = Vector3.zero;
            }
            else
            {

                // Move to the position
                Quaternion direction = Quaternion.LookRotation(moveToPosition - this.transform.position);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, rotationSpeed * Time.deltaTime);

                //// if we're going to collide
                //if (GoingToCollide(this.transform.forward))
                //{
                //    moveToPosition = Vector3.zero; // Reset movement
                //}
                //else
                //{
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                //}

            }

        }

    }

    // Stop movement
    private void OnCollisionEnter(Collision collision)
    {
        // if the object is the salamander
        if (collision.collider.gameObject.transform.root.gameObject.layer == 12)
        {
            Debug.Log("disabling colliders");
            this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled = false;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        // if the object is the salamander
        if (collision.collider.gameObject.transform.root.gameObject.layer == 12)
        {
            Debug.Log("enabling colliders");
            this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled = true;
        }
    }

    ///// <summary>
    ///// Determines if we're going to run into anything in this direction.
    ///// </summary>
    ///// <returns><c>true</c>, if we run into anything, <c>false</c> otherwise.</returns>
    ///// <param name="direction">Direction.</param>
    //private bool GoingToCollide(Vector3 direction)
    //{

    //    if (Physics.Raycast(this.transform.position, direction, 2.0f, 11))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    /// <summary>
    /// Check the surroundings for object
    /// </summary>
    private void CheckSurroundings()
    {
        
        RaycastHit hit;

        if (Physics.BoxCast(this.transform.position, new Vector3(7.5f, 7.5f, 7.5f), transform.forward, out hit, transform.rotation, 5.0f))
        {

            // if the object is not in the projectile layer or ignore layer or isn't this object
            if((hit.collider.gameObject.layer != 9 && hit.collider.gameObject.layer != 2 && hit.collider.gameObject.transform.root != this.gameObject.transform.root))
            {

                if (hit.collider.gameObject.transform.root.gameObject.layer == 12)
                {
                    //Debug.Log("disable collider");
                    this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled = false;
                }
                else
                {
                    if (this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled == false)
                    {
                        this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled = true;
                    }
                }

                //Debug.Log(this.name + " Moving away from " + hit.collider.name);
                moveToPosition = transform.position + hit.normal * 20.0f;
                randomMovementTimer = Time.time - 1;
                speed = 150.0f;
                rotationSpeed = 50.0f;

            }
            else
            {

                if (this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled == false)
                {
                    this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled = true;
                }
                speed = defaultSpeed;
                rotationSpeed = defaultRotationSpeed;

            }

        }
        else
        {

            if (this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled == false)
            {
                this.transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled = true;
            }
            speed = defaultSpeed;
            rotationSpeed = defaultRotationSpeed;

        }

    }

    // On Destroy(this) event.
    private void OnDestroy()
    {

        // create a rubbish pile.
        Instantiate(rubbishPile, this.transform.localPosition, this.transform.localRotation);
		//GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().timeDestroyed[transform.GetSiblingIndex()] = Time.time;

    }

#if DEBUG

    private void OnDrawGizmosSelected()
    {
        Color cubeColor = Color.yellow;
        cubeColor.a /= 2;
        Gizmos.color = cubeColor;
        Gizmos.DrawCube(startingPosition, boundsSize * 2);
    }

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a /= 2;
        Gizmos.color = color;
        Gizmos.DrawWireCube(this.transform.position + transform.forward * 0.1f, new Vector3(30, 30, 30));
    }

#endif

}