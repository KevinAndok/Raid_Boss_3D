using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraScrollSpeed;
    public float[] cameraSizes;

    Camera cam;
    bool isDragging = false;
    int currentCameraSizeIndex;
    Vector3 mouseDeltaPosition;

    private void Start()
    {
        InitiateCameraResize();
        InitializeCameraDrag();
    }
    private void Update()
    {
        CameraDragging();
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
        if (!TryGetComponent(out cam)) cam = Camera.main;
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
        int val = (int)Mathf.Sign(CustomInput.ScrollValue);

        currentCameraSizeIndex = Mathf.Clamp(currentCameraSizeIndex + (skipInput ? 0 : val), 0, cameraSizes.Length - 1);
        cam.orthographicSize = cameraSizes[currentCameraSizeIndex];
    }
}
