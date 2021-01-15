using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReformZoomImpl : AbstractReform
{
    [Range(1, 15)]
    public int flow = 2;
    [Range(1, 15)]
    public int friction = 1;
    [Range(1, 15)]
    public int power = 1;

    public float minOffset = 2;
    public float maxOffset = 100;

    private float torqueZoom;

    private Transform Target;
    private Transform Source;

    private Vector3 Offset { get; set; }

    private float lerpSpeed;

    public override void OnInit(Transform source, Transform target)
    {
        Source = source;
        Target = target;
    }

    public override void OnReform(Vector3 axis)
    {
        torqueZoom += axis.x;

        lerpSpeed = 0.01f * (float)friction;
    }

    // Update is called once per frame
    void Update()
    {

        if (!IsZooming())
            return;

        lerpSpeed = Mathf.Lerp(lerpSpeed, 0.01f * (float)friction / (float)flow, 0.03f * (float)friction);

        float dt = power * Time.deltaTime;

        Offset = Source.position - Target.position;
        float curDistance = Offset.magnitude;

        if (//avoid changing distance near the target only during zooming in
            (curDistance > minOffset && torqueZoom > 0) ||
            //avoid changing distance far away from target only during zooming out
            (curDistance < maxOffset && torqueZoom < 0))
        {
            Source.Translate(Vector3.forward * torqueZoom * dt);
            torqueZoom = Mathf.Lerp(torqueZoom, 0, lerpSpeed);
        }
        else
        {
            torqueZoom = 0;
        }
    }

    public bool IsZooming()
    {
        return Mathf.Abs(torqueZoom) > 0.1f;
    }
}
