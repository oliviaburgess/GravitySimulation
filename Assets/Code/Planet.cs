using UnityEngine;

public class Planet : MonoBehaviour
{
    const float G = 667.4f;
    public Color color;
    public float mass = 1000f;
    public float size = 1.0f; // Radius of the planet

    private float initialSize;

    public Rigidbody rigidBody;

    void FixedUpdate()
    {
        Planet[] planets = FindObjectsOfType<Planet>();
        foreach (Planet p in planets)
        {
            if (p != this)
            {
                GravitationalAttraction(p);
            }
        }
    }

    private void Start()
    {
        // Store the initial size for scaling purposes
        initialSize = transform.localScale.x;

        // Set the initial color of the sphere
        SetColor(color);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the size of the planet based on the 'size' property
        transform.localScale = Vector3.one * (size * initialSize);
    }

    // Function to apply force to the planet
    public void ApplyForce(Vector3 force, float dt)
    {

    }

    void GravitationalAttraction(Planet planetToAttract)
    {
        Rigidbody rbToAttract = planetToAttract.rigidBody;

        Vector3 direction = rigidBody.position - rbToAttract.position;

        //The lenght of the direction vector is the distance between the bodies centres
        float distance = direction.magnitude;

        float forceMagnitude = G * (this.mass * planetToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }



    // Function to set the color of the planet
    public void SetColor(Color newColor)
    {
        color = newColor;

        // Change the color of the sphere's material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}
