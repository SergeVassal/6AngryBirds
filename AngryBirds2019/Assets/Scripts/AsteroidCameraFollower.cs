using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCameraFollower : MonoBehaviour
{
    [SerializeField] private Transform asteroid;
    [SerializeField] private Transform leftBounary;
    [SerializeField] private Transform rightBounary;
    private const float CAMERA_Z_POSITION = -10f;



    private void Update()
    {
        var newPosition = new Vector3(Mathf.Clamp(asteroid.position.x, leftBounary.position.x, rightBounary.position.x),
            transform.position.y, CAMERA_Z_POSITION);
        transform.position = newPosition;
    }
}
