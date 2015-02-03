using System.Collections;

public class PerformanceCounterInstance : PerformanceCounter {
    private PerformanceCounterClass parent = null;
    // start with one item which will be enough in most cases
    private IList methods = new ArrayList(1);

    public PerformanceCounterInstance(string instanceName, PerformanceCounterClass parent)
        : base(instanceName) {
        this.parent = parent;
        this.parent.AddInstance(this);
    }

    public PerformanceCounterClass Parent { get { return parent; } }

    public void AddMethod(JCsProfilerMethod method) {
        methods.Add(method);
    }

    public IList Methods { get { return methods; } }
}
