using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopUi : MonoBehaviour
{
    [Header("Traits To Buy")]

    [SerializeField]
    private Trait[] _allTraits;

    [SerializeField]
    private int _dnaCost = 250;

    [Header("Purchase Icons")]

    [SerializeField]
    private Transform _buttonGridLayout;

    [SerializeField]
    private Button _traitButton;

    [Header("Trait Information")]

    [SerializeField]
    private Text _selectedTraitName;

    [SerializeField]
    private Text _selectedTraitDescription;

    [SerializeField]
    private Button _purchaseButton;

    [SerializeField]
    private Text _purchaseText;

    [Header("Toggle")]

    [SerializeField]
    private Transform _toggleOnPos;

    [SerializeField]
    private Transform _toggleOffPos;

    [SerializeField]
    private float _timeToToggle = 1.5f;

    [SerializeField]
    private Image _toggleImage;

    [SerializeField]
    private Sprite _activeTab;

    [SerializeField]
    private Sprite _hiddenTab;

    private bool _toggled = true;

    private Trait _currentlySelectedTrait;

    private bool _toggleInProgress = false;

    private void Awake()
    {
        _purchaseText.text = "Purchase for " + _dnaCost;
        GenerateTraitIcons();
        HiveManager.Instance.OnDNAChange += UpdatePurchaseStatus;
    }

    private void UpdatePurchaseStatus(int currentDna)
    {
        _purchaseButton.interactable = currentDna >= _dnaCost;
    }

    private void GenerateTraitIcons()
    {
        for (int i = 0; i < _allTraits.Length; ++i)
        {
            Button newTraitButton = Instantiate(_traitButton, _buttonGridLayout, false);

            Trait selected = _allTraits[i];
            newTraitButton.GetComponent<Image>().sprite = selected.icon;
            newTraitButton.onClick.AddListener(delegate { SetViewedTrait(selected); });
        }

        if (_allTraits != null &&_allTraits.Length > 0)
            SetViewedTrait(_allTraits[0]);
    }

    private void SetViewedTrait(Trait t)
    {
        _currentlySelectedTrait = t;
        _selectedTraitName.text = t.name;
        _selectedTraitDescription.text = t.description;
    }

    public void PurchaseSelectedTrait()
    {
        if(HiveManager.Instance.CurrentDNA >= _dnaCost){
            HiveManager.Instance.ActiveColony.addTrait(_currentlySelectedTrait);
            HiveManager.Instance.CurrentDNA -= _dnaCost;
            HiveManager.Instance.ColonyUIHandler.UpdateColonyUI(HiveManager.Instance.ActiveColony);
        }
        else{
            return;
        }
    }

    public void ToggleShopTab()
    {
        if (_toggleInProgress) return;

        _toggled = !_toggled;
        if (_toggled)
        {
            _toggleImage.sprite = _activeTab;
        }
        else
        {
            _toggleImage.sprite = _hiddenTab;
        }
        StartCoroutine(Toggle());
    }

    private IEnumerator Toggle()
    {
        _toggleInProgress = true;
        float startingTime = 0f;
        Vector3 starting = transform.position;
        Vector3 ending = _toggled ? _toggleOnPos.position : _toggleOffPos.position;
        while (startingTime < _timeToToggle)
        {
            startingTime += Time.deltaTime;
            Vector3 nextPos = Vector3.Slerp(starting, ending, startingTime / _timeToToggle);
            transform.position = nextPos;
            yield return null;
        }
        transform.position = ending;
        _toggleInProgress = false;
    }

}
