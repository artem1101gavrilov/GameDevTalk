using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAtan : MonoBehaviour
{
    void Update()
    {
        var positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var rotationVector = transform.rotation.eulerAngles;
        var direction = positionMouse - transform.position;
        rotationVector.z = Mathf.Atan2(direction.y, direction.x) * 180.0f / 3.14f;
        transform.rotation = Quaternion.Euler(rotationVector);
    }
}
