using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public event Action OnCtrlDown;
    public event Action OnCtrlUp;

    public event Action OnAltDown;
    public event Action OnAltUp;

    public event Action OnOneDown;
    public event Action OnTwoDown;
    public event Action OnThreeDown;
    public event Action OnFourDown;
    public event Action OnFiveDown;
    public event Action OnSixDown;
    public event Action OnSevenDown;
    public event Action OnEightDown;
    public event Action OnNineDown;
    public event Action OnZeroDown;
    #endregion

    #region Properties
    public bool leftMouseDown { get; private set; }
    public bool rightMouseDown { get; private set; }
    public bool shiftDown { get; private set; }
    public bool ctrlDown { get; private set; }
    public bool altDown { get; private set; }
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
    public void OnCtrl()
    {
        ctrlDown = !ctrlDown;

        switch (ctrlDown)
        {
            case true:
                OnCtrlDown?.Invoke();
                break;
            case false:
                OnCtrlUp?.Invoke();
                break;
        }
    }
    public void OnAlt()
    {
        altDown = !altDown;

        switch (altDown)
        {
            case true:
                OnAltDown?.Invoke();
                break;
            case false:
                OnAltUp?.Invoke();
                break;
        }
    }
    public void OnOne() => OnOneDown?.Invoke();
    public void OnTwo() => OnTwoDown?.Invoke();
    public void OnThree() => OnThreeDown?.Invoke();
    public void OnFour() => OnFourDown?.Invoke();
    public void OnFive() => OnFiveDown?.Invoke();
    public void OnSix() => OnSixDown?.Invoke();
    public void OnSeven() => OnSevenDown?.Invoke();
    public void OnEight() => OnEightDown?.Invoke();
    public void OnNine() => OnNineDown?.Invoke();
    public void OnZero() => OnZeroDown?.Invoke();
}
