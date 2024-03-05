using UnityEngine;

public class Planet : MonoBehaviour
{
    public Color color;
    public float mass = 1.0f;
    public float size = 1.0f; // Radius of the planet

    private float initialSize;

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
        // Implement your physics calculations here
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
