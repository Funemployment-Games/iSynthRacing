using UnityEngine;
using System.Collections;

/// <summary>
///     Provides a simple mechanism for profiling method calls in classes and
///     instances. Attach this class as a component to any game object you
///     feel suits the purpose, a level game object that you can easily deactivate.
/// </summary>
/// <author>Jashan Chittesh - info@jashan-chittesh.de</author>
public class JCsProfiler : MonoBehaviour {

    /*
     * to use this with log4net, using JCsLogger, comment in
     * the following line, and then replace //log. with log.
     * (i.e. comment in the 2 or 3 instances where log is
     * actually used)
     */

    #region log4net declaration
    //private static readonly JCsLogger log = new JCsLogger(typeof(JCsProfiler));
    #endregion

    #region Public inspector "properties" ;-)
    /// <summary>
    ///     Use this public variable to set your preferred GUISkin. You may
    ///     consider using a specific GUISkin for the stats (with small fonts
    ///     etc.)
    /// </summary>
    public GUISkin mySkin;

    /// <summary>
    ///     Indicates whether or not profiling should be active. Use this
    ///     to switch off profiling to avoid a performance hit (if this
    ///     is false, there should only be a very little performance hit
    ///     through the code checks involved).
    /// </summary>
    public bool trackProfiling = true;

    /// <summary>
    ///     Indicates whether or not to render the stats window at all.
    ///     If trackProfiling is false, the stats window is never rendered!
    /// </summary>
    public bool renderStatsWindow = true;

    /// <summary>
    ///     Indicates whether or not to render the stats details.
    /// </summary>
    public bool renderStatsDetails = true;

    /// <summary>
    ///     If true, this makes the profiler render the results to the
    ///     console every renderOutputIntervalSeconds seconds.
    /// </summary>
    public bool renderToConsole = false;

    /// <summary>
    ///     If true, this makes the profiler render the results to the
    ///     logging devices configured with log4net (if available) every
    ///     renderOutputIntervalSeconds seconds.
    /// </summary>
    public bool renderToLogging = true;

    /// <summary>
    ///     Number of seconds between rendering the profiler output to
    ///     the console or logging.
    /// </summary>
    public int renderOutputIntervalSeconds = 30;

    /// <summary>
    ///     Number of characters reserved for the class / instance / method
    ///     column when outputting text.
    /// </summary>
    public int renderTextClassWidth = 15;

    /// <summary>
    ///     Number of characters used for indenting between classes,
    ///     instances and methods.
    /// </summary>
    public int renderTextIndentWidth = 3;

    /// <summary>
    ///     Number of characters reserved for the details columns when
    ///     outputting text.
    /// </summary>
    public int renderTextColumnWidth = 8;

    /// <summary>
    ///     Padding before stats (spacer between main control buttons and actual stats).
    /// </summary>
    public float paddingTop = 5.0F;

    /// <summary>
    ///     Padding after list of methods.
    /// </summary>
    public float paddingBottomMethods = 9.0F;


    /// <summary>
    ///     The width of the classes / instances / method - names.
    /// </summary>
    public float mainLabelWidth = 180.0F;

    /// <summary>
    ///     The width of the columns
    /// </summary>
    public float detailsWidth = 45.0F;

    /// <summary>
    ///     The width used for indenting instances / methods.
    /// </summary>
    public int indentWidth = 23;
    #endregion

    #region Internal member variables
    /// <summary>
    ///     Used to store the window size and location.
    /// </summary>
    private Rect windowRect = new Rect(200, 20, 150, 50);

    /// <summary>
    ///     This is here because creating a rect object for each OnGUI call
    ///     seems to create a lot of unneeded objects.
    /// </summary>
    private Rect dragRect = new Rect(0, 0, 10000, 10000);

    /// <summary>
    ///     Used for quick access to all "performance counters".
    ///     Maps PerformanceCounterKeys to JCsProfilerMethods
    ///     for quick access.
    /// </summary>
    private Hashtable allPerformanceCounters = new Hashtable();

    /// <summary>
    ///     Used for a list of all classes which can be used to
    ///     generate a complete tree of classes, instances and
    ///     methods.
    /// </summary>
    private IList classes = new ArrayList();

