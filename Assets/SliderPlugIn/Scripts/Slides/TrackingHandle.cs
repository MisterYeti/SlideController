using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingHandle : DefaultTrackableEventHandler
{
    public Action found = () => { };
    public Action lost = () => { };

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        // extra behaviour here
        found();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        // extra behaviour here
        lost();
    }
}
