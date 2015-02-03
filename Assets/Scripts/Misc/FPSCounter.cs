// FPSCounter
// Date: ?
// Author: some dude in the unity forums
// Description: attach to a gui text to have a fps counter

using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float accum;
    private float fps = 15f;
    private int frames;
    private int gotIntervals;
    private float lastSample;
    private float timeleft;
    public float updateInterval = 1f;
	
    private float GetFPS()
    {
        return fps;
    }
	
    private bool HasFPS()
    {
        return (gotIntervals > 2);
    }
	
    private void Start()
    {
        timeleft = updateInterval;
        lastSample = Time.realtimeSinceStartup;
    }
	
    private void Update()
    {
        frames++;
        float num = Time.realtimeSinceStartup;
        float num2 = num - lastSample;
        lastSample = num;
        timeleft -= num2;
        accum += 1f / num2;
        if (timeleft <= 0f)
        {
            fps = accum / ((float) frames);
            guiText.text = "FPS: " + fps.ToString();
            timeleft = updateInterval;
            accum = 0f;
            frames = 0;
            gotIntervals++;
        }
    }
}
