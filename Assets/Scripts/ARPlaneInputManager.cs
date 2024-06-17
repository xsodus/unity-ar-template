using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneInputManager : MonoBehaviour
{
    ARRaycastManager aRRayCastManager;
    List<ARRaycastHit> result = new List<ARRaycastHit>();

    public GameObject markerProfab;

    [SerializeField]
    private GameObject currentActiveMarker;

    // Start is called before the first frame update
    void Start()
    {
        aRRayCastManager = GetComponent<ARRaycastManager>();
        currentActiveMarker = GameObject.Instantiate(markerProfab,new Vector3(1000f,1000f,0f),Quaternion.Euler(Vector3.zero));
    }

    Vector2? GetTouchPosition()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return Input.GetMouseButtonDown(0) ? Input.mousePosition : null;
        }
        else
        {
            return Input.touchCount == 0 ? Input.GetTouch(0).position : null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Vector2? touchPosition = GetTouchPosition();

        if(touchPosition == null)
        {
            return;
        }

        if (!aRRayCastManager.Raycast(touchPosition ?? Vector2.zero, result))
        {
            return;
        }
        if (result == null)
        {
            return;
        }

        result.ForEach(item => {
            Debug.Log($"item name:{item.trackable.gameObject.name}");
            Vector3 currentPosition = item.pose.position;
            currentPosition.Set(currentPosition.x, currentPosition.y + 0.1f, currentPosition.z);
            currentActiveMarker.transform.position = currentPosition;
        });
    }

   
}
