using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ReformRotateImpl : AbstractReform
{
    // ---------------------------------------
    // properties for vertical rotate processing 
    // ---------------------------------------

    const float RADIAN = Mathf.Deg2Rad;

    [Header("Vertical rotate ")]

    [Range(1, 15)]
    public float minVRateRotation = 3;
    public float rotateVFriction = 0.05f;
    public float rotateVSpeed = 5;

    [HideInInspector]
    public float torqueVertical; //vertical power torque

    [Range(0, 90)]
    public int meredianLimit = 0; //vertical rotation limit 

    // ---------------------------------------
    // properties for horizontal rotate processing
    // ---------------------------------------

    [Header("Horizontal rotate")]
    [Range(1, 15)]
    public float minHRateRotation = 3;
    public float rotateHFriction = 0.05f;
    public float rotateHSpeed = 5;

    [HideInInspector]
    public float torqueHorizontal;

    private Transform Target;
    private Transform Source;

    private Vector3 Offset { get; set; }
    private Vector3 Axis;

    public override void OnInit(Transform source, Transform target)
    {
        Source = source;
        Target = target;
    }

    // if v in range (min,max), then return min if v < mid, and return max if v > mid 
    float ReversalClamp(float v, float min, float mid, float max)
    {
        return v < mid ?
            Mathf.Clamp(v, float.MinValue, min):
            Mathf.Clamp(v, max, float.MaxValue);
    }

    public override void OnReform(Vector3 axis)
    {
        Assert.IsNotNull(Source);
        Assert.IsNotNull(Target);
        Axis = axis;

        if (Mathf.Abs(Axis.x) < minHRateRotation * RADIAN && Mathf.Abs(Axis.x) > RADIAN)
            Axis.x = ReversalClamp(Axis.x, -minHRateRotation * RADIAN, 0, minHRateRotation * RADIAN);

        if (Mathf.Abs(Axis.z) < minVRateRotation * RADIAN && Mathf.Abs(Axis.z) > RADIAN)
            Axis.z = ReversalClamp(Axis.z, -minVRateRotation * RADIAN, 0, minVRateRotation * RADIAN);

        torqueHorizontal += Axis.x;
        torqueVertical += Axis.z;
    }

    private void Update()
    {
        if (IsRotatingHorizontal())
            DoRotateHorizontal();
        if (IsRotatingVertical())
            DoRotateVertical();
    }

    // ---------------------------------------
    //  Horizontal Rotate Implementation 
    // ---------------------------------------

    private void DoRotateHorizontal()
    {
        Offset = Source.position - Target.position;
        torqueHorizontal = Mathf.Lerp(torqueHorizontal, 0, rotateHFriction);

        if (Mathf.Abs(torqueHorizontal) < RADIAN)
            torqueHorizontal = 0;

        Quaternion rotation = Quaternion.Euler(0, torqueHorizontal * rotateHSpeed * rotateHFriction, 0);
        Offset = rotation * Offset;
        Vector3 desiredPosition = Target.position + Offset;

        Source.position = Vector3.RotateTowards(Source.position, desiredPosition, Mathf.Abs(torqueHorizontal), Offset.magnitude);
        Source.LookAt(Target.position);

        //Debug.DrawRay (source.position, desiredPosition - source.position, Color.red);
        //Debug.DrawRay (target.position, desiredPosition - target.position, Color.green);
    }

    private bool IsRotatingHorizontal()
    {
        return Mathf.Abs(torqueHorizontal) > RADIAN;
    }

    // ---------------------------------------
    //  Vertical Rotate Implementation 
    // ---------------------------------------

    private void DoRotateVertical()
    {
        Offset = Source.position - Target.position;
        torqueVertical = Mathf.Lerp(torqueVertical, 0, rotateVFriction);

        if (Mathf.Abs(torqueVertical) < RADIAN)
            torqueVertical = 0;


        int sign = torqueVertical > 0 ? -1 : 1;
        Vector3 offsetV = new Vector3(0, Offset.magnitude * sign, 0);

        float curAngle = Vector3.Angle(Offset, offsetV);
        float resAngle = curAngle - Mathf.Abs(torqueVertical);

        if (resAngle < meredianLimit)
        {
            torqueVertical = 0;
            return;
        }
        
        Vector3 desiredPosition = Target.position + offsetV;

        //Debug.DrawLine (target.position, desiredPosition , Color.green);
        //Debug.DrawLine(source.position, desiredPosition , Color.blue);

        Vector3 side1 = Source.position - Target.position;
        Vector3 side2 = desiredPosition - Target.position;
        Vector3 normal = Vector3.Cross(side1, side2);

        //Debug.DrawRay(Source.position, normal, Color.red);

        Quaternion rotation = Quaternion.AngleAxis(Mathf.Abs(torqueVertical) * rotateVSpeed * rotateVFriction, normal);
        Offset = rotation * Offset;

        desiredPosition = Target.position + Offset;

        Source.position = Vector3.RotateTowards(Source.position, desiredPosition, Mathf.Abs(torqueVertical), Offset.magnitude);
        Source.LookAt(Target.position);

    }

    private bool IsRotatingVertical()
    {
        return Mathf.Abs(torqueVertical) > RADIAN;
    }


}
