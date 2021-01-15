using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReformMoveImpl : AbstractReform
{
    [Header("Horizont Moving")]

    [Range(1, 10)]
    public int flow = 2;

    [Range(1, 10)]
    public int friction = 2; 


    [Range(1, 10)]
    public int power = 4;
    private Vector3 moveForward;
    private Vector3 moveLaterally;

    [Header("Moving")]
    // after select some target user could move target of camera not longer then predefined distance value
    public Bounds bounds = new Bounds(Vector3.zero, new Vector3( 30,30,30 ));

    private Transform Source;
    private Transform Target;

    private Vector3 Axis;
    private Vector3 Offset { get; set; }

    private float lerpSpeed;
    private float minSpeed = 0.1f;
    public override void OnInit(Transform source, Transform target)
    {
        Source = source;
        Target = target;
    }

    public override void OnReform(Vector3 axis)
    {
        Axis += axis ;

        moveForward = Vector3.Cross(Source.right, Vector3.up);
        moveLaterally = Source.right;
        lerpSpeed = 0.01f * (float)friction;
    }

    private void Update()
    {
        if(IsMoving())
            DoMoving();
    }

    // ---------------------------------------
    //  Moving Implementation 
    // ---------------------------------------

    private void DoMoving()
    {
        Offset = Source.position - Target.position;

        Vector3 newPosFw = Vector3.zero;
        Vector3 newPosLt = Vector3.zero;
        Vector3 newPosVr = Vector3.zero;


        lerpSpeed = Mathf.Lerp(lerpSpeed, 0.01f * (float)friction / (float)flow, 0.03f * (float)friction);

        float dt = power * Time.deltaTime;

        if (IsMovingForward())
        {
            Axis.z = Mathf.Lerp(Axis.z, 0, lerpSpeed);
            newPosFw = -moveForward * Axis.z * dt;
            
        }

        if (IsMovingLaterally())
        {
            Axis.x = Mathf.Lerp(Axis.x, 0, lerpSpeed);
            newPosLt = -moveLaterally * Axis.x * dt;
        }

        if (IsMovingVertically())
        {
            Axis.y = Mathf.Lerp(Axis.y, 0, lerpSpeed);
            newPosVr = -Vector3.up * Axis.y * dt;
        }

        if (IsMoving())
        {
 
            if (GetNewTSPositions(out Vector3 newSpos, out Vector3 newTpos, newPosFw + newPosLt + newPosVr)
                || GetNewTSPositions(out newSpos, out newTpos, newPosFw + newPosLt)
                || GetNewTSPositions(out newSpos, out newTpos, newPosFw)
                || GetNewTSPositions(out newSpos, out newTpos, newPosLt + newPosVr)
                || GetNewTSPositions(out newSpos, out newTpos, newPosVr)
                || GetNewTSPositions(out newSpos, out newTpos, newPosLt)
                )
            {
                Source.position = newSpos;
                Target.position = newTpos;
            }
            else
            {
                Axis = Vector3.zero;
            }
        }
    }

    private bool GetNewTSPositions(out Vector3 newSpos, out Vector3 newTpos, Vector3 newPos)
    {
        newSpos = Source.position + newPos;
        newTpos = newSpos - Offset;
        return bounds.Contains(newTpos);
    }

    private bool IsMovingForward()
    {
        return Mathf.Abs(Axis.z) > minSpeed;
    }

    private bool IsMovingLaterally()
    {
        return Mathf.Abs(Axis.x) > minSpeed;
    }

    private bool IsMovingVertically()
    {
        return Mathf.Abs(Axis.y) > minSpeed;
    }

    public bool IsMoving()
    {
        return IsMovingForward() || IsMovingLaterally() || IsMovingVertically();
    }


}
