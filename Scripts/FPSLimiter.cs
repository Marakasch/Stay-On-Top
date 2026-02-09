using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public int targetFps;
    
    void Update()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;
    }
}
