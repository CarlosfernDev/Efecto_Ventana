using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamSetter : MonoBehaviour
{
    Image _image;
    string _webcamName;

    Texture2D _material;

    public float durationLerp;

    // Start is called before the first frame update
    void Awake()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.Instance.GetWebcam() == null /* || _webcamName == GameManager.Instance.GetWebcam().deviceName*/)
        {
            if (GameManager.Instance.GetWebcam() == null)
            {
                _webcamName = null;
                _image.material.mainTexture = null;
                _image.color = new Vector4(1, 1, 1, 0);
                _image.SetMaterialDirty();
            }

            return;
        }
            

        _image.enabled = true;

        //Cv2.ImWrite(Application.dataPath + "ImagenFinal.jpg", GameManager.Instance.WebCamMat());
        if (GameManager.Instance.GetWebcam().didUpdateThisFrame)
        {
            //_image.material.mainTexture = GameManager.Instance.WebcamToTexture2D(GameManager.Instance.GetWebcam());
            _image.material.mainTexture = GameManager.Instance.MatToTexture(GameManager.Instance.WebCamMat());
            _image.SetMaterialDirty();
        }
        _webcamName = GameManager.Instance.GetWebcam().deviceName;

        if (_image.color.a == 0)
            StartCoroutine(AnimateOpacity());
    }

    IEnumerator AnimateOpacity()
    {
        float blend = 0;
        float journey = 0;

        while (journey <= durationLerp)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / durationLerp);
            blend = Mathf.Lerp(0, 1, percent);

            _image.color = new Vector4(1, 1, 1, blend);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}
