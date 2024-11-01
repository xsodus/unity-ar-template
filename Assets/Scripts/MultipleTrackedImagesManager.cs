using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[Serializable]
public class ReferredImage {
    public string imageName;
    public GameObject mappedObject;
}

public class MultipleTrackedImagesManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    public ReferredImage[] referredImages;

    // Start is called before the first frame update
    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        // trackedImageManager.trackedImagesChanged was deprecated in AR Foundation 6 in Unity 6
        // [NEW!] trackedImageManager.trackablesChanged is introduced to receive the tracked image changes.
        trackedImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        if (args.added.Count > 0)
        {
            foreach (ARTrackedImage addedImage in args.added)
            {
                Debug.Log($"Detected! {addedImage.name}");
                Debug.Log($"Detected! {addedImage.gameObject.name}");
                Debug.Log($"ImageName {addedImage.referenceImage.name}");
                ReferredImage targetObject = Array.Find(referredImages, element => 
                    addedImage.referenceImage.name.Equals(element.imageName));
                if (targetObject != null)
                {
                    // TODO : This should replace with object pooling machanism.
                    Debug.Log($"Create object! {targetObject.mappedObject.name}");
                    Instantiate(targetObject.mappedObject, addedImage.transform);
                }
            }

        } else if(args.removed.Count > 0)
        {
            foreach (var removedObject in args.removed)
            {
                Destroy(removedObject.Value.gameObject);
            }
        }
    }
}