    // used to store the singleton instance of JCsProfiler.
    private static JCsProfiler instance = null;
    #endregion

    /// <summary>
    ///     Returns the single instance of JCsProfiler (singleton-pattern).
    ///     Use this to access the profiler from anywhere in your code.
    /// </summary>
    public static JCsProfiler Instance {
        get {
            if (instance == null) {
                instance = new JCsProfiler();
            }
            return instance;
        }
    }

    #region Initialization methods (Awake / Start)
    /// <summary>
    ///     Checks if this is a redundant instance of JCsProfiler and removes
    ///     itself, if this is the case. If not, sets the singleton instance
    ///     to this instance so other objects can access it.
    /// </summary>
    void Awake() {
        if (instance != null) {
            string msg = string.Format(
                "Destroying redundant instance of JCsProfiler attached to {0}",
                gameObject.name);
            print(msg);
            DestroyImmediate(this);
        } else {
            instance = this;
            if (renderToLogging || renderToConsole) {
                StartCoroutine(AutomaticTextRendering());
            }
        }
    }

	/// <summary>Not used.</summary>
	void Start() { }
    #endregion

    #region GUI rendering code
    /// <summary>
	///     Paints the currents stats.
	/// </summary>
	void OnGUI() {
        if (renderStatsWindow && trackProfiling) {
            if (mySkin != null) {
                GUI.skin = mySkin;
            }
            windowRect = GUI.Window(0, windowRect, this.DrawStatsWindow, "JCsProfiler Stats");

            JCsProfilerMethod pcm = JCsProfiler.Instance.StartCallStopWatch("JCsProfiler", gameObject.name, "OnGUI");

            // make sure user does not drag window out of screen (might be bad for performance, though...)
            windowRect = new Rect(
                Mathf.Clamp(windowRect.x, 0, Screen.width - windowRect.width),
                Mathf.Clamp(windowRect.y, 0, Screen.height - windowRect.height),
                windowRect.width, windowRect.height);

            pcm.CallIsFinished();
        }
	}

    /// <summary>
    ///     Does the actual window drawing.
    /// </summary>
    /// <param name="windowID">Ignored</param>
    void DrawStatsWindow(int windowID) {
        JCsProfilerMethod pcm = JCsProfiler.Instance.StartCallStopWatch("JCsProfiler", gameObject.name, "DrawStatsWindow");

        float windowTitleHeight = GUI.skin.window.padding.top;
        float windowPaddingBottom = GUI.skin.window.padding.bottom;
        const float windowPadding = 5.0F; // The "natural" padding of the window...

        GUILayout.BeginArea(new Rect(windowPadding, windowTitleHeight, windowRect.width - 2 * windowPadding, windowRect.height - windowTitleHeight));

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Print2Console")) {
            PrintProfileResultsToConsole();
        }
        if (GUILayout.Button("Reset")) {
            PrintProfileResultsToConsole();
            classes.Clear();
            allPerformanceCounters.Clear();
        }
        renderStatsDetails = GUILayout.Toggle(renderStatsDetails, "Show stats details"/*, GUILayout.Width(windowRect.width - 2 * windowPadding)*/);
        GUILayout.EndHorizontal();

        GUILayout.Space(paddingTop);

        float toggleHeight = GUI.skin.toggle.lineHeight * 1.8F;
        float labelHeight = GUI.skin.label.lineHeight * 1.5F;
        float buttonHeight = GUI.skin.button.lineHeight;
        // only render stats details if this is enabled...
        float height = windowTitleHeight + buttonHeight + windowPaddingBottom;
        float width = 300;
        string label = null;
        GUIStyle classStyle = new GUIStyle(GUI.skin.toggle);
        classStyle.alignment = TextAnchor.MiddleLeft;
        GUIStyle instanceStyle = new GUIStyle(GUI.skin.toggle);
        instanceStyle.alignment = TextAnchor.MiddleLeft;

        GUIStyle methodStyle = new GUIStyle(GUI.skin.label);
        methodStyle.alignment = TextAnchor.MiddleLeft;
        methodStyle.fixedHeight = instanceStyle.fixedHeight;

