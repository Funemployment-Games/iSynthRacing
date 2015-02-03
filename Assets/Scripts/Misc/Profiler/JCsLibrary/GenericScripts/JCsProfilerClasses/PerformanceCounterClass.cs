using System.Collections;

public class PerformanceCounterClass : PerformanceCounter {
    // start with one item which will be enough in most cases
    private IList instances = new ArrayList(1);

    public PerformanceCounterClass(string className)
        : base(className) { /* done ;-) */ }

    public void AddInstance(PerformanceCounterInstance instance) {
        instances.Add(instance);
    }

    public IList Instances { get { return instances; } }

}
