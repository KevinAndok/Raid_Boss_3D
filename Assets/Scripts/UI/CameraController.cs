using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraScrollSpeed;

    [Header("Zoom Data")]
    public int countInEachDirection;
    public float scaleInterval;
    public float minRotation, maxRotation;

    [Space(10)]
    public CameraSizeRotationData[] cameraData;

    Camera cam;
    bool isDragging = false;
    int currentCameraSizeIndex;
    Vector3 mouseDeltaPosition;

    private void Awake()
    {
        if (!TryGetComponent(out cam)) cam = Camera.main;

        cameraData = new CameraSizeRotationData[countInEachDirection * 2 + 1];
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
        for (int i = 0; i < cameraData.Length; i++)
        {
            var diff = i - countInEachDirection;

            cameraData[i].Size = cam.orthographicSize + diff * scaleInterval;
            cameraData[i].Rotation = Mathf.Clamp(cam.transform.rotation.eulerAngles.x + diff * scaleInterval * 10, minRotation, maxRotation);
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

            cam.transform.position = Vector3.MoveTowards(camPos, camPos - new Vector3(moveVector.x, 0, moveVector.y), (cameraData[currentCameraSizeIndex].Size / cameraData[cameraData.Length - 1].Size) * cameraScrollSpeed);
        }

        mouseDeltaPosition = Input.mousePosition;
    }
    private void InitiateCameraResize()
    {
        CustomInput.OnMouseScroll += () => ResizeCameraSize();

        if (cameraData.Length == 0)
        {
            cameraData = new CameraSizeRotationData[1];
            cameraData[0] = new CameraSizeRotationData { Size = cam.orthographicSize, Rotation = cam.transform.rotation.x };
        }
        currentCameraSizeIndex = Mathf.Max((cameraData.Length - 1) / 2, 0);

        ResizeCameraSize(true);
    }
    public void ResizeCameraSize(bool skipInput = false)
    {
        int val = (int)Mathf.Sign(CustomInput.ScrollValue);

        currentCameraSizeIndex = Mathf.Clamp(currentCameraSizeIndex + (skipInput ? 0 : val), 0, cameraData.Length - 1);
        cam.orthographicSize = cameraData[currentCameraSizeIndex].Size;
        cam.transform.rotation = Quaternion.Euler(new Vector3(cameraData[currentCameraSizeIndex].Rotation, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z));
    }
}

[Serializable]
public class CameraSizeRotationData
{
    public float Size;
    public float Rotation;
}
