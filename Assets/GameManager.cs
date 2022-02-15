using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager:MonoBehaviour
{

    //public static GameManager instance;
    public GameObject prefab;
    Boids[] flock=new Boids[100];
    public float alignForce, cohesionForce, separationForce, perceptionRadius;
    float maxSpeed;
    public bool Cohesion, Alignment, Separation, WrapEdges=true;
    int i = 1;
    // Start is called before the first frame update
    void Start()
    {
        perceptionRadius = 10;
        alignForce= cohesionForce= separationForce=0.02f;//align 0.01
        
        WrapEdges = true;
        maxSpeed = 1f;
        for (int i = 0; i < 100; i++)
        {
            //Debug.Log("Created boid no: " + i + 1);
            flock[i]=new Boids();
            flock[i].body = Instantiate(prefab, flock[i].position, Quaternion.identity);
            flock[i].rb = flock[i].body.GetComponent<Rigidbody>();
            //flock[i].rb.AddForce(flock[i].velocity, ForceMode.VelocityChange);
            
        }
    }
    private void Update()
    {
        {//for random forces every 5 seconds
            i++;
            if(i%300==0)
            {
                alignForce = Random.Range(0.0f, 0.5f);
                separationForce = Random.Range(0.0f, 0.5f);
                cohesionForce = Random.Range(0.01f, 0.1f);
            }

        }
        

        foreach (Boids boid in flock)
        {
            
            if(WrapEdges)
                boid.edges();
            //boid.flock(flock,Alignment,Cohesion,Separation, alignForce, cohesionForce, separationForce);   for current bak
            boid.flock(flock,Alignment,Cohesion,Separation,alignForce,cohesionForce,separationForce); // for ported bak
            boid.update(perceptionRadius);
            boid.velocity = Vector3.Normalize(boid.velocity) * maxSpeed;
            boid.body.transform.position=(boid.position);
            
        }
    }


}
