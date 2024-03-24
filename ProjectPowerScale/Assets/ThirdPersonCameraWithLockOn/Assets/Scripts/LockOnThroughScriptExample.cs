using UnityEngine;
using System.Collections;
using ThirdPersonCameraWithLockOn;

public class LockOnThroughScriptExample : MonoBehaviour {

    public ThirdPersonCamera camScript;
    public GameObject lockOnObject;

	// Use this for initialization
	void Start () {

        StartCoroutine("ToggleLockOn");
	    
	}
	
	// Update is called once per frameS
	void Update () {
	
	}

    IEnumerator ToggleLockOn()
    {
        //while (true)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("Initiating lock on");
            camScript.InitiateLockOn(lockOnObject);
            yield return new WaitForSeconds(2f);
            Debug.Log("Exiting lock on");
            camScript.ExitLockOn();

        }

    }
}
