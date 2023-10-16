using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownWebcam : MonoBehaviour
{
    public TMP_Dropdown _dropdown;
    private int _realValue;
    private bool _wasOpened = false;
    private string _text;
    private string _none = "(none)";
    [SerializeField] private Button _button;

    // Start is called before the first frame update
    void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        SetNewList();
        if (GameManager.Instance.GetWebcam() != null)
        {
            int value = 0;
            foreach (var myoptiondata in _dropdown.options)
            {
                if (myoptiondata.text == GameManager.Instance.GetWebcam().deviceName)
                    _dropdown.value = value;

                value++;
            }
            _realValue = _dropdown.value;
        }
    }

    private void Start()
    {
        if(GameManager.Instance.GetWebcam() == null)
        {
            _realValue = 0;
            _text = _none;
            _dropdown.captionText.text = _none;
            _button.interactable = false;
        }
        else
        {
            _text = GameManager.Instance.GetWebcam().deviceName;
            _dropdown.captionText.text = GameManager.Instance.GetWebcam().deviceName;
            _button.interactable = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!_dropdown.IsExpanded)
        {
            if (_wasOpened == true && _dropdown.value != 0)
                SetCamera();
            else if (_dropdown.value == 0)
            {
                GameManager.Instance.StopWebcam();
                _button.interactable = false;
            }
            _wasOpened = _dropdown.IsExpanded;
            return;
        }
        SetNewList();
    }

    void SetCamera()
    {

        WebCamTexture webcamTexture = new WebCamTexture();

        webcamTexture.deviceName = _dropdown.captionText.text;

        GameManager.Instance.SetWebcam(webcamTexture);

        _button.interactable = true;
    }

    void SetNewList()
    {
        if (_wasOpened)
            return;

        _realValue = _dropdown.value;
        _text = _dropdown.captionText.text;

        _dropdown.ClearOptions();
        List<string> deviceList = new List<string>();
        WebCamDevice[] devices = WebCamTexture.devices;

        for (int i = 0; i < devices.Length; i++)
            deviceList.Add(devices[i].name);

        _dropdown.options.Add(new TMP_Dropdown.OptionData() { text = _none });

        foreach (string t in deviceList)
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData() { text = t });
        }
        _dropdown.value = _realValue;
        _wasOpened = _dropdown.IsExpanded;

        _dropdown.captionText.text = _text;
    }
}