        GUIStyle rightAlignStyle = new GUIStyle(GUI.skin.label);
        rightAlignStyle.alignment = TextAnchor.MiddleRight;

        
        if (renderStatsDetails) {
            height += paddingTop;
            width = mainLabelWidth + 2 * windowPadding + GUI.skin.window.padding.left + GUI.skin.window.padding.right;
            width += RenderColumnHeaders() * (detailsWidth + rightAlignStyle.margin.left + rightAlignStyle.margin.right);
            height += labelHeight; // just rendered a row of labels
            foreach (PerformanceCounterClass pcClass in classes) {
                height += toggleHeight;
                GUILayout.BeginHorizontal();
                label = string.Format("{0} ({1})", pcClass.Name, pcClass.Instances.Count);
                pcClass.showChildren = GUILayout.Toggle(pcClass.showChildren, label, classStyle, GUILayout.Width(mainLabelWidth));
                RenderRowStats(pcClass, rightAlignStyle);
                GUILayout.EndHorizontal();
                if (pcClass.showChildren) {
                    foreach (PerformanceCounterInstance pcInstance in pcClass.Instances) {
                        height += toggleHeight;
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(indentWidth);
                        label = string.Format("{0} ({1})", pcInstance.Name, pcInstance.Methods.Count);
                        pcInstance.showChildren = GUILayout.Toggle(pcInstance.showChildren, label, instanceStyle, GUILayout.Width(mainLabelWidth - indentWidth));
                        RenderRowStats(pcInstance, rightAlignStyle);
                        GUILayout.EndHorizontal();
                        if (pcInstance.showChildren) {
                            foreach (JCsProfilerMethod pcMethod in pcInstance.Methods) {
                                height += labelHeight;
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(2 * indentWidth);
                                GUILayout.Label(pcMethod.Name, methodStyle, GUILayout.Width(mainLabelWidth - 2 * indentWidth));
                                RenderRowStats(pcMethod, rightAlignStyle);
                                GUILayout.EndHorizontal();
                            }
                        }
                        GUILayout.Space(paddingBottomMethods);
                        height += paddingBottomMethods;
                    }
                }
            }
        }
        windowRect = new Rect(windowRect.x, windowRect.y, width, height);

        GUILayout.EndArea();
        // Make the window draggable
        GUI.DragWindow(dragRect);

