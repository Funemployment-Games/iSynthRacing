using UnityEngine;

/// <summary>
///     Instances of this class are returned to the method being profiled
///     and must correctly be "returned" by calling CallIsFinished() at the
///     end of the code-section to be profiled.
/// </summary>
public class JCsProfilerMethod : PerformanceCounter {
    private PerformanceCounterInstance parent = null;
    private float currentCallTime = 0;

    public JCsProfilerMethod(string methodName, PerformanceCounterInstance parent)
        : base(methodName) {
        if (parent == null) {
            return;
        }
        this.parent = parent;
        this.parent.AddMethod(this);
    }

    public void StartCallStopWatch() {
        if (currentCallTime != 0.0F) {
            string msg = string.Format(
                "WARN: You call {0} multiple times without calling CallIsFinished()!",
                ToString());
            MonoBehaviour.print(msg);
        }
        currentCallTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    ///     This method MUST be called whenever you've started measuring 
    ///     time with JCsProfiler.Instance.StartCallStopWatch(...).
    /// </summary>
    public void CallIsFinished() {
        if (parent == null) { // => we're just a dummy, don't waste time...
            return;
        }
        // do this first to avoid adding management code to profiling stuff...
        float diff = Time.realtimeSinceStartup - currentCallTime;
        IncrementCallCount();
        IncrementMsSpent(diff);
        // doing this for the parents here avoids having to iteratively / recursively sum this up...
        parent.IncrementCallCount();
        parent.IncrementMsSpent(diff);

        parent.Parent.IncrementCallCount();
        parent.Parent.IncrementMsSpent(diff);

        // reset current call time for sanity checks...
        currentCallTime = 0.0F;
    }

    public override string ToString() {
        if (parent == null) {
            return "dummy - profiling not active! Set JCsProfiler.trackProfiling = true to start profiling!";
        }
        return string.Format("{0}[{1}].{2}(...)", parent.Parent.Name, parent.Name, Name);
    }
}

