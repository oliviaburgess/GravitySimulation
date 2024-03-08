using UnityEngine;

public class Planet : MonoBehaviour
{
    const float G = 66.7430f;
    public Color color;
    public float mass = 1.0f;
    public float size = 1.0f; // Radius of the planet
    public float axialTilt = 0.0f; // Angle of axial tilt in degrees

    private float initialSize;
    private GameObject rotationAxisObject;
    private LineRenderer lineRenderer;
    public bool showRotationAxis = true;
    private TrailRenderer orbitTrail;

    public bool showOrbitTrail = true;
    private Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 10.0f;

    public Vector3 initialVelocity; // Initial velocity of the planet

    public Rigidbody rigidBody;

    void FixedUpdate()
    {
        Planet[] planets = FindObjectsOfType<Planet>();
        foreach (Planet p in planets)
        {
            if (p != this) //Only apply force between another planet
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
        // Store the initial size for scaling purposes
        initialSize = transform.localScale.x;

        // Set the initial color of the sphere
        SetColor(color);

        // Create the rotation axis visualization
        CreateRotationAxis();

        // Create the orbit ellipse visualization
        CreateOrbitTrail();

        // Apply axial tilt
        ApplyAxialTilt();

        rigidBody.velocity = initialVelocity;

    }

    // Update is called once per frame
    void Update()
    {
        // Update the size of the planet based on the 'size' property
        transform.localScale = Vector3.one * (size * initialSize);

        //Show/hide axis line depending on settings 
        rotationAxisObject.SetActive(showRotationAxis); 

        //Show/hide trail depending on settings 
        orbitTrail.enabled = showOrbitTrail;

    }

    void GravitationalAttraction(Planet planetToAttract)
    {
        Rigidbody rbToAttract = planetToAttract.rigidBody;

        Vector3 direction = rigidBody.position - rbToAttract.position;

        //The length of the direction vector is the distance between the bodies centers
        float distance = direction.magnitude;

        float forceMagnitude = G * (this.mass * planetToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        //Debug.Log("Force Magnitude: " + forceMagnitude);


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

        //Get a random colour and assign it to the planet's trail
        Color randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        orbitTrail.material = new Material(Shader.Find("Sprites/Default"));
        orbitTrail.material.color = randomColor;


    }

    
}
