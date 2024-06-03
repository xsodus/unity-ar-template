using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleTrackedImagesManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    public string[] imageNames;

    public GameObject[] modelPrefabs;

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
                int index = Array.FindIndex(imageNames, element => addedImage.gameObject.name.Contains(element));
                Debug.Log($"Prefab index! {index}");
                if (index > -1)
                {
                    Debug.Log($"Create object! {modelPrefabs[index].name}");
                    Instantiate(modelPrefabs[index], addedImage.transform);
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
