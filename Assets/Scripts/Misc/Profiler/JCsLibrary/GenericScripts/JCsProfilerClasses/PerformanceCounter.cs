using UnityEngine;

public class PerformanceCounter {
    public bool showChildren = false; // set/get, so I can also just make it public

    private string name = null;
    protected float firstCallTime = 0;
    protected int callCount = 0;
    protected float timeSpent = 0;
    protected float timeSpentMin = float.MaxValue;
    protected float timeSpentMax = 0;


    public PerformanceCounter(string name) {
        this.name = name;
        // this time does not need to be perfectly exact as a lot of time will pass
        // while this is being used...
        firstCallTime = Time.realtimeSinceStartup;
    }

    public string Name {
        get { return name; }
    }

    public float FirstCallTime {
        get { return firstCallTime; }
    }

    public float SecondsSinceFirstCall {
        get { return Time.realtimeSinceStartup - firstCallTime; }
    }

    public int CallCount {
        get { return callCount; }
    }

    public void IncrementCallCount() {
        callCount++;
    }

    public float TimeSpent {
        get { return timeSpent * 1000.0F; }
    }

    public float TimeSpentMin {
        get { return timeSpentMin * 1000.0F; }
    }

    public float TimeSpentMax {
        get { return timeSpentMax * 1000.0F; }
    }

    public void IncrementMsSpent(float timeSpent) {
        this.timeSpent += timeSpent;
        timeSpentMin = Mathf.Min(timeSpentMin, timeSpent);
        timeSpentMax = Mathf.Max(timeSpentMax, timeSpent);
    }
}

