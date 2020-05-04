using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

[RequireComponent(typeof(Seeker))]
public class EnemyMovement : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = .2f;

    public float nextWaypointDistance = .1f;
    public bool reachedEndOfPath;

    private Path path;
    private Seeker seeker;

    private int currentWaypoint = 0;

    private void Awake()
    {
        // Get a reference to the Seeker component we added earlier
        seeker = GetComponent<Seeker>();
    }

    public void OnDisable()
    {
        seeker.pathCallback -= OnPathComplete;
        seeker.pathCallback -= OnPathUpdate;
    }

    public void Start()
    {
        // Start to calculate a new path to the targetPosition object, return the result to the OnPathComplete method.
        // Path requests are asynchronous, so when the OnPathComplete method is called depends on how long it
        // takes to calculate the path. Usually it is called the next frame.
        if(targetPosition != null)
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        // Path pooling. To avoid unnecessary allocations paths are reference counted.
        // Calling Claim will increase the reference count by 1 and Release will reduce
        // it by one, when it reaches zero the path will be pooled and then it may be used
        // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
        // take a path from the pool if possible. See also the documentation page about path pooling.
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;

            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;

            Movement(true);
        }
        else
        {
            p.Release(this);
        }
    }

    public void OnPathUpdate(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        // Path pooling. To avoid unnecessary allocations paths are reference counted.
        // Calling Claim will increase the reference count by 1 and Release will reduce
        // it by one, when it reaches zero the path will be pooled and then it may be used
        // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
        // take a path from the pool if possible. See also the documentation page about path pooling.
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;

            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;

            Movement();
        }
        else
        {
            p.Release(this);
        }
    }

    public void MoveEnemy()
    {
        if (seeker != null)
        {
            if (seeker.IsDone())
            {
                // Calculate Path
                seeker.StartPath(transform.position, targetPosition.position, OnPathUpdate);
            }
        }
    }

    private void Movement(bool isStart = false)
    {
        if (!isStart)
        {
            transform.DOMove(path.vectorPath[currentWaypoint + 1], moveSpeed, false);
        }
        
        transform.DOLookAt(path.vectorPath[currentWaypoint + 1], .2f, AxisConstraint.Y, Vector3.up);
    }

    public void Update()
    {
        if (path == null || reachedEndOfPath)
        {
            return;
        }

        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                // Stops one unit before target
                if (currentWaypoint + 1 < path.vectorPath.Count - 1)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }
}
