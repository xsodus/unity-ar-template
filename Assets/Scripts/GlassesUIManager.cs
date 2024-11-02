using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GlassesUIManager : MonoBehaviour
{
    public ARFaceManager arFaceManager;
    public Button[] glassesButtons;
    public Button[] buttonColors;
    private Dictionary<string,List<MeshRenderer>> activeGlassesDict = new Dictionary<string,List<MeshRenderer>>();
    private int activeGlassesIndex;

    // Start is called before the first frame update
    void Start()
    {
        arFaceManager.trackablesChanged.AddListener(OnTrackablesChanged);

        for(int i = 0; i < glassesButtons.Length; i++)
        {
            var button = glassesButtons[i];
            int capturedIndex = i; // Capture i by value. This will fix the stale closure issue
            button.onClick.AddListener(() => { OnGlassesButtonClick(capturedIndex); });
            buttonColors[i].onClick.AddListener(() => { OnChangeColor(capturedIndex); });
        }
    }

    void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARFace> args) {
        if(args.added.Count > 0)
        { 
            foreach (ARFace face in args.added)
            {
                Debug.Log($"Found face ${face.name}");
                List<MeshRenderer> glasses = new List<MeshRenderer>();
                // Create cached mesh renderers for each glasses object in the face for controlling grasses and material color later.
                for (int i = 0; i < face.transform.childCount; i++)
                {
                    var child = face.transform.GetChild(i);
                    
                    if (!child.name.Contains("Glasses"))
                    {
                        continue;
                    }
                    if (child.gameObject.activeSelf)
                    {
                        activeGlassesIndex = i;
                    }
                    var component = child.GetComponent<MeshRenderer>();
                    glasses.Add(component);
                }
                activeGlassesDict.Add(face.name, glasses);
            }
        } else if(args.removed.Count > 0)
        {
            // Destroy all glasses game objects which activeGlassesDict key is matching with removed face name
            foreach (var kvp in args.removed)
            {
                ARFace face = kvp.Value;
                Debug.Log($"Removed face ${face.name}");
                foreach (var glasses in activeGlassesDict[face.name])
                {
                    Destroy(glasses.gameObject);
                }
                activeGlassesDict.Remove(face.name);
            }
        }
    }

    void OnGlassesButtonClick(int newActiveIndex)
    {
        // Activate new glasses and deactivate old glasses in activeGlassesDict
        foreach (var glassesList in activeGlassesDict.Values)
        {
            glassesList[activeGlassesIndex].gameObject.SetActive(false);
            glassesList[newActiveIndex].gameObject.SetActive(true);
        }
        activeGlassesIndex = newActiveIndex;
    }

    void OnChangeColor(int colorIndex)
    { 
        Color newColor = buttonColors[colorIndex].image.color;
        foreach (var glassesList in activeGlassesDict.Values)
        {
            glassesList[activeGlassesIndex].material.color = newColor;
        }
    }

    void OnDestroy()
    {
        arFaceManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
        // Remove all listeners from all buttons
        for(int i = 0; i < glassesButtons.Length; i++)
        {
            var button = glassesButtons[i];
            button.onClick.RemoveAllListeners();
            buttonColors[i].onClick.RemoveAllListeners();
        }
    }
}
