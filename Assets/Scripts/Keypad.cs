using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SubmitCodeEvent : UnityEvent<string> { }

public class Keypad : MonoBehaviour
{
    public TMPro.TextMeshPro display;
    public int charLimit = 4;
    public string input = "";

    public SubmitCodeEvent onOK = new SubmitCodeEvent();

    // Start is called before the first frame update
    void Start()
    {
        display.text = input;
    }

    public void Write(string s)
    {
        if (input.Length + s.Length > charLimit)
        {
            return;
        }

        input += s;
        display.text = input;
    }

    public void OK()
    {
        onOK.Invoke(input);
    }

    public void Clear()
    {
        input = "";
        display.text = input;
    }

    public void ShowError()
    {
        StartCoroutine(ErrorCoroutine());
    }

    private IEnumerator ErrorCoroutine()
    {
        display.text = "#ERROR#";
        yield return new WaitForSeconds(3f);
        input = "";
        display.text = input;
    }
}
