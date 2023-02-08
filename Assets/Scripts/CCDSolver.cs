using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDSolver : MonoBehaviour
{
    public Transform tip;
    public int chainLength = 3;
    public int maxIters = 10;
    public Vector3 target = Vector3.zero;

    private Transform[] bones;
    private float epsilon = 0.01f;

    // Runs when resetting the CCDSolver component in the editor.
    void Reset()
    {
        //set tip as current GameObject
        tip = transform;

        //assign bones up the chain
        var current = transform;
        bones = new Transform[chainLength];
        try
        {
            for (int i = chainLength - 1; i >= 0; i--)
            {
                bones[i] = current.parent;
                current = current.parent;
            }
            Debug.Log("Bones assigned.");
        }
        catch (UnityException)
        {
            Debug.Log("Could not find required transforms.");
        }

        //reset bone rotation
        foreach (Transform bone in bones) {
            bone.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //set ik handle a bit away from limb for visibility
        target = tip.position * 1.25f;
    }

    void Awake()
    {

    }

    void SolveCCD()
    {
        for (int r = 0; r < maxIters; r++)
        {
            for (int i = chainLength - 1; i >= 0; i--)
            {
                //doing it all in local space
                var t_i = bones[i].InverseTransformDirection(Vector3.Normalize(target - bones[i].position));
                var e_i = bones[i].InverseTransformDirection(Vector3.Normalize(tip.position - bones[i].position));
                var a_i = Quaternion.FromToRotation(e_i, t_i);
                bones[i].Rotate(a_i.eulerAngles, Space.Self);

                //early out
                if (a_i.eulerAngles.magnitude < epsilon)
                {
                    return;
                }

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        SolveCCD();
    }
}
