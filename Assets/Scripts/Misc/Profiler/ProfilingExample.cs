using UnityEngine;
using System.Collections;

public class ProfilingExample : MonoBehaviour {

    void Awake() {
        JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch("ProfilingExample", gameObject.name, "Awake");
//        for (int i=0; i < 20000; i++) {
//        	string myName = transform.name;
//        }
        StartCoroutine(MyFunnyCoroutine());
        pm.CallIsFinished();
    }

	// Use this for initialization
	void Start() {
        JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch("ProfilingExample", gameObject.name, "Start");
//        for (int i=0; i < 1000; i++) {
//        	string myName = transform.name;
//        }
        pm.CallIsFinished();
	}
	
	// Update is called once per frame
	void Update() {
        JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch("ProfilingExample", gameObject.name, "Update");
//        for (int i=0; i < 500; i++) {
//        	string myName = transform.name;
//        }
        pm.CallIsFinished();
	}

	void FixedUpdate() {
        JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch("ProfilingExample", gameObject.name, "FixedUpdate");
//        for (int i=0; i < 500; i++) {
//        	string myName = transform.name;
//        }
        pm.CallIsFinished();
	}
	
	public IEnumerator MyFunnyCoroutine() {
		while (true) {
        	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch("ProfilingExample", gameObject.name, "MyFunnyCoroutine");
	        yield return new WaitForSeconds(1.0F);
	        pm.CallIsFinished();
		}
	}
}
