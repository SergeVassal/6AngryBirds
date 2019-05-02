using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public event Action AsteroidStopped;

    [SerializeField] private LineRenderer backLine;
    [SerializeField] private LineRenderer frontLine;
    [SerializeField] private Transform lowGroundBoundary;
    [SerializeField] private float maxStretch;
    [SerializeField] private float asteroidStopVelocity;

    private bool clickedOn;
    private Transform catapult;
    private SpringJoint2D springJoint;
    private float maxStretchSqr;    
    private Ray frontCatapultToProjRay;
    private Rigidbody2D rBody;
    private Vector2 prevVelocity;
    private float circleRadius;
    private bool isAsteroidConnected;
    private bool isAsteroidStopped;
    private const float ASTEROID_ANGULAR_DRAG=3f;



    private void Awake()
    {
        isAsteroidConnected = true;
        rBody = GetComponent <Rigidbody2D>();
        springJoint = GetComponent<SpringJoint2D>();
        catapult = springJoint.connectedBody.transform;
    }

    private void Start()
    {
        SetupLineRenderers();
        maxStretchSqr = maxStretch * maxStretch;
        CircleCollider2D circleCol = GetComponent<CircleCollider2D>();
        circleRadius = circleCol.radius;
    }

    private void SetupLineRenderers()
    {
        backLine.SetPosition(0, backLine.transform.position);
        frontLine.SetPosition(0, frontLine.transform.position);

        backLine.sortingLayerName = "FG";
        backLine.sortingOrder= 1;
        frontLine.sortingLayerName = "FG";
        frontLine.sortingOrder = 3;
    }
    
    private void Update()
    {
        if (clickedOn)
        {
            DragAsteroid();
        }

        if (isAsteroidConnected)
        {
            if (!rBody.isKinematic && prevVelocity.sqrMagnitude > rBody.velocity.sqrMagnitude)
            {
                springJoint.enabled = false;
                isAsteroidConnected = false;
                rBody.velocity = prevVelocity;
            }
            if (!clickedOn)
            {
                prevVelocity = rBody.velocity;
            }
            UpdateLineRenderers();
        }
        else
        {
            backLine.enabled = false;
            frontLine.enabled = false;
        }

        if (!isAsteroidStopped && !isAsteroidConnected && rBody.velocity.sqrMagnitude < asteroidStopVelocity)
        {            
            isAsteroidStopped = true;
            if (AsteroidStopped != null)
            {
                
                AsteroidStopped();
            }
        }
    }

    private void OnMouseDown()
    {
        springJoint.enabled = false;
        clickedOn = true;
    }

    private void OnMouseUp()
    {
        springJoint.enabled = true;
        rBody.isKinematic = false;
        rBody.angularDrag = ASTEROID_ANGULAR_DRAG;
        clickedOn = false;
    }

    private void DragAsteroid()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            Ray rayToMouse = new Ray(catapult.position,catapultToMouse);
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }

        mouseWorldPoint.y = Mathf.Clamp(mouseWorldPoint.y, lowGroundBoundary.position.y+circleRadius, Mathf.Infinity);

        mouseWorldPoint.z = 0f;
        rBody.position = mouseWorldPoint;

    }

    private void UpdateLineRenderers()
    {        
        Vector2 frontCatapultToAsteroid = transform.position - frontLine.transform.position;
        Ray frontCatapultToAsteroidRay = new Ray(frontLine.transform.position, frontCatapultToAsteroid);
        Vector3 lineToAsteroidHoldPoint = frontCatapultToAsteroidRay.GetPoint(frontCatapultToAsteroid.magnitude + circleRadius);
        frontLine.SetPosition(1, lineToAsteroidHoldPoint);
        backLine.SetPosition(1, lineToAsteroidHoldPoint);

    }


}