        pcm.CallIsFinished();
    }

    private int RenderColumnHeaders() {
        GUILayout.BeginHorizontal();

        GUIStyle centerAlignStyle = new GUIStyle(GUI.skin.label);
        centerAlignStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Space(mainLabelWidth);

        GUILayout.Label("TPassd", centerAlignStyle, GUILayout.Width(detailsWidth));
        GUILayout.Label("# Calls", centerAlignStyle, GUILayout.Width(detailsWidth));
        GUILayout.Label("TSpent", centerAlignStyle, GUILayout.Width(detailsWidth));
        GUILayout.Label("Min", centerAlignStyle, GUILayout.Width(detailsWidth));
        GUILayout.Label("Avg", centerAlignStyle, GUILayout.Width(detailsWidth));
        GUILayout.Label("Max", centerAlignStyle, GUILayout.Width(detailsWidth));
        GUILayout.Label("%o of T", centerAlignStyle, GUILayout.Width(detailsWidth));

        GUILayout.EndHorizontal();

        return 7;
    }

    private void RenderRowStats(PerformanceCounter pc, GUIStyle style) {
        GUILayout.Label(pc.SecondsSinceFirstCall.ToString("0"), style, GUILayout.Width(detailsWidth));
        GUILayout.Label(pc.CallCount.ToString(), style, GUILayout.Width(detailsWidth));
        GUILayout.Label(pc.TimeSpent.ToString("0"), style, GUILayout.Width(detailsWidth));
        GUILayout.Label(pc.TimeSpentMin.ToString("0.00"), style, GUILayout.Width(detailsWidth));
        GUILayout.Label((pc.TimeSpent / (float)pc.CallCount).ToString("0.00"), style, GUILayout.Width(detailsWidth));
        GUILayout.Label(pc.TimeSpentMax.ToString("0.00"), style, GUILayout.Width(detailsWidth));
        // pc.TimeSpent = seconds * 1000 / x => per mille...
        GUILayout.Label((pc.TimeSpent / pc.SecondsSinceFirstCall).ToString("0.0"), style, GUILayout.Width(detailsWidth));
    }

    /// <summary>
    ///     A co-routine that renders the profile results to text every
    ///     renderOutputIntervalSeconds seconds.
    /// </summary>
    public IEnumerator AutomaticTextRendering() {
        while (Application.isPlaying) {
            if (classes.Count > 0) {
                if (renderToConsole || renderToLogging) {
                    string text = CreateProfileText();
                    if (renderToConsole) {
                        print(text);
                    }
                    if (renderToLogging) {
                        //log.Info(text);
                    }
                }
            }
            yield return new WaitForSeconds(renderOutputIntervalSeconds);
        }
    }

    private void PrintProfileResultsToConsole() {
        print(CreateProfileText());
    }

    private void PrintProfileResultsToLogging() {
        //log.Info(CreateProfileText());
    }

    private string CreateProfileText() {
        /*
         * one SHOULD use a StringBuilder for this... but that would probably
         * bloat the Web player :-(
         * doing this without a string builder is not really efficient, but
         * neither is bloating the Web player, so I'll go with a lot of
         * string += otherString
         */
        const string emptySpace = "                                        ";
        const string separator = "===================================================================="
            + "================================================================";
        int minCharLength = renderTextClassWidth + 7 * renderTextColumnWidth;
        string result = string.Format("\n\n{0}\n", separator.Substring(0, minCharLength));
        string label = null;

        result += emptySpace.Substring(0, renderTextClassWidth);
        result += RenderTextColumnHeaders();
        result += "\n";

        foreach (PerformanceCounterClass pcClass in classes) {
            label = string.Format("{0}", pcClass.Name, pcClass.Instances.Count);
            result += label + emptySpace.Substring(0, Mathf.Max(0, renderTextClassWidth - label.Length));
            result += RenderTextRowStats(pcClass);
            result += "\n";
            foreach (PerformanceCounterInstance pcInstance in pcClass.Instances) {
                label = emptySpace.Substring(0, renderTextIndentWidth)
                    + string.Format("{0}", pcInstance.Name, pcInstance.Methods.Count);
                result += label + emptySpace.Substring(0, Mathf.Max(0, renderTextClassWidth - label.Length));
                result += RenderTextRowStats(pcInstance);
                result += "\n";
                foreach (JCsProfilerMethod pcMethod in pcInstance.Methods) {
                    label = emptySpace.Substring(0, 2 * renderTextIndentWidth)
                        + pcMethod.Name;
                    result += label + emptySpace.Substring(0, Mathf.Max(0, renderTextClassWidth - label.Length));
                    result += RenderTextRowStats(pcMethod);
                    result += "\n";
                }
            }
        }
        return result + string.Format("{0}\n\n", separator.Substring(0, minCharLength));
    }

    private string RenderTextColumnHeaders() {
        string result = "";
        result += AddRightAlignedText("TPassd");
        result += AddRightAlignedText("# Calls");
        result += AddRightAlignedText("TSpent");
        result += AddRightAlignedText("Min");
        result += AddRightAlignedText("Avg");
        result += AddRightAlignedText("Max");
        result += AddRightAlignedText("%o of T");
        return result;
    }


    private string RenderTextRowStats(PerformanceCounter pc) {
        string result = "";
        result += AddRightAlignedText(pc.SecondsSinceFirstCall.ToString("0"));
        result += AddRightAlignedText(pc.CallCount.ToString());
        result += AddRightAlignedText(pc.TimeSpent.ToString("0"));
        result += AddRightAlignedText(pc.TimeSpentMin.ToString("0.00"));
        result += AddRightAlignedText((pc.TimeSpent / (float)pc.CallCount).ToString("0.00"));
        result += AddRightAlignedText(pc.TimeSpentMax.ToString("0.00"));
        result += AddRightAlignedText((pc.TimeSpent / pc.SecondsSinceFirstCall).ToString("0.0"));
        return result;
    }

    private string AddRightAlignedText(string text) {
        const string emptySpace = "                                        ";
        return emptySpace.Substring(0, Mathf.Max(0, renderTextColumnWidth - text.Length))
            + text;
    }

    #endregion

    private JCsProfilerMethod dummy = new JCsProfilerMethod(null, null);

    /// <summary>
    ///     This method starts the profiling. Put it at the beginning of the
    ///     method you with to profile and don't forget to call CallIsFinished()
    ///     on the returned instance of JCsProfilerMethod.
    /// </summary>
    /// <param name="className">The name of the class which is being profiled.</param>
    /// <param name="instanceName">
    ///     The name of the instance calling the method, usually you would pass gameObject.Name
    /// </param>
    /// <param name="methodName">The name of the method being profiled without parenthesis.</param>
    /// <returns>
    ///     an instance of JCsProfilerMethod on which you MUST call
    ///     CallIsFinished() as last statement of the method you're profiling...
    /// </returns>
    public JCsProfilerMethod StartCallStopWatch(string className, string instanceName, string methodName) {
        if (trackProfiling) {
            // this is a bit of a waste, but it *should* be better than concatenating the strings...
            PerformanceCounterKey key = new PerformanceCounterKey(className, instanceName, methodName);
            JCsProfilerMethod method = null;
            if (allPerformanceCounters.ContainsKey(key)) {
                method = (JCsProfilerMethod)allPerformanceCounters[key];
            } else {
                method = CreateNewPCMethod(key);
            }
            method.StartCallStopWatch();
            return method;
        } else {
            return dummy;
        }
    }

    #region Don't look at this code ;-)
    private JCsProfilerMethod CreateNewPCMethod(PerformanceCounterKey key) {
        string className = key.ClassName;
        string instanceName = key.InstanceName;
        string methodName = key.MethodName;
        /*
         * The following operation is only done once for each method to
         * be profiled (when it's called for the very first time), so
         * instead of adding extra structures I do it the expansive way...
         */
        PerformanceCounterClass myClass = null;
        PerformanceCounterInstance myInstance = null;
        JCsProfilerMethod newMethod = null;
        // see if the class exists already...
        foreach (PerformanceCounterClass pcClass in classes) {
            if (pcClass.Name.Equals(className)) {
                myClass = pcClass;
                break;
            }
        }
        // if not - create it...
        if (myClass == null) {
            myClass = new PerformanceCounterClass(className);
            classes.Add(myClass);
        }
        // now see if that class has the relevant instance
        foreach (PerformanceCounterInstance pcInstance in myClass.Instances) {
            if (pcInstance.Name.Equals(instanceName)) {
                myInstance = pcInstance;
                break;
            }
        }
        // if not - create it...
        if (myInstance == null) {
            myInstance = new PerformanceCounterInstance(instanceName, myClass);
        }
        // now we do know the method doesn't exist, or we wouldn't be here right now ;-)
        newMethod = new JCsProfilerMethod(methodName, myInstance);
        allPerformanceCounters.Add(key, newMethod);
        return newMethod;
    }
    #endregion

    #region Helper classes
    private class PerformanceCounterKey {
        private string className = null;
        public string ClassName {
            get { return className; }
        }

        private string instanceName = null;
        public string InstanceName {
            get { return instanceName; }
        }

        private string methodName = null;
        public string MethodName {
            get { return methodName; }
        }

        public PerformanceCounterKey(string className, string instanceName, string methodName) {
            this.className = className;
            this.instanceName = instanceName;
            this.methodName = methodName;
        }

        public override bool Equals(object obj) {
            if (obj is PerformanceCounterKey) {
                PerformanceCounterKey other = (PerformanceCounterKey)obj;
                return this.className.Equals(other.className)
                    && this.instanceName.Equals(other.instanceName)
                    && this.methodName.Equals(other.methodName);
            }
            return false; // other objects never are equal...
        }

        public override int GetHashCode() {
            /*
             * this is somewhat unsafe but should work in most cases...
             * if you get different methods with the same values - you'll know this didn't work ;-)
             */
            return className.GetHashCode() * instanceName.GetHashCode() * methodName.GetHashCode();
        }
    }
    #endregion
}
