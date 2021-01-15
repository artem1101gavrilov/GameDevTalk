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
        myInputs.eventMove.AddListener(delegate (MoveEventData ed) 
        {
            xyzMoveHandler.OnReform(ed.axis);
        });
        
        myInputs.SubscribeToEvent(InputsEventED.Move, OnMoveCamera);
        myInputs.SubscribeToEvent(InputsEventED.Rotate, OnRotateCamera);
        myInputs.SubscribeToEvent(InputsEventED.Zoom, OnZoomCamera);
    }

    void OnDisable()
    {
        myInputs.UnsubscribeFromEvent(InputsEventED.Move, OnMoveCamera);
        myInputs.UnsubscribeFromEvent(InputsEventED.Rotate, OnRotateCamera);
        myInputs.UnsubscribeFromEvent(InputsEventED.Zoom, OnZoomCamera);
    }

    void OnRotateCamera(EventData ed)
    {
        Vector3 axis = ((RotateEventData)ed).axis;
        hvRotateHandler.OnReform(axis);
    }

    void OnMoveCamera(EventData ed)
    {
        Vector3 axis = ((MoveEventData)ed).axis;
        xyzMoveHandler.OnReform(axis);
    }

    void OnZoomCamera(EventData ed)
    {
        Vector3 axis = ((ZoomEventData)ed).axis;
        zoomHandler.OnReform(axis);
    }

}
