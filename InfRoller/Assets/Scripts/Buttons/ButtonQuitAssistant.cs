using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The <c>ButtonQuitAssistant</c> class.
/// Handle common quiting app events on Quit button.
/// </summary>
public class ButtonQuitAssistant : Button
{
// Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log("OnClick Quit!");
        //TODO: There should be an confirm popup raised here.
        Application.Quit();
    }
}
