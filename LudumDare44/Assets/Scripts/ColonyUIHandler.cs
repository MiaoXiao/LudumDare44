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

    [SerializeField]
    private Text _speedUnit;

    /// <summary>
    /// updated text variables
    /// </summary>
    [SerializeField]
    private int _numBees;

    [SerializeField]
    private Sprite _beePopulationIcon;

    [SerializeField]
    private Sprite _temperatureIcon;

    [SerializeField]
    private float _currentSpeed;

    [SerializeField]
    private Sprite _increasePopulation;

    [SerializeField]
    private Button _popButton;

    [SerializeField]
    private Transform _buttonLocation;

    private void Awake(){
        generateButtonLoc();
    }

    private void generateButtonLoc(){
        Button populationUpgrade = Instantiate(_popButton, _buttonLocation, false);
        populationUpgrade.GetComponent<Image>().sprite = _increasePopulation;
    }

    public void _updateColonyUI(beeColony currentColony)
    {
        _numBees = currentColony.numBees;
        _currentSpeed = currentColony.Speed;
    }

}
