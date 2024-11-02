using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[Serializable]
public class ReferredImage {
    public string imageName;
    public GameObject mappedObject;
}

public class MultipleTrackedImagesManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private ReferredImage[] referredImages;

    private Dictionary<string, GameObject> referredImagesDict = new Dictionary<string, GameObject>();

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
            referredImagesDict[referredImage.imageName] = referredImage.mappedObject;
        }
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

                if (addedImage.referenceImage != null && referredImagesDict.TryGetValue(addedImage.referenceImage.name, out GameObject targetObject))
                {
                    Debug.Log($"Create object! {targetObject.name}");
                    Instantiate(targetObject, addedImage.transform);
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

    void OnDestroy()
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }
}
