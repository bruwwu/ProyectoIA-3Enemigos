using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DebugManager;

public class LookAtYon : MonoBehaviour
{

    [Header("VisionCone")]
    public Transform juanitoTorreta;
    public Transform player;
    public float coneAngle;
    public float coneDistance;
    bool isDetected = false;

    [Header("Corrutina")]
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 juanitoDirection = juanitoTorreta.forward;

        if(Utilities.Utility.inInsideCone(coneAngle, coneDistance, player.position, juanitoTorreta.position, juanitoDirection)){
            StartCoroutine(lockIn(3, !isDetected));
        }
        else{
            StartCoroutine(rotateIdle(3, isDetected));
        }
    }

    void OnDrawGizmos()
    {
        if(DebugGizmoManager.VisionCone){
            Utility.DrawVisionCone(juanitoTorreta.position, coneAngle, coneDistance, isDetected, juanitoTorreta);
        }
    }

    IEnumerator rotateIdle(float seconds, bool isDetected){

            yield return new WaitForSeconds(seconds);
    }
    IEnumerator lockIn(float seconds, bool isDetected){
        Vector3 relativePos = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos);

        yield return new WaitForSeconds(5f);
    }
}
