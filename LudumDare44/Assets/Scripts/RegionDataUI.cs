﻿using UnityEngine.UI;
using UnityEngine;

public class RegionDataUI : MonoBehaviour {
    [Header("Temperature")]
    [SerializeField]
    private GameObject _thermometerNotch;
    [SerializeField]
    private Sprite[] _tempImages;
    [SerializeField]
    private Image _temperatureIcon;

    [Header("Predator")]
    [SerializeField]
    private Text _predatorText;
    [SerializeField]
    private Sprite[] _predatorSprites;
    [SerializeField]
    private Image _predatorIcon;

    [Header("Everything Else")]
    [SerializeField]
    private Text _regionRemainingHoney;
    [SerializeField]
    private Text _distanceToHive;
    [SerializeField]
    private Text _regionRisk;

    private float _thermNotchRatio;

    void Start() {
        //ratio is width of the thermometer image divided by absolute value max of the temp scale
        _thermNotchRatio = 150f / 9f;
    }

    /// <summary>
    /// Updates the region UI 
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    public void UpdateRegionUI(Region r, beeColony c) {
        UpdateTemperature(r);
        UpdatePredator(r);

        _regionRemainingHoney.text = r.RemainingHoneyCapacity.ToString();
        _distanceToHive.text = r.HiveDistance.ToString("F1") + " mi."; 
        _regionRisk.text = Formulas.Instance.showPotential(r, c).ToString() + "%";
        
    }

    public void UpdateHoneyCount(Region r) {
        _regionRemainingHoney.text = r.RemainingHoneyCapacity.ToString();
    }

    private void UpdateTemperature(Region r) {
        int iconIndex = 0;
        if(r.Temperature > 0) {
            iconIndex = 2;
        }
        else if (r.Temperature < 0) {
            iconIndex = 1;
        }
        _temperatureIcon.sprite = _tempImages[iconIndex];

        _thermometerNotch.transform.localPosition = new Vector3(r.Temperature * _thermNotchRatio, 0);
    }

    private void UpdatePredator(Region r) {
        _predatorIcon.sprite = _predatorSprites[r.PredatorLevel];
        string predatorWarning = "";

        switch (r.PredatorLevel) {
            case 0:
                predatorWarning = "None";
                break;
            case 1:
                predatorWarning = "Low";
                break;
            case 2:
                predatorWarning = "Med";
                break;
            case 3:
                predatorWarning = "High";
                break;
            default:
                predatorWarning = "N/A";
                break;
        }
        _predatorText.text = predatorWarning;
    }

}
