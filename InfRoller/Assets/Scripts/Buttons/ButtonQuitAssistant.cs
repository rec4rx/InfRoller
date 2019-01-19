using UnityEngine;
using UnityEngine.UI;

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
    }
}
