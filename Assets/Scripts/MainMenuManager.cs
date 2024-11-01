using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button arFaceExample1;
    public Button arFaceExample2;
    public Button arImageMarker;
    public Button arPlaneMarker;
    // Start is called before the first frame update
    void Start()
    {
        arFaceExample1.onClick.AddListener(() => {
            SceneManager.LoadScene("ARFaceFilter1");
        });
        arFaceExample2.onClick.AddListener(() => {
            SceneManager.LoadScene("ARFaceFilter2");
        });
        arImageMarker.onClick.AddListener(() => {
            SceneManager.LoadScene("ARImageMarker");
        });
        arPlaneMarker.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("ARCreativeWork");
        });
    }

}
