using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static PlayerInputs;
using static UnityEngine.InputSystem.InputAction;

public class CameraMoveController : Singleton<CameraMoveController>, IMainActions
{
    [SerializeField] private float _camMoveSpeed;
    [SerializeField] private float _camZoomSpeed;
    private Transform _camaraObj;
    private Vector2Control _delta;
    private BackgroundController _background;
    

    private void Start()
    {
        _delta = InputSystem.GetDevice<Mouse>().delta;
        _camaraObj = Camera.main.gameObject.transform;
        _background = BackgroundController.Instance;
    }

    public void OnLeftClick(CallbackContext context)
    {
    }

    public void OnMousePosition(CallbackContext context)
    {
        var d = _delta.ReadValue().normalized * -1 * Time.deltaTime;
        _camaraObj.position += new Vector3(d.x, d.y, 0) * _camMoveSpeed * Math.Abs(_camaraObj.position.z);
        _background.Move();
    }

    public void OnScroll(CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y > 0 && _camaraObj.position.z < - 1)
        {
            _camaraObj.position += new Vector3(0, 0, _camZoomSpeed);
        }
        else if (context.ReadValue<Vector2>().y < 0 && _camaraObj.position.z > -30)
            _camaraObj.position -= new Vector3(0, 0, _camZoomSpeed);
    }
}
