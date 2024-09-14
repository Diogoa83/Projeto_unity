using UnityEngine;

public class FrameRateLimit : MonoBehaviour
{
    public enum FrameRateOption
    {
        FPS_30 = 30,
        FPS_40 = 40,
        FPS_60 = 60,
        FPS_90 = 90
    }

    public FrameRateOption targetFrameRate = FrameRateOption.FPS_60;

    private void Awake()
    {
        Application.targetFrameRate = (int)targetFrameRate;
    }
}

