using UnityEngine;

public class InputEvents : MonoBehaviour
{
    public event System.Action<Vector3> eventMove;
    public event System.Action<Vector3> eventZoom;
    public event System.Action<Vector3> eventRotate;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            eventMove?.Invoke(new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")));
        }
        if (Input.GetMouseButton(1))
        {
            eventRotate?.Invoke(new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")));
        }
        float x = Input.GetAxis("Mouse ScrollWheel");
        if(Mathf.Abs(x)>0)
        {
            eventRotate?.Invoke(new Vector3(Mathf.Sign(x), 0, 0));
        }
    }
}
