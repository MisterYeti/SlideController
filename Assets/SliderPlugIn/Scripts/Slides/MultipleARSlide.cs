using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultipleARSlide : Slide
{
    //Data about the trackers
    [SerializeField] private List<TrackingHandle> _imagesTargets = null;
    [SerializeField] private List<float> _distances;

    [SerializeField] private float _rotEpsilon = 0.2f;
    [SerializeField] private float _distEpsilon = 2.0f;

    [SerializeField] private List<TrackingHandle> _imagesTargetsFound = new List<TrackingHandle>();

    public UnityEvent trackerFound;

    private int _nbTrackers;
    private int _nbFound = 0;
    private bool _trackersFound = false;


    protected override void Awake()
    {
        base.Awake();
        if (_imagesTargets != null)
        {
            foreach (TrackingHandle imageTarget in _imagesTargets)
            {
                imageTarget.gameObject.SetActive(true);

                imageTarget.lost += () => {


                    _nbFound -= 1;
                    _imagesTargetsFound.Remove(imageTarget);              

                    //Are all trackers found ?
                    _trackersFound = AreTrackers(_nbFound);

                };
                imageTarget.found += () =>
                {

                    _nbFound += 1;
                    _imagesTargetsFound.Add(imageTarget);

                    //Are all trackers found ?
                    _trackersFound = AreTrackers(_nbFound);

                };
            }
        }
    }

    private void Start()
    {
        _nbTrackers = _imagesTargets.Count;
    }


    private void Update()
    {
        //All trackers have been found
        if (_trackersFound)
        {
            //Check if trackers are well rotationed and positioned !
            if (CheckIfWellRotationed() && CheckIfWellPositioned())
            {
                trackerFound.Invoke();
                foreach (TrackingHandle image in _imagesTargets)
                {
                    image.gameObject.SetActive(false);
                }
                Hide(null);
            }
        }
    }



    private bool CheckIfWellRotationed()
    {
        int nbWellRotationed = 0;
        float angle;

        for (int i = 0; i <= _imagesTargets.Count - 2; i++)
        {
            angle = Vector3.Dot(_imagesTargets[i].gameObject.transform.right, _imagesTargets[i + 1].gameObject.transform.right);
            nbWellRotationed =
                (angle <= 1 + _rotEpsilon && angle >= 1 - _rotEpsilon)
                ?
                nbWellRotationed += 1 : 0;
        }



        angle = Vector3.Dot(_imagesTargets[_imagesTargets.Count - 1].gameObject.transform.right, _imagesTargets[0].gameObject.transform.right);
        nbWellRotationed =
            (angle <= 1 + _rotEpsilon && angle >= 1 - _rotEpsilon)
            ?
            nbWellRotationed += 1 : 0;

        return AreTrackers(nbWellRotationed);
    }


    private bool CheckIfWellPositioned()
    {
        int nbWellPositioned = 0;
        float distance;

        //Looking for distances : Distance(tracker[0],tracker[1] ... to Distance(tracker[max-1],tracker[max]
        for (int i = 0; i <= _imagesTargets.Count - 2; i++)
        {
            distance = Vector3.Distance(_imagesTargets[i].gameObject.transform.localPosition, _imagesTargets[i + 1].gameObject.transform.localPosition);
            nbWellPositioned =
                (distance < _distances[i] + _distEpsilon && distance > _distances[i] - _distEpsilon)
                ?
                nbWellPositioned += 1 : 0;
        }

        //Looking for distance : Distance(tracker[max],tracker[0]
        distance = Vector3.Distance(_imagesTargets[_imagesTargets.Count - 1].gameObject.transform.localPosition, _imagesTargets[0].gameObject.transform.localPosition);
        nbWellPositioned =
                (distance < _distances[_distances.Count - 1] + _distEpsilon && distance > _distances[_distances.Count - 1] - _distEpsilon)
                ?
                nbWellPositioned += 1 : 0;

        return AreTrackers(nbWellPositioned);
    }


    //Compare nbElements with the number of trackers max
    private bool AreTrackers(int nbElement)
    {
        return (_nbTrackers == nbElement);
    }
}
