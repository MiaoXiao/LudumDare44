using UnityEngine;
using UnityEngine.UI;

public class ColonyUIHandler : MonoBehaviour
{

    /// <summary>
    /// Text are just for displaying the information text. i.e. Honey Level:
    /// </summary>
    [SerializeField]
    private Text _temperatureText;

    [SerializeField]
    private Text _speedText;

    /// <summary>
    /// updated text variables
    /// </summary>
    [SerializeField]
    private Text _numBees;

    [SerializeField]
    private Sprite[] _tempImages;

    [SerializeField]
    private Sprite _beePopulationIcon;

    [SerializeField]
    private Image _temperatureIcon;

    [SerializeField]
    private Sprite _increasePopulation;

    [SerializeField]
    private Button _popButton;

    [SerializeField]
    private Image _trait1;
    [SerializeField]
    private Image _trait2;
    [SerializeField]
    private Image _trait3;

    private void Awake(){
        _popButton.onClick.AddListener( _buttonCall );
    }   

    private void _buttonCall(){
        _updateNumBees(HiveManager.Instance.ActiveColony);
        UpdateColonyUI(HiveManager.Instance.ActiveColony);
    }
    
    //taken from Region Data UI
    private void UpdateTemperature(beeColony c)
    {
        int iconIndex = 0;
        if (c.TemperatureResistance < 0)
        {
            iconIndex = 2;
        }
        else if (c.TemperatureResistance > 0)
        {
            iconIndex = 1;
        }
        _temperatureIcon.sprite = _tempImages[iconIndex];
    }

    private void _updateNumBees(beeColony currentColony){
        currentColony.addBees(HiveManager.Instance.CurrentHoney);
    }

    public void UpdateColonyUI(beeColony currentColony){
        _numBees.text = "x " + currentColony.numBees;
        _speedText.text = "Speed: " + currentColony.Speed + " mi/s";
        UpdateTemperature(currentColony);
    }

    public void UpdateTraits(beeColony currentColony){
        if (currentColony.numUpgrades == 0){
            _trait1.enabled = false;
            _trait2.enabled = false;
            _trait3.enabled = false;
        }
        else if (currentColony.numUpgrades == 1){
            _trait1.enabled = true;
            _trait1.sprite = currentColony.beeTraits[0].icon;
            _trait2.enabled = false;
            _trait3.enabled = false;
        }
        else if (currentColony.numUpgrades == 2)
        {
            _trait1.enabled = true;
            _trait2.enabled = true;
            _trait1.sprite = currentColony.beeTraits[0].icon;
            _trait2.sprite = currentColony.beeTraits[1].icon;
            _trait3.enabled = false;
        }
        else if (currentColony.numUpgrades == 3)
        {
            _trait1.enabled = true;
            _trait2.enabled = true;
            _trait3.enabled = true;
            _trait1.sprite = currentColony.beeTraits[0].icon;
            _trait2.sprite = currentColony.beeTraits[1].icon;
            _trait3.sprite = currentColony.beeTraits[2].icon;
        }
        else{
            return;
        }
    }

}
