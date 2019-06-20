using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ARSlide : Slide
{
    [Header("Tracker")]
    public TrackingHandle imageTarget = null;

    public UnityEvent trackerFound;

    protected override void Awake()
    {
        base.Awake();

        if (imageTarget != null)
        {
            imageTarget.gameObject.SetActive(true);
            imageTarget.lost += () => { };
            imageTarget.found += () => {

                if (imageTarget.gameObject.activeSelf)
                {
                    Debug.Log("Tracker Found : Next");
                    trackerFound.Invoke();
                    imageTarget.gameObject.SetActive(false);
                    Hide(null);
                }

            };
        }
    }

}
