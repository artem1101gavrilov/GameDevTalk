using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ReformRotateImpl), typeof(ReformMoveImpl), typeof(ReformZoomImpl)) ]
public class CameraController : MonoBehaviour
{

    [Range(0,1)]
    public float moveCameraPower = 0.5f;

    private AbstractReform hvRotateHandler;
    private AbstractReform xyzMoveHandler;
    private AbstractReform zoomHandler;

    // Property
    //--------------
    public Camera camSource;
    public Vector3 camTargetPos;

    [HideInInspector]
    public GameObject camTarget;

    public InputEvents myInputs;

    private void Awake()
    {
        hvRotateHandler = GetComponent<ReformRotateImpl>();
        xyzMoveHandler = GetComponent<ReformMoveImpl>();
        zoomHandler = GetComponent<ReformZoomImpl>();

        if (!camTarget)
        {
            camTarget = new GameObject("camTarget");
            camTarget.transform.parent = transform;
            camTarget.transform.position = camTargetPos;
        }

        if(!camSource)
        {
            camSource = Camera.main;
        }

        var source = camSource.transform;
        var target = camTarget.transform;

        hvRotateHandler.OnInit(source, target);
        xyzMoveHandler.OnInit(source, target);
        zoomHandler.OnInit(source, target);

        source.LookAt(target.position);

    }


    void OnEnable()
    {
        myInputs.eventMove += OnMoveCamera;
        myInputs.eventRotate += OnRotateCamera;
        myInputs.eventZoom += OnZoomCamera;
    }

    void OnDisable()
    {
        myInputs.eventMove -= OnMoveCamera;
        myInputs.eventRotate -= OnRotateCamera;
        myInputs.eventZoom -= OnZoomCamera;
    }

    void OnRotateCamera(Vector3 ed)
    {
        hvRotateHandler.OnReform(ed);
    }

    void OnMoveCamera(Vector3 ed)
    {
        xyzMoveHandler.OnReform(ed);
    }

    void OnZoomCamera(Vector3 ed)
    {
        zoomHandler.OnReform(ed);
    }

}
