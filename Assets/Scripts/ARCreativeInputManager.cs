using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

enum GameState
{
    ScanField,
    SelectMarker,
    DragMarker,
}

public class ARCreativeInputManager : MonoBehaviour
{
    public ARPlaneManager aRPlaneManager;
    public ARRaycastManager aRRayCastManager;

    [SerializeField]
    private List<ARRaycastHit> result = new List<ARRaycastHit>();

    public GameObject[] markerPrefab;

    public Button[] decorateButtons;

    public int selectedMarkerIndex = -1;

    [SerializeField]
    private GameState currentState = GameState.ScanField;

    [SerializeField]
    private List<GameObject> markerPool = new List<GameObject>();

    [SerializeField]
    private GameObject currentActiveMarker;

    [SerializeField]
    private GameObject[] tutorialUIObjects;

    public Vector3 decoratorOffset = new Vector3(0, 0.05f, 0);

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate all the marker prefabs and set them to a far away position
        foreach (var item in markerPrefab)
        {
            var markerInstance = Instantiate(item, new Vector3(1000f, 1000f, 0f), Quaternion.identity);
            markerPool.Add(markerInstance);
        }

        // Add event listeners to the toggle buttons
        for (int i = 0; i < decorateButtons.Length; i++)
        {
            int index = i; // Capture the current index to avoid closure issues
            decorateButtons[i].onClick.AddListener(() => {
                selectedMarkerIndex = index;
                currentActiveMarker = markerPool[selectedMarkerIndex];
                if(currentState == GameState.SelectMarker){
                    tutorialUIObjects[(int)currentState].SetActive(false);
                    currentState = GameState.DragMarker;
                }
            });
        }

        decorateButtons[0].transform.parent.gameObject.SetActive(false);

        // Add event listener to the ARPlaneManager
        // [NEW!] ARPlaneManager.planesChanged was deprecated in ARFoundation 6.0 and Unity version 6
        // This function is easier to manage all tracked object types with a single callback
        aRPlaneManager.trackablesChanged.AddListener(OnTrackableStateChangedEvent);
    }

    private void OnTrackableStateChangedEvent(ARTrackablesChangedEventArgs<ARPlane> arg)
    {
        // Allow the user to select a marker only when a new plane is starting to create
        if(arg.added.Count > 0){
            if(currentState == GameState.ScanField){  
                tutorialUIObjects[(int)currentState].SetActive(false);
                currentState = GameState.SelectMarker;
                tutorialUIObjects[(int)currentState].SetActive(true);
                decorateButtons[0].transform.parent.gameObject.SetActive(true);
            }
        }
    }


    Vector2? GetTouchPosition()
    {
        // Get the touch position from the input
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            // Get touch input from the editor
            return Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) ? Input.mousePosition : null;
        }
        else
        {
            // Get touch input from the physical device
            return Input.touchCount > 0 ? Input.GetTouch(0).position : null;
        }
    }

    void Update()
    {
        // Read input from the user
        var touchPosition = GetTouchPosition();
        if (touchPosition == null || EventSystem.current.IsPointerOverGameObject())
        {
            // If no touch position or the user is touching a UI element, return immediately
            return;
        }

        // Perform a raycast from the touch position to detect if it intersects with any AR planes within their bounds.
        if (!aRRayCastManager.Raycast(touchPosition.Value, result, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds) || result.Count == 0)
        {
            // If no intersection is found or the result is empty, return immediately
            return;
        }

        // Switch the current state to the appropriate state
        if (currentState == GameState.DragMarker)
        {
            DragSelectedObject();
        }

        // Clear the result list
        result.Clear();
    }

    public void DragSelectedObject()
    {
        // Find the plane object (blue grid)
        var floorObj = result.FirstOrDefault(item => item.trackable is ARPlane);

        // If the touched plane object is not found, return immediately
        if (currentActiveMarker == null || floorObj == null)
        {
            return;
        }

        // Set the position and rotation of the marker to the position and rotation of the touch point on the plane
        currentActiveMarker.transform.SetPositionAndRotation(
            floorObj.pose.position + decoratorOffset, 
            floorObj.pose.rotation
        );
    }

    void OnDestroy()
    {
        // Remove the event listener to prevent memory leaks
        aRPlaneManager.trackablesChanged.RemoveListener(OnTrackableStateChangedEvent);
    }
}
