
using UnityEngine;

public class InputsController : MonoBehaviour
{
    private PlayerInputs playerInput;
    private Camera cam;
    [SerializeField] private ConnectionsController connectLineDraw;
    [SerializeField] private Transform dragObject;

    private void Start()
    {
      cam = Camera.main;
    }
    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInputs();
        }
        playerInput.Main.Enable();

        playerInput.Main.LeftClick.started += x => LeftClickStart(GetRaycastHit());
        playerInput.Main.LeftClick.canceled += x => LeftClickEnd(GetRaycastHit());
        playerInput.Main.RightClick.started += x => RightClickStart(GetRaycastHit());
        playerInput.Main.RightClick.canceled += x => RightClickEnd(GetRaycastHit());

    }

    private void OnDisable()
    {
        playerInput.Main.Disable();
    }

    private void LeftClickStart(RaycastHit2D hit)
    {
        if (connectLineDraw != null)
        {
            connectLineDraw.CancelDrawLine();
            connectLineDraw = null;
        }
        else if(hit && hit.transform.TryGetComponent(typeof(ConnectionsController), out Component connect))
        {
            dragObject = connect.transform;
            dragObject.GetComponent<ConnectionsController>().ActivateConnectdLines(true);
        }
    }
    private void LeftClickEnd(RaycastHit2D hit)
    {
        if(dragObject != null)
        {
            dragObject.GetComponent<ConnectionsController>().ActivateConnectdLines(false);
            dragObject = null;
        }
    }
    private void RightClickStart(RaycastHit2D hit)
    {
        if (connectLineDraw == null && hit && hit.transform.TryGetComponent(typeof(ConnectionsController), out Component connect))
        {
            connectLineDraw = connect.GetComponent<ConnectionsController>();
            connectLineDraw.StartDrawLine(this);
        }
    }
    private void RightClickEnd(RaycastHit2D hit)
    {
        if(hit && connectLineDraw != null && hit.transform.TryGetComponent(typeof(ConnectionsController), out Component connect))
        {
            if (connectLineDraw.AddOutputConnection(connect.GetComponent<ConnectionsController>()))
            {
                connectLineDraw = null;
            }
        }
    }
    private RaycastHit2D GetRaycastHit()
    {
        Ray ray = cam.ScreenPointToRay(playerInput.Main.MousePosition.ReadValue<Vector2>());
        return Physics2D.GetRayIntersection(ray);
    }
    public Vector2 MousePosition()
    {
        var mpp = playerInput.Main.MousePosition.ReadValue<Vector2>();
        var mousePoint = new Vector3(mpp.x, mpp.y, 10);
        return cam.ScreenToWorldPoint(mousePoint);
    }
    private void FixedUpdate()
    {
        if(dragObject != null)
        {
            dragObject.position = MousePosition();
        }
    }
}
