using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids
{
    public float alignForce, cohesionForce, separationForce, perceptionRadius;
    public GameObject prefab;
    public float roomX1 = 50.3371f, roomX2 = -49.6629f, rooxY1 = 95.57827f, roomY2 = -4.521726f, roomZ1 = 50.00253f, roomZ2 = -49.99747f;
    public Vector3 position, velocity, acceleration;
    public float maxForce, maxSpeed;


    //public GameObject prefab;
    public GameObject body;
    public Rigidbody rb;

    public Boids()
    {
        this.position = new Vector3(Random.Range(roomX1, roomX2), Random.Range(rooxY1, roomY2), Random.Range(roomZ1, roomZ2));
        this.velocity = Random.insideUnitSphere;
        this.velocity *= (Random.Range(2, 4));
        this.acceleration = new Vector3();
        this.maxForce = 1;
        this.maxSpeed = 4;
    }

    public void edges()
    {

        if (this.position.x > 50.3371f)
        {
            this.position.x = -49.6629f;
            // rb.transform.position += new Vector3(-49.6629f, rb.transform.position.y, rb.transform.position.z);
        }
        else if (this.position.x < -49.6629f)
        {
            // rb.transform.position += new Vector3(50.3371f, rb.transform.position.y, rb.transform.position.z);
            this.position.x =50.3371f;
        }
        if (this.position.y > 95.57827f)
        {
            this.position.y = -4.521726f;
            //rb.transform.position+= new Vector3(rb.transform.position.x, -4.521726f, rb.transform.position.z);
        }
        else if (this.position.y < -4.521726f)
        {
            this.position.y = 95.57827f;
            //rb.transform.position += new Vector3(rb.transform.position.x, 95.57827f, rb.transform.position.z);
        }
        if (this.position.z > 50.00253f)
        {
            this.position.z = -49.99747f;
            //rb.transform.position += new Vector3(rb.transform.position.x,rb.transform.position.y,-49.99747f);
        }
        else if (this.position.z < -49.99747)
        {
            this.position.z = 50.00253f;
            //rb.transform.position += new Vector3(rb.transform.position.x, rb.transform.position.y, 50.00253f);
        }
    }

    public Vector3 align(Boids[] boids)
    {
        //perceptionRadius = 50;
        Vector3 steering = new Vector3();
        int total = 0;
        foreach (var other in boids)
        {
            float d = Vector3.Distance(this.position, other.position);
            if (other != this && d < perceptionRadius)
            {
                steering += (other.velocity);
                total++;
                
            }
        }
        if (total > 0)
        {
            
            steering /= (total);
            steering.Normalize();
            steering *= maxSpeed;
            //Vector3.ClampMagnitude(steering, this.maxSpeed);
            //steering = steering * this.maxSpeed;
            //steering.setMag(this.maxSpeed);
            steering -= velocity;
            steering=Vector3.ClampMagnitude(steering, maxForce);
        }
        return steering;
    }

    public Vector3 separation(Boids[] boids)
    {

        //perceptionRadius = 50;
        Vector3 steering = new Vector3();
        int total = 0;
        foreach (Boids other in boids)
        {
            float d = Vector3.Distance(this.position, other.position);
            if (other != this && d < perceptionRadius)
            {
                Vector3 diff = this.position - other.position;
                //Debug.Log("diff: "+diff);
                
                diff /= (d * d);
                //Debug.Log("d sq: "+d*d);
                //Debug.Log("diff: "+diff);
                
                steering += diff;
                total++;
            }
        }
        if (total > 0)
        {
            steering /= (total);
            steering.Normalize();
            steering = steering * maxSpeed;
            //steering.setMag(this.maxSpeed);
            steering -= (velocity);
            steering=Vector3.ClampMagnitude(steering, maxForce);
        }
        return steering;
    }

    public Vector3 cohesion(Boids[] boids)
    {
        //perceptionRadius = 50;
        Vector3 steering = new Vector3();
        int total = 0;
        foreach (Boids other in boids)
        {
            float d = Vector3.Distance(position, other.position);
            if (other != this && d < perceptionRadius)
            {
                steering += (other.position);
                total++;
            }
        }
        if (total > 0)
        {
            steering /= (total);
            steering -= position;
            steering.Normalize();
            steering = steering * maxSpeed;
            //steering.setMag(this.maxSpeed);
            steering -= (velocity);
            Vector3.ClampMagnitude(steering, maxForce);
        }
        return steering;
    }

    public void flock(Boids[] boids,bool a,bool b,bool c,float x,float y,float z)
    {
        Debug.Log("flock function in boids.cs called");
        alignForce = x;
        cohesionForce = y;
        separationForce = z;

        if(a)
        {
            Debug.Log("alignment enabled");
            Vector3 alignment = this.align(boids);
            alignment *= alignForce;
            this.acceleration += (alignment);
        }
        if(b)
        {
            Debug.Log("Cohesion enabled");
            Vector3 cohesion = this.cohesion(boids);
            cohesion *= cohesionForce;
            this.acceleration += (cohesion);
        }
        if (c)
        {
            Debug.Log("separation enabled");
            Vector3 separation = this.separation(boids);
            separation *= separationForce;
            this.acceleration += (separation);
        }
    }

    public void update(float x)
    {
        perceptionRadius = x;
        this.position += (this.velocity);
        this.velocity += (this.acceleration);
        //this.velocity = Vector3.Normalize(this.velocity) * maxSpeed;
        Vector3.ClampMagnitude(velocity, this.maxSpeed);
        this.acceleration *= (0);
    }

    public void show()
    {

    }
}
