using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GlassesUIManager : MonoBehaviour
{
    public ARFaceManager arFaceManager;
    public Button[] glassesButtons;
    public Button[] buttonColors;
    private List<MeshRenderer> glasses = new List<MeshRenderer>();
    private int activeGlassesIndex;

    // Start is called before the first frame update
    void Start()
    {
        arFaceManager.facesChanged += OnARFacesChanged;

        for(int i = 0; i < glassesButtons.Length; i++)
        {
            var button = glassesButtons[i];
            int capturedIndex = i; // Capture i by value. This will fix the stale closure issue
            button.onClick.AddListener(() => { OnGlassesButtonClick(capturedIndex); });
            buttonColors[i].onClick.AddListener(() => { OnChangeColor(capturedIndex); });
        }
    }

    void OnARFacesChanged(ARFacesChangedEventArgs args) {
        if(args.added.Count > 0)
        { 
            ARFace face = args.added[0];
            Debug.Log($"Found face ${face.name}");
            glasses.Clear();
            for (int i = 0; i < face.transform.childCount; i++)
            {
                var child = face.transform.GetChild(i);
                Debug.Log($"Child {child.name}");
                if (!child.name.Contains("Glasses"))
                {
                    continue;
                }
                if (child.gameObject.activeSelf)
                {
                    activeGlassesIndex = i;
                }
                var component = child.GetComponent<MeshRenderer>();
                Debug.Log($"Found component {component}");
                glasses.Add(child.GetComponent<MeshRenderer>());
            }
        } else if(args.removed.Count > 0)
        {
            glasses.Clear();
        }
        
    }

    void OnGlassesButtonClick(int newActiveIndex)
    {
        Debug.Log($"Change Active Glasses : {newActiveIndex}");
        Debug.Log($"Glasses Count {glasses.Count}");
        glasses[activeGlassesIndex].gameObject.SetActive(false);
        glasses[newActiveIndex].gameObject.SetActive(true);
        activeGlassesIndex = newActiveIndex;
    }

    void OnChangeColor(int colorIndex)
    { 
        Debug.Log($"Active Index {activeGlassesIndex}");
        Debug.Log($"Color Index {colorIndex}");
        Debug.Log($"Glasses Count {glasses.Count}");
        Debug.Log($"Current Color {glasses[activeGlassesIndex].material.color}");
        Color newColor = buttonColors[colorIndex].image.color;
        Debug.Log($"New Color {newColor}");
        glasses[activeGlassesIndex].material.color = newColor;
    }
}
