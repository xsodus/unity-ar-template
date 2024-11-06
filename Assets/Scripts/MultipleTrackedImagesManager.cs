using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[Serializable]
public struct ReferredImage {
    public string referenceImageName;
    public GameObject mappedObject;
}

public struct TrackedImageMapping {
    public string referenceImageName;
    public GameObject mappedPrefab;
    public GameObject mappedInstance;
}

public class MultipleTrackedImagesManager : MonoBehaviour
{
    [SerializeField]
    private ARSession aRSession;
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private ReferredImage[] referredImages;

    private Dictionary<string, TrackedImageMapping> referredImagesDict = new Dictionary<string, TrackedImageMapping>();

    // Start is called before the first frame update
    void Start()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        // trackedImageManager.trackedImagesChanged was deprecated in AR Foundation 6 in Unity 6
        // [NEW!] trackedImageManager.trackablesChanged is introduced to receive the tracked image changes.
        trackedImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
        // Create a dictionary for faster lookup
        foreach (var referredImage in referredImages)
        {
            var gameObject = Instantiate(referredImage.mappedObject, Vector3.zero, Quaternion.identity);
            gameObject.SetActive(false);
            referredImagesDict[referredImage.referenceImageName] = new TrackedImageMapping
            {
                referenceImageName = referredImage.referenceImageName,
                mappedPrefab = referredImage.mappedObject,
                mappedInstance = gameObject
            };
        }
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        if (args.added.Count > 0)
        {
            foreach (ARTrackedImage addedImage in args.added)
            {
                Debug.Log($"Detected! {addedImage.name}, {addedImage.gameObject.name}, ImageName: {addedImage.referenceImage.name}, TrackingState: {addedImage.trackingState}");
                
                if(addedImage.trackingState != TrackingState.Tracking)
                {
                    continue;
                }
                if (addedImage.referenceImage != null && referredImagesDict.TryGetValue(addedImage.referenceImage.name, out TrackedImageMapping targetObject))
                {
                    var targetObjectInstance = targetObject.mappedInstance;
                    targetObjectInstance.transform.parent = addedImage.transform;
                    targetObjectInstance.transform.localPosition = Vector3.zero;
                    targetObjectInstance.SetActive(true);
                }
            }

        } else if(args.removed.Count > 0)
        {
            foreach (var removedObject in args.removed)
            {
                if (removedObject.Value.referenceImage != null && referredImagesDict.TryGetValue(removedObject.Value.referenceImage.name, out TrackedImageMapping targetObject))
                {
                    var targetObjectInstance = targetObject.mappedInstance;
                    targetObjectInstance.SetActive(false);
                    targetObjectInstance.transform.parent = null;
                }
            }
        }
    }

    void OnDestroy()
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }
}
