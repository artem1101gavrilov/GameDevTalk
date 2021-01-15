using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum InputsEventED
{
    Move,
    Rotate,
    Zoom
}

public delegate void InputEventCallback (EventData ed);

public struct CallBackData
{
    public InputsEventED id;
    public InputEventCallback callback;
}


public class InputEvents : MonoBehaviour
{
    List<CallBackData> callbacks = new List<CallBackData>();

    public UnityEvent<MoveEventData> eventMove;
    public UnityEvent<ZoomEventData> eventZoom;
    public UnityEvent<RotateEventData> eventRotate;

    public void SubscribeToEvent(InputsEventED id, InputEventCallback action)
    {
        callbacks.Add(new CallBackData()
        {
            id = id,
            callback = action
        });
    }
    public void UnsubscribeFromEvent(InputsEventED id, InputEventCallback action)
    {
        List<CallBackData> newCollbacks = new List<CallBackData>();
        foreach (var cd in callbacks)
        {
            if (cd.id != id || cd.callback != action)
            {
                newCollbacks.Add(cd);
            }
        }
        callbacks = newCollbacks;
    }

    void InvokeAll(InputsEventED id, EventData ed)
    {
        foreach(var cd in callbacks)
        {
            if(cd.id == id)
            {
                cd.callback(ed);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ed = new MoveEventData()
            {
                eventID = (int)InputsEventED.Move,
                axis = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"))
            };
            InvokeAll(InputsEventED.Move, ed);

            eventMove.Invoke(ed);
        }
        if (Input.GetMouseButton(1))
        {
            var ed = new RotateEventData()
            {
                eventID = (int)InputsEventED.Rotate,
                axis = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"))
            };
            InvokeAll(InputsEventED.Rotate, ed);

            eventRotate.Invoke(ed);
        }
        float x = Input.GetAxis("Mouse ScrollWheel");
        if(Mathf.Abs(x)>0)
        {
            var ed = new ZoomEventData()
            {
                eventID = (int)InputsEventED.Zoom,
                axis = new Vector3(Mathf.Sign(x), 0, 0)
            };
            InvokeAll(InputsEventED.Zoom    , ed);
            eventZoom.Invoke(ed);
        }
    }
}
