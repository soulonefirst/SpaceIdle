using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static PlayerInputs;
using static UnityEngine.InputSystem.InputAction;

public class CameraMoveController : Singleton<CameraMoveController>, IMainActions
{
    [SerializeField] private float camMoveSpeed;
    [SerializeField] private float camZoomSpeed;
    private Transform camareObj;
    private Vector2Control delta;

    private void Start()
    {
        delta = InputSystem.GetDevice<Mouse>().delta;
        camareObj = Camera.main.gameObject.transform;
    }

    public void OnLeftClick(CallbackContext context)
    {
    }

    public void OnMousePosition(CallbackContext context)
    {
        var d = delta.ReadValue().normalized * -1 * Time.deltaTime;
        camareObj.position += new Vector3(d.x, d.y, 0) * camMoveSpeed * Math.Abs(camareObj.position.z);
    }

    public void OnScroll(CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y > 0 && camareObj.position.z < - 1)
        {
            camareObj.position += new Vector3(0, 0, camZoomSpeed);
        }
        else if (context.ReadValue<Vector2>().y < 0 && camareObj.position.z > -30)
            camareObj.position -= new Vector3(0, 0, camZoomSpeed);
    }
}
