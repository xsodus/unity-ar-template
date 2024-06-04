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
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            args.added.ForEach((ARTrackedImage addedImage) => {
                Debug.Log($"Detected! {addedImage.name}");
                Debug.Log($"Detected! {addedImage.gameObject.name}");
                Debug.Log($"ImageName {addedImage.referenceImage.name}");
                ReferredImage targetObject = Array.Find<ReferredImage>(referredImages,
                    element => addedImage.referenceImage.name.Equals(element.imageName));
                if (targetObject != null)
                {
                    // TODO : This should replace with object pooling machanism.
                    Debug.Log($"Create object! {targetObject.mappedObject.name}");
                    Instantiate(targetObject.mappedObject, addedImage.transform);
                }
            });

        } else if(args.removed.Count > 0)
        {
            args.removed.ForEach((ARTrackedImage removedObject) => {
                GameObject.Destroy(removedObject);
            });
        }
    }

  
}
