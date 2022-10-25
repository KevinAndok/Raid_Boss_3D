using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float[] cameraSizes;

    int currentCameraSizeIndex;
    Camera cam;

    private void Awake()
    {
        if (!TryGetComponent(out cam)) cam = Camera.main;
        CustomInput.OnMouseScroll += ResizeCameraSize;

        if (cameraSizes.Length == 0)
        {
            cameraSizes = new float[1];
            cameraSizes[0] = cam.orthographicSize;
        }
        currentCameraSizeIndex = Mathf.FloorToInt(cameraSizes.Length / 2);
    }

    public void ResizeCameraSize()
    {
        int val = (int)Mathf.Sign(CustomInput.ScrollValue);

        currentCameraSizeIndex = Mathf.Clamp(currentCameraSizeIndex + val, 0, cameraSizes.Length - 1);
        cam.orthographicSize = cameraSizes[currentCameraSizeIndex];
    }
}
