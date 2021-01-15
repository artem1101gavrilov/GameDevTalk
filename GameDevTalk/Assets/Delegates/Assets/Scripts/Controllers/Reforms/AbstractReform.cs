using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractReform : MonoBehaviour
{
    public abstract void OnInit(Transform source, Transform target);
    public abstract void OnReform(Vector3 axis);// x,y,z
}
