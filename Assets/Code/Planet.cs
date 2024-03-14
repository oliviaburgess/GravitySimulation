using UnityEngine;

public class Planet : MonoBehaviour
{
    public Color color;
    public float mass = 1.0f;
    public float size = 1.0f; 
    private float initialSize;
    const float G = 66.7430f;

    public float axialTilt = 0.0f; 
    private GameObject rotationAxisObject;
    private LineRenderer lineRenderer;
    public bool showRotationAxis = true;
    private Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 10.0f;

    private TrailRenderer orbitTrail;

    public bool showOrbitTrail = true;

    public Vector3 initialVelocity;

    private Rigidbody rigidBody;

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

        // Rotate the planet around its axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.fixedDeltaTime);

        // Update the rotation axis visualization depending on settings
        if (rotationAxisObject != null && showRotationAxis)
        {
            UpdateRotationAxis();
        }
    }

    private void Start()
    {
        
        // Create and attach the Rigidbody component
        CreateRigidbody();

        // Set the initial velocity
        rigidBody.velocity = initialVelocity;

        // Store the initial size for scaling purposes
        initialSize = transform.localScale.x;

        // Set the initial color of the sphere
        SetColor(color);

        // Create the rotation axis visualization
        CreateRotationAxis();

        // Apply axial tilt
        ApplyAxialTilt();

        // Create the orbit ellipse visualization
        CreateOrbitTrail();
    }

    void Update()
    {
        // Show/hide axis line depending on settings 
        rotationAxisObject.SetActive(showRotationAxis);

        // Update the size of the planet based on the 'size' property
        transform.localScale = Vector3.one * (size * initialSize);

        // Show/hide trail depending on settings 
        orbitTrail.enabled = showOrbitTrail;
    }

    void GravitationalAttraction(Planet planetToAttract)
    {
        Rigidbody rbToAttract = planetToAttract.rigidBody;

        Vector3 direction = rigidBody.position - rbToAttract.position;

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

    // Function to create the rotation axis visualization
    private void CreateRotationAxis()
    {
        rotationAxisObject = new GameObject("RotationAxis");
        rotationAxisObject.transform.parent = transform;
        rotationAxisObject.transform.localPosition = Vector3.zero;

        lineRenderer = rotationAxisObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        UpdateRotationAxis();
    }

    // Update axis visualization based on the rotation
    private void UpdateRotationAxis()
    {
        lineRenderer.SetPosition(0, transform.TransformPoint(-rotationAxis * size * initialSize));
        lineRenderer.SetPosition(1, transform.TransformPoint(rotationAxis * size * initialSize));
    }

    // Apply axial tilt to the planet
    private void ApplyAxialTilt()
    {
        rotationAxis = Quaternion.Euler(axialTilt, 0, 0) * rotationAxis;
    }

    private void CreateOrbitTrail()
    {
        orbitTrail = gameObject.AddComponent<TrailRenderer>();
        orbitTrail.time = 3.0f;
        orbitTrail.startWidth = 0.25f;
        orbitTrail.endWidth = 0.0f;

        Color randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        orbitTrail.material = new Material(Shader.Find("Sprites/Default"));
        orbitTrail.material.color = randomColor;
    }

    private void CreateRigidbody()
    {
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.mass = mass;
        rigidBody.useGravity = false;
    }
}
