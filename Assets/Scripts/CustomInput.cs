using System;
using UnityEngine;

public sealed class CustomInput : MonoBehaviour
{
    #region Events
    public static event Action OnLeftMouseDown;
    public static event Action OnLeftMouseUp;

    public static event Action OnRightMouseDown;
    public static event Action OnRightMouseUp;

    public static event Action OnShiftDown;
    public static event Action OnShiftUp;

    public static event Action OnCtrlDown;
    public static event Action OnCtrlUp;

    public static event Action OnAltDown;
    public static event Action OnAltUp;

    public static event Action OnTabDown;
    public static event Action OnTabUp;

    public static event Action OnOneDown;
    public static event Action OnTwoDown;
    public static event Action OnThreeDown;
    public static event Action OnFourDown;
    public static event Action OnFiveDown;
    public static event Action OnSixDown;
    public static event Action OnSevenDown;
    public static event Action OnEightDown;
    public static event Action OnNineDown;
    public static event Action OnZeroDown;

    public static event Action OnQDown;
    public static event Action OnWDown;
    public static event Action OnEDown;
    public static event Action OnRDown;
    public static event Action OnADown;
    public static event Action OnSDown;
    public static event Action OnDDown;
    public static event Action OnFDown;
    public static event Action OnZDown;
    public static event Action OnXDown;
    public static event Action OnCDown;
    public static event Action OnVDown;
    #endregion

    #region Properties
    public static bool leftMouseDown { get; private set; }
    public static bool rightMouseDown { get; private set; }
    public static bool shiftDown { get; private set; }
    public static bool ctrlDown { get; private set; }
    public static bool altDown { get; private set; }
    public static bool tabDown { get; private set; }
    #endregion

    #region Functions
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
    public void OnTab()
    {
        tabDown = !tabDown;

        switch (tabDown)
        {
            case true:
                OnTabDown?.Invoke();
                break;
            case false:
                OnTabUp?.Invoke();
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

    public void OnQ() => OnQDown?.Invoke();
    public void OnW() => OnWDown?.Invoke();
    public void OnE() => OnEDown?.Invoke();
    public void OnR() => OnRDown?.Invoke();
    public void OnA() => OnADown?.Invoke();
    public void OnS() => OnSDown?.Invoke();
    public void OnD() => OnDDown?.Invoke();
    public void OnF() => OnFDown?.Invoke();
    public void OnZ() => OnZDown?.Invoke();
    public void OnX() => OnXDown?.Invoke();
    public void OnC() => OnCDown?.Invoke();
    public void OnV() => OnVDown?.Invoke();
    #endregion
}
