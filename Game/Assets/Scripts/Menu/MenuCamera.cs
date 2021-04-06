using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public static MenuCamera Instance;

    public float TimeToTransition;
    public AnimationCurve TransitionCurve;

    private Vector3 origPos;
    private Vector3 nextPos;
    private bool transitioning;
    private float transitionProgress;

    public void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Transition(Transform nextPos)
    {
        if(!transitioning)
        {
            origPos = transform.position;
            this.nextPos = nextPos.position;
            transitionProgress = 0;
            transitioning = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transitioning)
        {
            transitionProgress += Time.deltaTime;
            transitionProgress = Mathf.Clamp(transitionProgress, 0, TimeToTransition);
            transform.position = Vector3.Slerp(origPos, nextPos, TransitionCurve.Evaluate(transitionProgress / TimeToTransition));
            if(transitionProgress == TimeToTransition)
            {
                transitioning = false;
            }
        }
    }
}
