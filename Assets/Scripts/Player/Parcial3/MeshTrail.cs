using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MeshTrail : MonoBehaviour
{
    PlayerStateMachine player;
    public float activeTime = 2f;

    public bool isTrailActive;

    public Transform positionToRespawn;

    public float meshReferenceRate = 0.1f;
    // Start is called before the first frame update

    public Material mat;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    public string shaderVarRef;

    public float shaderVarRate = 0.1f;

    public float shaderVarRefreshRate = 0.05f;

    public float meshDestroyDelay = 3f;
    void Start()
    {
        player = GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.isDodgeTap && player.dodgeBlock && !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActiveTrail(activeTime));
        }
    }

    IEnumerator ActiveTrail(float activeTime)
    {
        while (activeTime > 0)
        {
            activeTime -= meshReferenceRate;
            if(skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for(int i = 0; i<skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetLocalPositionAndRotation(positionToRespawn.position, positionToRespawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();

                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

               StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));
                Destroy(gObj, meshDestroyDelay);
            }
            yield return new WaitForSeconds(meshReferenceRate);
        }

        isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat (Material mat, float goal, float rate, float refrehRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);
        while(valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refrehRate);
        }
    }
}
