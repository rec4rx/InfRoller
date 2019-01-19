using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The <c>HomeController</c> class.
/// Includes UIs flow and logic on Home Scene
/// </summary>
/// 
public class HomeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TapToStartOnClicked handles tap to start event
    public void TapToStartOnClicked ()
    {
        ToGameScene();
    }

    // ToGameScene replace Home by Game scene
    private void ToGameScene ()
    {
        SceneManager.LoadScene(SceneName.GAME);
    }
}
