using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    
    //Components for avoidance or collision
    public SpriteInfo info;
    //Vectors for movement
    public Vector3 position;
    public Vector3 acceleration;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 center;
    //floats
    public float mass;
    public float maxSpeed;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        info = gameObject.GetComponent<SpriteInfo>();
        velocity = new Vector3(0, 0, 0);
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        center = info.center;
        MovementForm();
        CalcSteeringForces();
        Steer();
    }

    //Applies force to vehicle to move it
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    //Handles the basic movement formula
    public void MovementForm()
    {
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        direction = velocity.normalized;
        if (direction != Vector3.zero && gameObject.GetComponent<PathFinder>())
        {
            transform.forward = new Vector3(direction.x, 0, direction.z);
        }
        else if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
        acceleration = Vector3.zero;
        transform.position = position;
    }

    //Seeks a position
    public Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = targetPosition - position;
        desiredVelocity.Normalize();
        desiredVelocity = desiredVelocity * maxSpeed;
        Vector3 seekingForce = desiredVelocity - velocity;
        return seekingForce;
    }

    //Overload that seeks a gameobject
    public Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    //Keeps object on floorspace by steering towards center
    public void Steer()
    {
        if (position.x > 65 || position.x < 5 || position.z > 65 || position.z < 5 || position.y > 46 || position.y < 0)
        {
            ApplyForce(Seek(new Vector3(30, 150, 30)) * 30);
        }
    }

    //Avoids obstacle by returning desired steering force
    protected Vector3 AvoidObstacle(GameObject obstacle, float safeDistance)
    {
        //Info for obstacle avoidance
        Vector3 vecToCenter = obstacle.transform.position - position;
        float dotForward = Vector3.Dot(vecToCenter, transform.forward);
        float dotRight = Vector3.Dot(vecToCenter, transform.right);
        float radiiSum = obstacle.GetComponent<Obstacle>().radius + radius;

        //Step 1: check for front or back
        if (dotForward < 0)
        {
            return Vector3.zero;
        }
        //Step 2: check for closeness
        if (vecToCenter.magnitude > safeDistance)
        {
            return Vector3.zero;
        }
        //Step 3: check for right or left
        if (radiiSum < Mathf.Abs(dotRight))
        {
            return Vector3.zero;
        }
        //Steer away from object
        Vector3 desiredVelocity;
        if (dotRight < 0) //Turn left
        {
            desiredVelocity = transform.right * maxSpeed * 25; //Scaled up for noticability
        }
        else //Turn right
        {
            desiredVelocity = -transform.right * maxSpeed * 5;
        }

        //returns steering force
        Vector3 steeringForce = desiredVelocity - velocity;
        return steeringForce;
    }

    //Keeps vehicle seperate from neighboring vehicles
    protected Vector3 Seperate(List<GameObject> partners)
    {
        Vector3 desiredVelocity = Vector3.zero;
        //Info for seperation
        if (partners.Count == 0)
        {
            return desiredVelocity;
        }

        foreach (GameObject g in partners)
        {
            int turn = Random.Range(1, 3);
            Vector3 vecToCenter = g.transform.position - position;
            if (vecToCenter.magnitude < 3 && g != gameObject)
            {
                Vector3 movingTowards = this.position - g.transform.position;
                if (movingTowards.magnitude > 0)
                {
                    desiredVelocity += (movingTowards.normalized / movingTowards.magnitude) * 5; //generates force away from the neighbor object
                }
            }
        }
        return desiredVelocity;
    }

    //Sets up flock alignment so they all face same direction
    protected Vector3 Alignment(List<GameObject> partners)
    {
        Vector3 ultForce = Vector3.zero;
        if (partners.Count == 0)
        {
            return ultForce;
        }
        foreach(GameObject g in partners)
        {
            ultForce += g.GetComponent<Vehicle>().direction; //Calculate avg direction
        }
        ultForce = ultForce.normalized * maxSpeed;
        return ultForce - velocity;
    }

    //Objects seek a center of the flock
    protected Vector3 Cohesion(List<GameObject> partners)
    {
        Vector3 averageCenter = Vector3.zero;
        foreach (GameObject g in partners)
        {
            averageCenter += g.transform.position;

        }
        averageCenter = averageCenter / partners.Count; //Calculate average position
        return Seek(averageCenter);
    }


    //Abstract method for use by children
    public abstract void CalcSteeringForces();

}
