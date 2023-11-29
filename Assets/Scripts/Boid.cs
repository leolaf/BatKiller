using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Boid : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float behaviourCoefficient = 1;

    private Vector3 pos2D;

    private Vector3 velocity;

    private const float MAX_VELOCITY = 5f;

    private const float CLOSE_DISTANCE = 2f;

    private static List<Boid> allBoids = new List<Boid>();

    void Start()
    {
        transform.position = transform.parent.position + new Vector3((Random.value - 0.5f) * 2, (Random.value - 0.5f) * 2, (Random.value - 0.5f) * 2);
        pos2D = transform.position;
        velocity = new Vector3(Random.value-0.5f, Random.value - 0.5f, Random.value - 0.5f).normalized;
        allBoids.Add(this);
    }

    private void FixedUpdate()
    {

        List<Boid> closeBoids = new List<Boid>();
        foreach (Boid boid in allBoids)
        {
            if (boid == this) continue;
            if (Distance(boid) <= CLOSE_DISTANCE) closeBoids.Add(boid);
        }

        MoveTowards(GameObject.FindGameObjectWithTag("Player").transform.position);
        MoveCloser(closeBoids);
        MoveWith(closeBoids);
        MoveAway(closeBoids, CLOSE_DISTANCE / 10);

        Move();

        transform.position = new Vector3(pos2D.x, pos2D.y, pos2D.z); // transform.position.z);
    }

    float Distance(Boid other)
    {
        return (pos2D - other.pos2D).magnitude;
    }


    Vector3 GetAvgPos2D(List<Boid> boids)
    {
        Vector3 cumulgPos2D = Vector3.zero;
        foreach (Boid boid in boids)
        {
            if (boid.pos2D == pos2D) { continue; }
            cumulgPos2D += boid.pos2D * (boid.pos2D - pos2D).magnitude;
        }
        return cumulgPos2D / (float)boids.Count;
    }

    Vector3 GetAvgVelocity(List<Boid> boids)
    {
        Vector3 cumulVelocity = Vector3.zero;
        foreach (Boid boid in boids) { cumulVelocity += boid.velocity * (boid.pos2D - pos2D).magnitude; }
        return cumulVelocity / (float)boids.Count;
    }

    void MoveCloser(List<Boid> boids)
    {
        if (boids.Count < 1) { return; }
        // 100
        velocity += (GetAvgPos2D(boids) - pos2D) / 500;
    }

    void MoveWith(List<Boid> boids)
    {
        if (boids.Count < 1) { return; }
        // 40
        velocity += GetAvgVelocity(boids) / 500;
    }

    void MoveAway(List<Boid> boids, float minDist)
    {
        if (boids.Count < 1) { return; }

        Vector3 distanceDiff = Vector3.zero;

        int numClose = 0;

        foreach(Boid boid in boids)
        {
            float distance = Distance(boid);
            if(distance < minDist)
            {
                numClose++;
                Vector3 pos2DDiff = pos2D - boid.pos2D;

                if (pos2DDiff.x >= 0) { pos2DDiff.x = Mathf.Sqrt(minDist) - pos2DDiff.x; }
                else { pos2DDiff.x = -Mathf.Sqrt(minDist) - pos2DDiff.x; }

                if (pos2DDiff.y >= 0) { pos2DDiff.y = Mathf.Sqrt(minDist) - pos2DDiff.y; }
                else { pos2DDiff.y = -Mathf.Sqrt(minDist) - pos2DDiff.y; }

                if (pos2DDiff.z >= 0) { pos2DDiff.z = Mathf.Sqrt(minDist) - pos2DDiff.z; }
                else { pos2DDiff.z = -Mathf.Sqrt(minDist) - pos2DDiff.z; }

                distanceDiff += pos2DDiff * (2 - (boid.pos2D - pos2D).magnitude*2);
            }
        }

        if (numClose == 0) { return; }

        // 5
        velocity -= distanceDiff * 5;
    }

    void MoveTowards(Vector3 pos)
    {
        velocity += (pos - pos2D) / 500;
    }

    float RandomGaussianDistribution()
    {
        float u1 = 1.0f - Random.value; //uniform(0,1] random doubles
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        float randNormal = 0.5f + 0.25f * randStdNormal; //random normal(mean,stdDev^2
        return randNormal;
    }

    void Move()
    {
        if (velocity.magnitude > MAX_VELOCITY)
        {
            velocity = velocity.normalized * MAX_VELOCITY;
        }
        if (velocity.magnitude < 0.1f)
        {
            velocity = velocity.normalized * 0.1f;
        }

        spriteRenderer.flipX = velocity.x < 0;

        if (pos2D.z > 1 && velocity.z > 0 || pos2D.z < -1 && velocity.z < 0)
        {
            velocity.z = -velocity.z * Random.value;
        }

        float distToGround = Mathf.Infinity;
        foreach (RaycastHit hit in Physics.RaycastAll(pos2D + Vector3.up, Vector3.down, 1.5f))
        {
            if (hit.collider.tag == "Terrain")
            {
                distToGround = Mathf.Min(distToGround, hit.distance - 1);
            }
        }
        if (distToGround < 10f) { velocity.y = Mathf.Abs(velocity.y); }

        float distToCeiling = Mathf.Infinity;
        foreach (RaycastHit hit in Physics.RaycastAll(pos2D + Vector3.down, Vector3.up, 1.5f))
        {
            if (hit.collider.tag == "Terrain")
            {
                distToCeiling = Mathf.Min(distToCeiling, hit.distance - 1);
            }
        }
        if (distToCeiling < 10f) { velocity.y = -Mathf.Abs(velocity.y); }

        pos2D += velocity * Time.fixedDeltaTime;
    }
}
