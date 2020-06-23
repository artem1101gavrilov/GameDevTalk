using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtan : MonoBehaviour
{
    public bool RotateY;

    void Update()
    {
        var positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var scale = transform.localScale;
        if (positionMouse.x < transform.position.x)
        {
            if (scale.x > 0)
            {
                scale.x *= -1;
                if(RotateY) scale.y *= -1;
                transform.localScale = scale;
            }
        }
        else
        {
            if (scale.x < 0)
            {
                scale.x *= -1;
                if(RotateY) scale.y *= -1;
                transform.localScale = scale;
            }
        }
    }
}
