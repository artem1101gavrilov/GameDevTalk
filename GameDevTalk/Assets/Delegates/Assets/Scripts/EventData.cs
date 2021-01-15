using System.Collections;
using UnityEngine;

public class EventData
{
    public int eventID;
}

public class MoveEventData : EventData
{
    public Vector3 axis;
}

public class RotateEventData : EventData
{
    public Vector3 axis;
}
public class ZoomEventData : EventData
{
    public Vector3 axis;
}


