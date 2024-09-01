using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SettingScreenScript : MonoBehaviour
{

    [SerializeField]
    public int[] QualityOffset = { 1, 3, 5 };

    [SerializeField]
    public Slider VolumeSlider;
    [SerializeField]
    public Slider QualitySlider;
    [SerializeField]
    public Toggle MuteToggle;
    [SerializeField]
    public Toggle VibrateToggle;
    [SerializeField]
    public GameObject BG;

    public int QualityValue;
    public float VolumeValue;

    public bool MuteToggleEnabled;
    public bool VibrateToggleEnabled;

    // Start is called before the first frame update
    public void OnEnable()
    {
        GetData();
        UpdateUI();
    }

    private void GetData()
    {
        VolumeValue = 0.5f;
        QualityValue = 1;

        MuteToggleEnabled = false;
        VibrateToggleEnabled = true;
        Debug.Log("플레이어 데이터 불러오기");
    }

    private void UpdateUI()
    {
        QualitySlider.value = QualityValue;
        VolumeSlider.value = VolumeValue;
        MuteToggle.isOn = MuteToggleEnabled;
        VibrateToggle.isOn = VibrateToggleEnabled;

        if (MuteToggleEnabled)
        {
            VolumeSlider.interactable = false;
        }
        else
        {
            VolumeSlider.interactable = true;
        }
    }

    private void SetData()
    {
        Debug.Log("설정된 데이터 저장");
        //혹은 즉시 값 변경
    }

    public void CloseButtonPressed()
    {
        //설정 저장
        //gameObject.SetActive(false);
        BG.SetActive(false);
    }

    public void ResetDataButtonPressed()
    {
        Debug.Log("데이터 초기화");
    }

    public void MuteTogglePressed()
    {
        MuteToggleEnabled = MuteToggle.isOn;
        if (MuteToggleEnabled)
        {
            VolumeSlider.interactable = false;
        }
        else
        {
            VolumeSlider.interactable = true;
        }
    }

    public void VolumValueChanged()
    {
        VolumeValue = VolumeSlider.value;
    }

    public void VibrateTogglePressed()
    {
        VibrateToggleEnabled = VibrateToggle.isOn;
    }

    public void QualityValueChanged()
    {
        QualityValue = (int)QualitySlider.value;
        //QualitySettings.SetQualityLevel(QualityValue, true);  실제 품질 조정
    }

 
}
