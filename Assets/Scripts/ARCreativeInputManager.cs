using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.EventSystems;

enum GameState
{
    SelectField,
    SelectMarker,
    DragMarker,
}
public class ARCreativeInputManager : MonoBehaviour
{
    public ARPlaneManager aRPlaneManager;
    public ARRaycastManager aRRayCastManager;
    List<ARRaycastHit> result = new List<ARRaycastHit>();

    public GameObject[] markerPrefab;

    public Toggle[] decorateButtons;

    [SerializeField]
    public int selectedMarkerIndex = -1;

    [SerializeField]
    private GameState currentState = GameState.SelectField;

    [SerializeField]
    private GameObject selectedMarker;

    [SerializeField]
    private List<GameObject> markerPool = new List<GameObject>();

    [SerializeField]
    private GameObject currentActiveMarker;

    Vector2? touchPosition = null;

    Vector3 decoratorOffset = new Vector3(0, 0.05f, 0);

    // Start is called before the first frame update
    void Start()
    {
        Array.ForEach(markerPrefab, item => {
            markerPool.Add(Instantiate(item, new Vector3(1000f, 1000f, 0f), Quaternion.Euler(Vector3.zero)));
        });

        Array.ForEach(decorateButtons, button => {
            button.onValueChanged.AddListener((value) => {
                var index = Array.IndexOf(decorateButtons, button);
                if (value)
                {
                    selectedMarkerIndex = index;
                    HandleSelectMarker();
                } else {
                    if(selectedMarkerIndex == index)
                    {
                        OnUnselect();
                    }
                }
            });
        });

        aRPlaneManager.planesChanged += (planes) => {
            if (currentState == GameState.SelectField)
            {
                if (planes.added.Count > 0)
                {
                    HandleSelectField();
                }
            }
        };
    }

    private void OnUnselect() {
        selectedMarkerIndex = -1;
        currentState = GameState.SelectMarker;
    }

    Vector2? GetTouchPosition()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) ? Input.mousePosition : null;
        }
        else
        {
            return Input.touchCount > 0 ? Input.GetTouch(0).position : null;
        }
    }

    void Update()
    {
        touchPosition = GetTouchPosition();
    }

    void FixedUpdate() {
        if(touchPosition == null)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // If the user is touching a UI element (like your toggle button), return immediately
            return;
        }
        if (!aRRayCastManager.Raycast(touchPosition ?? Vector2.zero, result, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds))
        {
            return;
        }
        if (result.Count == 0)
        {
            return;
        }

        switch (currentState)
        {
            case GameState.DragMarker:
                DragSelectedObject();
                
                break;
        }

        result.Clear();
    }


    private void HandleSelectMarker()
    {
        currentActiveMarker = markerPool[selectedMarkerIndex];
        currentState = GameState.DragMarker;
    }

    private void HandleSelectField()
    {
        currentState = GameState.SelectMarker;
    }

  

    // Generate a function to drag the selected object  
    public void DragSelectedObject()
    {
        var floorObj = Array.Find(result.ToArray(), item => {
            return item.trackable is ARPlane;
        });
        if (currentActiveMarker != null && floorObj != null)
        {
            currentActiveMarker.transform.position = floorObj.pose.position + decoratorOffset;
            currentActiveMarker.transform.rotation = floorObj.pose.rotation;
        }
    }
}
