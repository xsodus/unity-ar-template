using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARFaceUIManager : MonoBehaviour
{
     [SerializeField]
     RectTransform leftSidebar;
    [SerializeField]
    Button rotateButton;

    [SerializeField]
    ARCameraManager arCameraManager;

    [SerializeField]
    ARSession arSession;
    // Start is called before the first frame update
    void Start()
    {
       Vector3 iconRotation = new Vector3(0,0,leftSidebar.rotation.eulerAngles.z);
       iconRotation.z -= iconRotation.z;
       rotateButton.GetComponent<RectTransform>().rotation = Quaternion.Euler(iconRotation);
       rotateButton.onClick.AddListener(switchCamera);
    }

   void switchCamera ()
    {
    
        switch (arCameraManager.currentFacingDirection){
               case CameraFacingDirection.User:
                    arCameraManager.requestedFacingDirection = CameraFacingDirection.World;
                    break;
               default:
                    arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
                    break;
          }
    }

    void onDestroy() {
      rotateButton.onClick.RemoveListener(switchCamera);
    }
}
