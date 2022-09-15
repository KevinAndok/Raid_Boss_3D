using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    float fps = 0;
    int count = 0;

    private void Update()
    {
        if (count <= 0)
        {
            fps = Mathf.RoundToInt(1.0f / Time.deltaTime);
            count = 30;
        }
        count--;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), fps.ToString());
    }
}
