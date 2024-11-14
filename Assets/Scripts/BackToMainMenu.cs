
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class BackToMainMenu : MonoBehaviour
{
    private ARSession arSession;
    private Button self;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Button>();
        arSession = GameObject.Find("AR Session").GetComponent<ARSession>();
        self.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        arSession.Reset();
        SceneManager.LoadScene("MainMenu");
    }

    void OnDestroy()
    {
        self.onClick.RemoveListener(OnButtonClicked);
    }
}
