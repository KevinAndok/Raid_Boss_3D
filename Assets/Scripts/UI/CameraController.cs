using System;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public float cameraScrollSpeed;
    public float cameraZoomSpeed;

    [Header("Zoom Data")]
    public int countInEachDirection;
    public float scaleInterval;
    public float minRotation, maxRotation;

    [Space(10)]
    public float[] cameraSizes;

    Camera cam;
    bool isDragging = false;
    int currentCameraSizeIndex;
    Vector3 mouseDeltaPosition;
    Tween zoomTween;

    private void Awake()
    {
        if (!TryGetComponent(out cam)) cam = Camera.main;

        cameraSizes = new float[countInEachDirection * 2 + 1];
    }
    private void Start()
    {
        InitializeCameraSizes();
        InitiateCameraResize();
        InitializeCameraDrag();
    }
    private void Update()
    {
        CameraDragging();
    }

    private void InitializeCameraSizes()
    {
        for (int i = 0; i < cameraSizes.Length; i++)
        {
            var diff = i - countInEachDirection;

            cameraSizes[i] = cam.orthographicSize + diff * scaleInterval;
        }
    }
    private void InitializeCameraDrag()
    {
        CustomInput.OnMiddleMouseDown += () => { isDragging = true; };
        CustomInput.OnMiddleMouseUp += () => { isDragging = false; };
    }
    private void CameraDragging()
    {
        if (isDragging)
        {
            var moveVector = Input.mousePosition - mouseDeltaPosition;
            var camPos = cam.transform.position;

            cam.transform.position = Vector3.MoveTowards(camPos, camPos - new Vector3(moveVector.x, 0, moveVector.y), (cameraSizes[currentCameraSizeIndex] / cameraSizes[cameraSizes.Length - 1]) * cameraScrollSpeed);
        }

        mouseDeltaPosition = Input.mousePosition;
    }
    private void InitiateCameraResize()
    {
        CustomInput.OnMouseScroll += () => ResizeCameraSize();

        if (cameraSizes.Length == 0)
        {
            cameraSizes = new float[1];
            cameraSizes[0] = cam.orthographicSize;
        }
        currentCameraSizeIndex = Mathf.Max((cameraSizes.Length - 1) / 2, 0);

        ResizeCameraSize(true);
    }
    public void ResizeCameraSize(bool skipInput = false)
    {
        if (zoomTween != null) zoomTween.Kill();

        int val = (int)Mathf.Sign(CustomInput.ScrollValue);

        currentCameraSizeIndex = Mathf.Clamp(currentCameraSizeIndex + (skipInput ? 0 : val), 0, cameraSizes.Length - 1);
        zoomTween = cam.DOOrthoSize(cameraSizes[currentCameraSizeIndex], cameraZoomSpeed);
        //cam.orthographicSize = cameraSizes[currentCameraSizeIndex];
    }
}

[Serializable]
public class CameraSizeRotationData
{
    public float Size;
    public float Rotation;
}
