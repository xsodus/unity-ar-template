using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button assignment1SceneButton;
    public Button glassesSceneButton;
    public Button arTrackedImage;
    // Start is called before the first frame update
    void Start()
    {
        assignment1SceneButton.onClick.AddListener(() => {
            SceneManager.LoadScene("ARFaceAssignment1");
        });
        glassesSceneButton.onClick.AddListener(() => {
            SceneManager.LoadScene("ARGlasses");
        });
        arTrackedImage.onClick.AddListener(() => {
            SceneManager.LoadScene("ARMarkerScene");
        });
    }

}
