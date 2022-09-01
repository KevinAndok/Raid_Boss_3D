using System;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    public static CustomInput Instance;

    #region Events
    public event Action OnLeftMouseDown;
    public event Action OnLeftMouseUp;

    public event Action OnRightMouseDown;
    public event Action OnRightMouseUp;

    public event Action OnShiftDown;
    public event Action OnShiftUp;
    #endregion

    #region Properties
    public bool leftMouseDown { get; private set; }
    public bool rightMouseDown { get; private set; }
    public bool shiftDown { get; private set; }
    #endregion

    private void Awake()
    {
        if (Instance) Destroy(Instance);
        Instance = this;
    }

    public void OnMouseLeft()
    {
        leftMouseDown = !leftMouseDown;

        switch (leftMouseDown)
        {
            case true:
                OnLeftMouseDown?.Invoke();
                break;
            case false:
                OnLeftMouseUp?.Invoke();
                break;
        }
    }
    public void OnMouseRight()
    {
        rightMouseDown = !rightMouseDown;

        switch (rightMouseDown)
        {
            case true:
                OnRightMouseDown?.Invoke();
                break;
            case false:
                OnRightMouseUp?.Invoke();
                break;
        }
    }
    public void OnShift()
    {
        shiftDown = !shiftDown;

        switch (shiftDown)
        {
            case true:
                OnShiftDown?.Invoke();
                break;
            case false:
                OnShiftUp?.Invoke();
                break;
        }
    }
}
