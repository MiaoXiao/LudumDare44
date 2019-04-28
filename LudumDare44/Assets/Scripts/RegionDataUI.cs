using UnityEngine.UI;
using UnityEngine;

public class RegionDataUI : MonoBehaviour {
    [SerializeField]
    private Text _regionTemperature;
    [SerializeField]
    private Text _regionPredator;
    [SerializeField]
    private Text _regionRemainingHoney;
    [SerializeField]
    private Text _distanceToHive;

    [SerializeField]
    private Text _regionRisk;

    //UI Images?

    /// <summary>
    /// Updates the region UI 
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    public void UpdateRegionUI(Region r, beeColony c) {
        _regionTemperature.text = r.Temperature.ToString();
        _regionPredator.text = r.PredatorLevel.ToString();
        _regionRemainingHoney.text = r.RemainingHoneyCapacity.ToString();
        
    }

    public void UpdateHoneyCount(Region r) {
        _regionRemainingHoney.text = r.RemainingHoneyCapacity.ToString();
    }


}
