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
    private Text _numBees;

    [SerializeField]
    private Sprite _temperatureIcon;

    [SerializeField]
    private int _currentSpeed;

}
