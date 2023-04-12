using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public enum BuildingBlocks
{
    none = 0,
    BlockCasual = 1,
    SphereCasual = 2,
    CylinderCasual = 3,
    ConeCasual = 4,

    UiTileBlockCasual = BlockCasual,
    UiTileSphereCasual = SphereCasual,
    UiTileCylinderCasual = CylinderCasual,
    UiTileConeCasual = ConeCasual,
}

public enum BlockColors
{
    none = 0,
    red = 1,
    orange = 2,
    brown = 3,
    yellow = 4,
    lightGrey = 5,
    green = 6,
    blue = 7,
    darkGrey = 8
}

public class UIManager : MonoBehaviour
{
    // Public Variables

    public Material RedMaterial;
    public Material OrangeMaterial;
    public Material BrownMaterial;
    public Material YellowMaterial;
    public Material LightGreyMaterial;
    public Material GreenMaterial;
    public Material BlueMaterial;
    public Material DarkGreyMaterial;

    // Private Variables

    private BlockColors _blockColor;

    private GridBuilder _gridBuilder;
    private GridTilePlacer _gridTilePlacer;
    [SerializeField]
    private ObjectBehaviour _preparationObjectBehaviour;
    public ObjectBehaviour PreparationObjectBehaviour
    {
        get { return _preparationObjectBehaviour; }
    }

    [Space(50)]
    [Header("Game Info")]
    [SerializeField]
    private GameObject _gameInfoPopupSection;

    [SerializeField]
    private Image _gameInfoPopupPage;

    private int _gameInfoPopupPageCounter = 1;
    private int _gameInfoPopupTotalPageCount = 6;

    [SerializeField]
    private TextMeshProUGUI _gameInfoPopupP1Text, _gameInfoPopupP2Text,
        _gameInfoPopupP3Text, _gameInfoPopupP4Text, _gameInfoPopupP5Text,
        _gameInfoPopupP6Text;

    [Space(5)]

    [SerializeField]
    private Button _gameInfoPopupLeftButton, _gameInfoPopupRightButton;

    [Space(20)]

    [Header("Languages")]

    //[SerializeField]
    //private  variablesGroupAsset

    // Language Toggles
    [SerializeField]
    private Toggle _languageChineseToggle, _languageEnglishToggle,
        _languageGermanToggle, _languageGreekToggle, _languageJapaneseToggle,
        _languageSpanishToggle;

    [Space(20)]

    [Header("Core")]

    // Core Toggles
    [SerializeField]
    private Toggle _coreBuildToggle, _coreEraserToggle, _coreTilePlacingModeToggle,
        _coreFreehandGridModeToggle, _coreVerticalBuildDirectionModeToggle;

    [Space(5)]

    // Core Toggles TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _coreBuildToggleText, _coreEraserToggleText,
        _coreTilePlacingModeToggleText, _coreFreehandGridModeToggleText,
        _coreVerticalBuildDirectionModeToggleText;

    [Space(20)]

    // Core Sliders
    [SerializeField]
    private Slider _coreAutoTilePlacingSpeedSlider;

    [Space(5)]

    // Core Sliders TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _coreAutoTilePlacingSpeedSliderTextHigh,
        _coreAutoTilePlacingSpeedSliderTextLow;

    [Space(20)]

    // Core Buttons
    [SerializeField]
    private Button _removeAllPlacedTilesButton;

    [Space(5)]

    // Core Buttons TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _removeAllPlacedTilesButtonText;

    [Header("Tile Pick")]

    // Tile Pick Buttons
    [SerializeField]
    private Button _tilePickLeftArrowButton, _tilePickRightArrowButton;

    // Tile Pick Toggles
    [SerializeField]
    private Toggle _tilePickBlockToggle, _tilePickSphereToggle,
        _tilePickCylinderToggle, _tilePickConeToggle;

    [Space(5)]

    // Core Buttons TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tilePickBlockToggleText, _tilePickSphereToggleText,
        _tilePickCylinderToggleText, _tilePickConeToggleText;

    [Space(5)]

    [SerializeField]
    private RectTransform _tilePickSectionRect;

    [Header("Tile Settings")]

    // Tile Settings Sliders
    [SerializeField]
    private Slider _tileSettingsSizeSlider, _tileSettingsScaleXSlider,
        _tileSettingsScaleYSlider, _tileSettingsScaleZSlider;
    
    [Space(5)]
    
    // Tile Settings Sliders TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tileSettingsSizeSliderText,
        _tileSettingsScaleXSliderText, _tileSettingsScaleYSliderText,
        _tileSettingsScaleZSliderText;

    [Space(5)]

    // Tile Settings Rects
    [SerializeField]
    private RectTransform _tileSettingsSectionRect, _tileSettingsPhysicsSectionRect,
        _tileSettingsFrictionSectionRect, _tileSettingsBounceSectionRect,
        _tileSettingsMassSectionRect, _tileSettingMetalicSectionRect,
        _tileSettingsMagneticStrengthRect, _tileSettingsMagneticRangeRect,
        _tileSettingsColorsSectionRect;

    [Space(5)]

    // Tile Settings Physics Toggles
    [SerializeField]
    private Toggle _tileSettingsPhysicsToggle, _tileSettingsGravityToggle;
    
    [Space(5)]

    // Tile Settings Physics Toggles TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tileSettingsPhysicsToggleText,
        _tileSettingsGravityToggleText;

    // Tile Settings Metalic Toggles
    [SerializeField]
    private Toggle _tileSettingsMetalicToggle, _tileSettingsMagneticToggle;

    [Space(5)]

    // Tile Settings Metalic Toggles TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tileSettingsMetalicToggleText,
        _tileSettingsMagneticToggleText;

    [Space(20)]

    // Tile Settings Metalic Magnetic Strength Toggles
    [SerializeField]
    private Toggle _tileSettingsMagneticStrengthLowToggle,
        _tileSettingsMagneticStrengthDefaultToggle,
        _tileSettingsMagneticStrengthHighToggle,
        _tileSettingsMagneticStrengthMaxToggle;

    [Space(5)]

    // Tile Settings Metalic Magnetic Strength Toggles TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tileSettingsMagneticStrengthLowToggleText,
        _tileSettingsMagneticStrengthDefaultToggleText,
        _tileSettingsMagneticStrengthHighToggleText,
        _tileSettingsMagneticStrengthMaxToggleText;

    [Space(20)]

    // Tile Settings Metalic Magnetic Range Toggles
    [SerializeField]
    private Toggle _tileSettingsMagneticRangeLowToggle,
        _tileSettingsMagneticRangeDefaultToggle,
        _tileSettingsMagneticRangeHighToggle,
        _tileSettingsMagneticRangeMaxToggle;

    [Space(5)]

    // Tile Settings Metalic Magnetic Range Toggles TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tileSettingsMagneticRangeLowToggleText,
        _tileSettingsMagneticRangeDefaultToggleText,
        _tileSettingsMagneticRangeHighToggleText,
        _tileSettingsMagneticRangeMaxToggleText;

    [Space(20)]

    // Tile Settings Friction Toggles
    [SerializeField]
    private Toggle _tileSettingsFrictionNoneToggle, _tileSettingsFrictionLowToggle,
        _tileSettingsFrictionDefaultToggle, _tileSettingsFrictionHighToggle,
        _tileSettingsFrictionMaxToggle;

    [Space(5)]

    // Tile Settings Friction Toggles TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tileSettingsFrictionNoneToggleText,
        _tileSettingsFrictionLowToggleText, _tileSettingsFrictionDefaultToggleText,
        _tileSettingsFrictionHighToggleText,_tileSettingsFrictionMaxToggleText;

    [Space(20)]

    // Tile Settings Bounce Toggles
    [SerializeField]
    private Toggle _tileSettingsBounceNoneToggle, _tileSettingsBounceLowToggle,
        _tileSettingsBounceDefaultToggle, _tileSettingsBounceHighToggle,
        _tileSettingsBounceMaxToggle;

    [Space(5)]

    // Tile Settings Bounce Toggles TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tileSettingsBounceNoneToggleText,
        _tileSettingsBounceLowToggleText, _tileSettingsBounceDefaultToggleText,
        _tileSettingsBounceHighToggleText, _tileSettingsBounceMaxToggleText;

    [Space(20)]

    // Tile Settings Mass Toggles
    [SerializeField]
    private Toggle _tileSettingsMassNoneToggle, _tileSettingsMassLowToggle,
        _tileSettingsMassDefaultToggle, _tileSettingsMassHighToggle,
        _tileSettingsMassMaxToggle;

    [Space(5)]

    // Tile Settings Mass Toggles TextsMeshProUGUIs
    [SerializeField]
    private TextMeshProUGUI _tileSettingsMassNoneToggleText,
        _tileSettingsMassLowToggleText, _tileSettingsMassDefaultToggleText,
        _tileSettingsMassHighToggleText, _tileSettingsMassMaxToggleText;

    [SerializeField] private GameObject _selectedTile;
    private BuildingBlocks _selectedTileType = BuildingBlocks.none;
    private GameObject _tileToPlace;

    [Space(20)]

    // Tile Settings Color Toggles
    [SerializeField]
    private Toggle _tileSettingsRedColorToggle, _tileSettingsOrangeColorToggle,
        _tileSettingsBrownColorToggle, _tileSettingsYellowColorToggle,
        _tileSettingsLightGreyColorToggle, _tileSettingsGreenColorToggle,
        _tileSettingsBlueColorToggle, _tileSettingsDarkGreyColorToggle;

    [Space(50)]

    [Header("Physic Materials")]
    [SerializeField] public PhysicMaterial _noBounceNoFriction;
    [SerializeField] public PhysicMaterial _noBounceLowFriction;
    [SerializeField] public PhysicMaterial _noBounceNormalFriction;
    [SerializeField] public PhysicMaterial _noBounceHighFriction;
    [SerializeField] public PhysicMaterial _noBounceMaxFriction;
    [SerializeField] public PhysicMaterial _lowBounceNoFriction;
    [SerializeField] public PhysicMaterial _lowBounceLowFriction;
    [SerializeField] public PhysicMaterial _lowBounceNormalFriction;
    [SerializeField] public PhysicMaterial _lowBounceHighFriction;
    [SerializeField] public PhysicMaterial _lowBounceMaxFriction;
    [SerializeField] public PhysicMaterial _normalBounceNoFriction;
    [SerializeField] public PhysicMaterial _normalBounceLowFriction;
    [SerializeField] public PhysicMaterial _normalBounceNormalFriction;
    [SerializeField] public PhysicMaterial _normalBounceHighFriction;
    [SerializeField] public PhysicMaterial _normalBounceMaxFriction;
    [SerializeField] public PhysicMaterial _highBounceNoFriction;
    [SerializeField] public PhysicMaterial _highBounceLowFriction;
    [SerializeField] public PhysicMaterial _highBounceNormalFriction;
    [SerializeField] public PhysicMaterial _highBounceHighFriction;
    [SerializeField] public PhysicMaterial _highBounceMaxFriction;
    [SerializeField] public PhysicMaterial _maxBounceNoFriction;
    [SerializeField] public PhysicMaterial _maxBounceLowFriction;
    [SerializeField] public PhysicMaterial _maxBounceNormalFriction;
    [SerializeField] public PhysicMaterial _maxBounceHighFriction;
    [SerializeField] public PhysicMaterial _maxBounceMaxFriction;

    private Slider _automaticTilePlacingSpeedModifier;
    private Slider _sizeSlider;
    private Slider _rescaleXSlider;
    private Slider _rescaleYSlider;
    private Slider _rescaleZSlider;

    private TextMeshProUGUI _sizeText;
    private TextMeshProUGUI _rescaleTextX;
    private TextMeshProUGUI _rescaleTextY;
    private TextMeshProUGUI _rescaleTextZ;
    private List<Toggle> _tileToggles = new();


    [SerializeField] private float _tileMassSetup;
    private float _timePerTilePlace = 0.33f;
    private float _timePerTilePlaceCounter = 0f;
    private float _tileRescaleXSliderValue = 1f;
    private float _tileRescaleYSliderValue = 1f;
    private float _tileRescaleZSliderValue = 1f;

    private bool _isAutoTilePlacing = false;
    private bool _isBuildMode = false;
    private bool _isEraserMode = false;
    private bool _isFreeHandBuilding = false;
    private bool _isTopToBottomBuildMode = false;

    // Getters & Setters

    public List<Toggle> TileToggles
    {
        get { return _tileToggles; }
    }

    public Slider AutomaticTilePlacingSpeedModifier
    {
        get { return _automaticTilePlacingSpeedModifier; }
    }

    public GameObject SelectedTile
    {
        get { return _selectedTile; }
    }

    public BuildingBlocks SelectedTileType
    {
        get { return _selectedTileType; }
        set { _selectedTileType = value; }
    }

    public GameObject TileToPlace
    {
        get { return _tileToPlace; }
        set { _tileToPlace = value; }
    }

    public float TimePerTilePlace
    {
        get { return _timePerTilePlace; }
        set { _timePerTilePlace = value; }
    }

    public float TimePerTilePlaceCounter
    {
        get { return _timePerTilePlaceCounter; }
        set { _timePerTilePlaceCounter = value; }
    }

    public float TileRescaleXSliderValue
    {
        get { return _tileRescaleXSliderValue; }
    }

    public float TileRescaleYSliderValue
    {
        get { return _tileRescaleYSliderValue; }
    }

    public float TileRescaleZSliderValue
    {
        get { return _tileRescaleZSliderValue; }
    }

    public bool IsAutoTilePlacing
    {
        get { return _isAutoTilePlacing; }
        set { _isAutoTilePlacing = value; }
    }

    public bool IsBuildMode
    {
        get { return _isBuildMode; }
        set { _isBuildMode = value; }
    }

    public bool IsEraserMode
    {
        get { return _isEraserMode; }
        set { _isEraserMode = value; }
    }

    public bool IsFreeHandBuilding
    {
        get { return _isFreeHandBuilding; }
    }

    public bool IsTopToBottomBuildMode
    {
        get { return _isTopToBottomBuildMode; }
    }

    public BlockColors BlockColor
    {
        get { return _blockColor; }
    }

    private List<string> _uIVariables = new();

    private void Awake()
    {
        _gridBuilder = FindObjectOfType<GridBuilder>(true);
        _gridTilePlacer = FindObjectOfType<GridTilePlacer>(true);
        _preparationObjectBehaviour.GridBuilderRef = _gridBuilder;
        _preparationObjectBehaviour.GridTilePlacerRef = _gridTilePlacer;
        
        _selectedTile.transform.localScale = new Vector3(
           _gridBuilder.GridTileSize,
           _gridBuilder.GridTileSize,
           _gridBuilder.GridTileSize);
        _selectedTile.SetActive(false);

        SetupUIToggleListeners();
        SetupOtherUIElements();

        Presettings();

        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));


        var source = LocalizationSettings.StringDatabase.SmartFormatter
            .GetSourceExtension<PersistentVariablesSource>();

        ICollection<string> keys = source["globalUI"].Keys;

        using (PersistentVariablesSource.UpdateScope())
        {
            foreach (string key in keys)
            {
                StringVariable value = source["globalUI"][key] as StringVariable;

                switch (key)
                {
                    case "Size":
                        value.Value = _gridBuilder.GridTileSize.ToString();
                        break;
                    case "XScale":
                        value.Value = _tileSettingsScaleXSlider.value.ToString("0.00");
                        break;
                    case "YScale":
                        value.Value = _tileSettingsScaleYSlider.value.ToString("0.00");
                        break;
                    case "ZScale":
                        value.Value = _tileSettingsScaleZSlider.value.ToString("0.00");
                        break;
                    default:
                        break;
                }
            }
        }

        GameInfoPopupPageButtonUpdate();
        gameInfoUpdatePageCounter();
        //var nestedGroup = source["global"] as NestedVariablesGroup;

        // An UpdateScope or using BeginUpdating and EndUpdating can be used to combine multiple changes into a single Update.
        // This prevents unnecessary string refreshes when updating multiple Global Variables.

        //foreach (string key in source["globalUI"].Keys)
        //{
        //    _uIVariables.Add(key);
        //    var itemValue = (source["globalUI"][key] as IntVariable).Value; // Get VariablesGroupAsset item key
        //    var itemKey = source["globalUI"][key];
        //    var itemKeyAndValue = source["globalUI"];

        //    print(itemValue);
        //    print(itemKey);
        //    print(itemKeyAndValue);
        //}

        //using (PersistentVariablesSource.UpdateScope())
        //{
        //    //foreach (var name in _uIVariables)
        //    //{

        //        //print(somesome);
        //        //foreach (var item in somesome)
        //        //{
        //        //    print(item);
        //        //    //print(somesomethin.GetSourceValue());
        //        //}
        //        //foreach (var thin in somesome.Values)
        //        //{
        //        //    print(thin);
        //        //}
        //        //foreach (var thing in somesome.Keys)
        //        //{
        //        //    print(thing);
        //        //}
        //        //var variable = source["globalUI"][name] as IntVariable;
        //        //variable.Value = Random.Range(0, 10);
        //    //}
        //}

        //var source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        //print(source["globalUI"]);
        //var thereal = source["globalUI"];
        //print(thereal.Count);
        //print(thereal.Values[_uIVariables[0]]);

    }

    private void Presettings()
    {
        _tileSettingsFrictionDefaultToggle.isOn = true;
        _tileSettingsBounceDefaultToggle.isOn = true;
        _tileSettingsMassDefaultToggle.isOn = true;
        _tileSettingsMagneticStrengthDefaultToggle.isOn = true;
        _tileSettingsMagneticRangeDefaultToggle.isOn = true;

        _tileSettingsGravityToggle.isOn = true;
        _tileSettingsGravityToggle.isOn = false;

        _tileSettingsMagneticToggle.isOn = true;
        _tileSettingsMagneticToggle.isOn = false;

        _tileSettingsMetalicToggle.isOn = true;
        _tileSettingsMetalicToggle.isOn = false;

        _tileSettingsPhysicsToggle.isOn = true;
        _tileSettingsPhysicsToggle.isOn = false;

    }

    public void PrintPrepTileValues()
    {
        print("Kinematic " + _preparationObjectBehaviour.ObjectKinematic + "\nHi!! \n\nHigh again!");
        print("Gravity " + _preparationObjectBehaviour.ObjectGravity);
        print("Magnet " + _preparationObjectBehaviour.ObjectMagnet);
        print("Magnet Range " + _preparationObjectBehaviour.ObjectMagnetRange);
        print("Magnet Strength " + _preparationObjectBehaviour.ObjectMagnetStrength);
        print("Mass " + _preparationObjectBehaviour.ObjectMass);
        print("Physic Mat " + _preparationObjectBehaviour.ObjectPhysicMaterial);
        print("Type " + _preparationObjectBehaviour.ObjectType);
    }

    private void SetupUIToggleListeners()
    {
        // Language Toggles
        _languageChineseToggle.onValueChanged.AddListener(
            delegate { ChineseLanguageToggling(); });
        _languageEnglishToggle.onValueChanged.AddListener(
            delegate { EnglishLanguageToggling(); });
        _languageGermanToggle.onValueChanged.AddListener(
            delegate { GermanLanguageToggling(); });
        _languageGreekToggle.onValueChanged.AddListener(
            delegate { GreekLanguageToggling(); });
        _languageJapaneseToggle.onValueChanged.AddListener(
            delegate { JapaneseLanguageToggling(); });
        _languageSpanishToggle.onValueChanged.AddListener(
            delegate { SpanishLanguageToggling(); });

        // Tile Pick Toggles
        _tilePickBlockToggle.onValueChanged.AddListener(
            delegate { UITileTogglesMaster(_tilePickBlockToggle); });
        _tilePickSphereToggle.onValueChanged.AddListener(
            delegate { UITileTogglesMaster(_tilePickSphereToggle); });
        _tilePickCylinderToggle.onValueChanged.AddListener(
            delegate { UITileTogglesMaster(_tilePickCylinderToggle); });
        _tilePickConeToggle.onValueChanged.AddListener(
            delegate { UITileTogglesMaster(_tilePickConeToggle); });

        // Core Toggles
        _coreBuildToggle.onValueChanged.AddListener(
            delegate { BuildModeToggling(); });
        _coreEraserToggle.onValueChanged.AddListener(
            delegate { EraserModeToggling(); });
        _coreTilePlacingModeToggle.onValueChanged.AddListener(
            delegate { TilePlacingModeToggling(); });
        _coreFreehandGridModeToggle.onValueChanged.AddListener(
            delegate { FreeHandModeToggling(); });
        _coreVerticalBuildDirectionModeToggle.onValueChanged.AddListener(
            delegate { BottomTopBuildDirectionToggle(); });

        // Tile Settings Friction
        _tileSettingsFrictionNoneToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });
        _tileSettingsFrictionLowToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });
        _tileSettingsFrictionDefaultToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });
        _tileSettingsFrictionHighToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });
        _tileSettingsFrictionMaxToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });

        // Tile Settings Bounce
        _tileSettingsBounceNoneToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });
        _tileSettingsBounceLowToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });
        _tileSettingsBounceDefaultToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });
        _tileSettingsBounceHighToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });
        _tileSettingsBounceMaxToggle.onValueChanged.AddListener(
            delegate { PhysicMaterialSetupToggle(); });

        // Tile Settings Mass
        _tileSettingsMassNoneToggle.onValueChanged.AddListener(
            delegate { MassSetupToggle(); });
        _tileSettingsMassLowToggle.onValueChanged.AddListener(
            delegate { MassSetupToggle(); });
        _tileSettingsMassDefaultToggle.onValueChanged.AddListener(
            delegate { MassSetupToggle(); });
        _tileSettingsMassHighToggle.onValueChanged.AddListener(
            delegate { MassSetupToggle(); });
        _tileSettingsMassMaxToggle.onValueChanged.AddListener(
            delegate { MassSetupToggle(); });

        // Tile Settings Physics
        _tileSettingsPhysicsToggle.onValueChanged.AddListener(
            delegate { PhysicsSetupToggle(); });
        _tileSettingsGravityToggle.onValueChanged.AddListener(
            delegate { GravitySetupToggle(); });

        // Tile Settings Metalic
        _tileSettingsMetalicToggle.onValueChanged.AddListener(
            delegate { MetalicSetupToggle(); });
        _tileSettingsMagneticToggle.onValueChanged.AddListener(
            delegate { MagneticSetupToggle(); });

        // Tile Settings Metalic Magnetic Strength
        _tileSettingsMagneticStrengthLowToggle.onValueChanged.AddListener(
            delegate { MagneticStrengthSetupToggle(); });
        _tileSettingsMagneticStrengthDefaultToggle.onValueChanged.AddListener(
            delegate { MagneticStrengthSetupToggle(); });
        _tileSettingsMagneticStrengthHighToggle.onValueChanged.AddListener(
            delegate { MagneticStrengthSetupToggle(); });
        _tileSettingsMagneticStrengthMaxToggle.onValueChanged.AddListener(
            delegate { MagneticStrengthSetupToggle(); });

        // Tile Settings Metalic Magnetic Range
        _tileSettingsMagneticRangeLowToggle.onValueChanged.AddListener(
            delegate { MagneticRangeSetupToggle(); });
        _tileSettingsMagneticRangeDefaultToggle.onValueChanged.AddListener(
            delegate { MagneticRangeSetupToggle(); });
        _tileSettingsMagneticRangeHighToggle.onValueChanged.AddListener(
            delegate { MagneticRangeSetupToggle(); });
        _tileSettingsMagneticRangeMaxToggle.onValueChanged.AddListener(
            delegate { MagneticRangeSetupToggle(); });

        // Tile Settings Color Toggles
        _tileSettingsRedColorToggle.onValueChanged.AddListener(
            delegate { RedColorToggle(); });
        _tileSettingsOrangeColorToggle.onValueChanged.AddListener(
            delegate { OrangeColorToggle(); });
        _tileSettingsBrownColorToggle.onValueChanged.AddListener(
            delegate { BrownColorToggle(); });
        _tileSettingsYellowColorToggle.onValueChanged.AddListener(
            delegate { YellowColorToggle(); });
        _tileSettingsLightGreyColorToggle.onValueChanged.AddListener(
            delegate { LightGreyColorToggle(); });
        _tileSettingsGreenColorToggle.onValueChanged.AddListener(
            delegate { GreenColorToggle(); });
        _tileSettingsBlueColorToggle.onValueChanged.AddListener(
            delegate { BlueColorToggle(); });
        _tileSettingsDarkGreyColorToggle.onValueChanged.AddListener(
            delegate { DarkGreyColorToggle(); });
    }

    private void ChineseLanguageToggling() =>
        LocalizationSettings.SelectedLocale = _languageChineseToggle.isOn ?
        LocalizationSettings.AvailableLocales.Locales[0] :
        LocalizationSettings.AvailableLocales.Locales[1];

    private void EnglishLanguageToggling() =>
        LocalizationSettings.SelectedLocale = _languageEnglishToggle.isOn ?
        LocalizationSettings.AvailableLocales.Locales[1] :
        LocalizationSettings.AvailableLocales.Locales[1];

    private void GermanLanguageToggling() =>
        LocalizationSettings.SelectedLocale = _languageGermanToggle.isOn ?
        LocalizationSettings.AvailableLocales.Locales[2] :
        LocalizationSettings.AvailableLocales.Locales[1];

    private void GreekLanguageToggling() =>
        LocalizationSettings.SelectedLocale = _languageGreekToggle.isOn ?
        LocalizationSettings.AvailableLocales.Locales[3] :
        LocalizationSettings.AvailableLocales.Locales[1];

    private void JapaneseLanguageToggling() =>
        LocalizationSettings.SelectedLocale = _languageJapaneseToggle.isOn ?
        LocalizationSettings.AvailableLocales.Locales[4] :
        LocalizationSettings.AvailableLocales.Locales[1];

    private void SpanishLanguageToggling() =>
    LocalizationSettings.SelectedLocale = _languageSpanishToggle.isOn ?
    LocalizationSettings.AvailableLocales.Locales[5] :
    LocalizationSettings.AvailableLocales.Locales[1];

    private void UITileTogglesMaster(Toggle toggle)
    {
        _selectedTileType = Enum.Parse<BuildingBlocks>(toggle.name, true);

        _tileToPlace = _gridTilePlacer.AvailableTiles.Find(
            res => _selectedTileType == Enum.Parse<BuildingBlocks>(res.name, true));
        _tileToPlace.transform.localScale = new Vector3(
            _gridBuilder.GridTileSize,
            _gridBuilder.GridTileSize,
            _gridBuilder.GridTileSize);

        if (_tilePickBlockToggle.isOn || _tilePickSphereToggle.isOn ||
            _tilePickCylinderToggle.isOn || _tilePickConeToggle.isOn)
        {
            if (_isBuildMode)
            {
                _sizeSlider.interactable = true;
                _rescaleXSlider.interactable = true;
                _rescaleYSlider.interactable = true;
                _rescaleZSlider.interactable = true;
                if (!_tileSettingsPhysicsSectionRect.gameObject.activeSelf)
                    _tileSettingsPhysicsSectionRect.gameObject.SetActive(true);
                if (!_tileSettingsColorsSectionRect.gameObject.activeSelf)
                    _tileSettingsColorsSectionRect.gameObject.SetActive(true);
                if (!_tileSettingsSectionRect.gameObject.activeSelf)
                    _tileSettingsSectionRect.gameObject.SetActive(true);
            }

            return;
        }

        _sizeSlider.interactable = false;
        _rescaleXSlider.interactable = false;
        _rescaleYSlider.interactable = false;
        _rescaleZSlider.interactable = false;
        if (_tileSettingsPhysicsSectionRect.gameObject.activeSelf)
            _tileSettingsPhysicsSectionRect.gameObject.SetActive(false);
        if (_tileSettingsColorsSectionRect.gameObject.activeSelf)
            _tileSettingsColorsSectionRect.gameObject.SetActive(false);
        if (_tileSettingsSectionRect.gameObject.activeSelf)
            _tileSettingsSectionRect.gameObject.SetActive(false);

        _selectedTileType = BuildingBlocks.none;
    }

    private void BuildModeToggling()
    {
        if (_coreBuildToggle.isOn)
        {
            _isBuildMode = true;
            if (!_tilePickSectionRect.gameObject.activeSelf)
                _tilePickSectionRect.gameObject.SetActive(true);

            if (_selectedTileType != BuildingBlocks.none)
            {
                _sizeSlider.interactable = true;
                _rescaleXSlider.interactable = true;
                _rescaleYSlider.interactable = true;
                _rescaleZSlider.interactable = true;
                if (!_tileSettingsPhysicsSectionRect.gameObject.activeSelf)
                    _tileSettingsPhysicsSectionRect.gameObject.SetActive(true);
                if (!_tileSettingsColorsSectionRect.gameObject.activeSelf)
                    _tileSettingsColorsSectionRect.gameObject.SetActive(true);
                if (!_tileSettingsSectionRect.gameObject.activeSelf)
                    _tileSettingsSectionRect.gameObject.SetActive(true);
            }
        }
        else if (!_coreBuildToggle.isOn)
        {
            _isBuildMode = false;
            if (_tilePickSectionRect.gameObject.activeSelf)
                _tilePickSectionRect.gameObject.SetActive(false);
            _sizeSlider.interactable = false;
            _rescaleXSlider.interactable = false;
            _rescaleYSlider.interactable = false;
            _rescaleZSlider.interactable = false;
            if (_tileSettingsPhysicsSectionRect.gameObject.activeSelf)
                _tileSettingsPhysicsSectionRect.gameObject.SetActive(false);
            if (_tileSettingsColorsSectionRect.gameObject.activeSelf)
                _tileSettingsColorsSectionRect.gameObject.SetActive(false);
            if (_tileSettingsSectionRect.gameObject.activeSelf)
                _tileSettingsSectionRect.gameObject.SetActive(false);
        }
    }
    

    private void TilePlacingModeToggling()
    {
        if (_coreTilePlacingModeToggle.isOn)
        {
            _isAutoTilePlacing = true;
            _automaticTilePlacingSpeedModifier.gameObject.SetActive(true);
        }
        else if (!_coreTilePlacingModeToggle.isOn)
        {
            _isAutoTilePlacing = false;
            _automaticTilePlacingSpeedModifier.gameObject.SetActive(false);
        }
    }

    private void EraserModeToggling()
    {
        if (_coreEraserToggle.isOn)
        {
            _isEraserMode = true;
        }
        else if (!_coreEraserToggle.isOn)
        {
            _isEraserMode = false;
        }
        //_isEraserMode = _coreEraserToggle.isOn ? true : false;
    }

    private void FreeHandModeToggling() =>
        _isFreeHandBuilding = _coreFreehandGridModeToggle.isOn ? true : false;

    private void BottomTopBuildDirectionToggle() =>
        _isTopToBottomBuildMode = _coreVerticalBuildDirectionModeToggle.isOn ?
        true : false;

    private void PhysicMaterialSetupToggle()
    {
        if (_tileSettingsBounceNoneToggle.isOn && _tileSettingsFrictionNoneToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _noBounceNoFriction;
        else if (_tileSettingsBounceNoneToggle.isOn && _tileSettingsFrictionLowToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _noBounceLowFriction;
        else if (_tileSettingsBounceNoneToggle.isOn && _tileSettingsFrictionDefaultToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _noBounceNormalFriction;
        else if (_tileSettingsBounceNoneToggle.isOn && _tileSettingsFrictionHighToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _noBounceHighFriction;
        else if (_tileSettingsBounceNoneToggle.isOn && _tileSettingsFrictionMaxToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _noBounceMaxFriction;
        else if (_tileSettingsBounceLowToggle.isOn && _tileSettingsFrictionNoneToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _lowBounceNoFriction;
        else if (_tileSettingsBounceLowToggle.isOn && _tileSettingsFrictionLowToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _lowBounceLowFriction;
        else if (_tileSettingsBounceLowToggle.isOn && _tileSettingsFrictionDefaultToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _lowBounceNormalFriction;
        else if (_tileSettingsBounceLowToggle.isOn && _tileSettingsFrictionHighToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _lowBounceHighFriction;
        else if (_tileSettingsBounceLowToggle.isOn && _tileSettingsFrictionMaxToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _lowBounceMaxFriction;
        else if (_tileSettingsBounceDefaultToggle.isOn && _tileSettingsFrictionNoneToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _normalBounceNoFriction;
        else if (_tileSettingsBounceDefaultToggle.isOn && _tileSettingsFrictionLowToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _normalBounceLowFriction;
        else if (_tileSettingsBounceDefaultToggle.isOn && _tileSettingsFrictionDefaultToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _normalBounceNormalFriction;
        else if (_tileSettingsBounceDefaultToggle.isOn && _tileSettingsFrictionHighToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _normalBounceHighFriction;
        else if (_tileSettingsBounceDefaultToggle.isOn && _tileSettingsFrictionMaxToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _normalBounceMaxFriction;
        else if (_tileSettingsBounceHighToggle.isOn && _tileSettingsFrictionNoneToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _highBounceNoFriction;
        else if (_tileSettingsBounceHighToggle.isOn && _tileSettingsFrictionLowToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _highBounceLowFriction;
        else if (_tileSettingsBounceHighToggle.isOn && _tileSettingsFrictionDefaultToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _highBounceNormalFriction;
        else if (_tileSettingsBounceHighToggle.isOn && _tileSettingsFrictionHighToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _highBounceHighFriction;
        else if (_tileSettingsBounceHighToggle.isOn && _tileSettingsFrictionMaxToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _highBounceMaxFriction;
        else if (_tileSettingsBounceMaxToggle.isOn && _tileSettingsFrictionNoneToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _maxBounceNoFriction;
        else if (_tileSettingsBounceMaxToggle.isOn && _tileSettingsFrictionLowToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _maxBounceLowFriction;
        else if (_tileSettingsBounceMaxToggle.isOn && _tileSettingsFrictionDefaultToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _maxBounceNormalFriction;
        else if (_tileSettingsBounceMaxToggle.isOn && _tileSettingsFrictionHighToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _maxBounceHighFriction;
        else if (_tileSettingsBounceMaxToggle.isOn && _tileSettingsFrictionMaxToggle.isOn)
            _preparationObjectBehaviour.ObjectPhysicMaterial = _maxBounceMaxFriction;
    }

    private void PhysicsSetupToggle()
    {
        if (_tileSettingsPhysicsToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectKinematic = false;
            _preparationObjectBehaviour.gameObject.layer =
                LayerMask.NameToLayer("Object Unstable");
            if (!_tileSettingsFrictionSectionRect.gameObject.activeSelf)
            {
                _tileSettingsFrictionSectionRect.gameObject.SetActive(true);
                RectUpdatePhysicsSettings(40f);
            }
            if (!_tileSettingsBounceSectionRect.gameObject.activeSelf)
            {
                _tileSettingsBounceSectionRect.gameObject.SetActive(true);
                RectUpdatePhysicsSettings(40f);
            }
        }
        else if (!_tileSettingsPhysicsToggle.isOn)
        {
            if (_tileSettingsGravityToggle.isOn)
            {
                _tileSettingsGravityToggle.isOn = false;
            }
            _preparationObjectBehaviour.ObjectKinematic = true;
            _preparationObjectBehaviour.gameObject.layer =
                LayerMask.NameToLayer("Object Stable");
            if (_tileSettingsFrictionSectionRect.gameObject.activeSelf)
            {
                _tileSettingsFrictionSectionRect.gameObject.SetActive(false);
                RectUpdatePhysicsSettings(-40f);
            }
            if (_tileSettingsBounceSectionRect.gameObject.activeSelf)
            {
                _tileSettingsBounceSectionRect.gameObject.SetActive(false);
                RectUpdatePhysicsSettings(-40f);
            }
        }
    }

    private void RectUpdatePhysicsSettings(float value)
    {
        _tileSettingsPhysicsSectionRect.sizeDelta = new Vector2(330f,
            _tileSettingsPhysicsSectionRect.sizeDelta.y + value);
    }

    private void GravitySetupToggle()
    {
        if (_tileSettingsGravityToggle.isOn)
        {
            if (!_tileSettingsPhysicsToggle.isOn)
            {
                _tileSettingsPhysicsToggle.isOn = true;
            }
            _preparationObjectBehaviour.ObjectGravity = true;
            if (!_tileSettingsMassSectionRect.gameObject.activeSelf)
            {
                _tileSettingsMassSectionRect.gameObject.SetActive(true);
                RectUpdatePhysicsSettings(40f);
            }
        }
        else if (!_tileSettingsGravityToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectGravity = false;

            if (_tileSettingsMassSectionRect.gameObject.activeSelf)
            {
                _tileSettingsMassSectionRect.gameObject.SetActive(false);
                RectUpdatePhysicsSettings(-40f);
            }
        }
    }

    private void MassSetupToggle()
    {
        if (_tileSettingsMassNoneToggle.isOn)
            _preparationObjectBehaviour.ObjectMass = 0f;
        else if (_tileSettingsMassLowToggle.isOn)
            _preparationObjectBehaviour.ObjectMass = 2.5f;
        else if (_tileSettingsMassDefaultToggle.isOn)
            _preparationObjectBehaviour.ObjectMass = 10f;
        else if (_tileSettingsMassHighToggle.isOn)
            _preparationObjectBehaviour.ObjectMass = 50f;
        else if (_tileSettingsMassMaxToggle.isOn)
            _preparationObjectBehaviour.ObjectMass = 500f;
    }

    private void MetalicSetupToggle()
    {
        if (_tileSettingsMetalicToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectType = ObjectType.metallic;
            //if (!_preparationObjectBehaviour.GetComponent<MetalicBehaviour>())
            //{
            //    MetalicBehaviour script = _preparationObjectBehaviour
            //        .gameObject.AddComponent<MetalicBehaviour>();
            //    script.Construct(_preparationObjectBehaviour);
            //}
            //if (_preparationObjectBehaviour.GetComponent<ObjectBehaviour>())
            //    Destroy(_preparationObjectBehaviour.GetComponent<ObjectBehaviour>());
        }
        else if (!_tileSettingsMetalicToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectType = ObjectType.none;
            //if (!_preparationObjectBehaviour.GetComponent<ObjectBehaviour>())
            //{
            //    ObjectBehaviour script = _preparationObjectBehaviour
            //        .gameObject.AddComponent<ObjectBehaviour>();
            //    script.Construct(_preparationObjectBehaviour);
            //}
            //if (_preparationObjectBehaviour.GetComponent<MetalicBehaviour>())
            //    Destroy(_preparationObjectBehaviour.GetComponent<MetalicBehaviour>());
            if (_tileSettingsMagneticToggle.isOn)
            {
                _tileSettingsMagneticToggle.isOn = false;
            }
        }
    }

    private void MagneticSetupToggle()
    {
        if (_tileSettingsMagneticToggle.isOn)
        {
            if (!_tileSettingsMetalicToggle.isOn)
            {
                _tileSettingsMetalicToggle.isOn = true;
            }

            _preparationObjectBehaviour.ObjectMagnet = true;

            if (!_tileSettingsMagneticStrengthRect.gameObject.activeSelf)
            {
                _tileSettingsMagneticStrengthRect.gameObject.SetActive(true);
                RectUpdateMagnetSettings(40f);
            }
            if (!_tileSettingsMagneticRangeRect.gameObject.activeSelf)
            {
                _tileSettingsMagneticRangeRect.gameObject.SetActive(true);
                RectUpdateMagnetSettings(40f);
            }

            //if (!_tileSettingsMagneticStrengthDefaultToggle.isOn)
            //    _tileSettingsMagneticStrengthDefaultToggle.isOn = true;
            //if (!_tileSettingsMagneticRangeDefaultToggle.isOn)
            //    _tileSettingsMagneticRangeDefaultToggle.isOn = true;
        }
        else if (!_tileSettingsMagneticToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnet = false;

            if (_tileSettingsMagneticStrengthRect.gameObject.activeSelf)
            {
                _tileSettingsMagneticStrengthRect.gameObject.SetActive(false);
                RectUpdateMagnetSettings(-40f);
            }
            if (_tileSettingsMagneticRangeRect.gameObject.activeSelf)
            {
                _tileSettingsMagneticRangeRect.gameObject.SetActive(false);
                RectUpdateMagnetSettings(-40f);
            }
        }
    }

    private void RectUpdateMagnetSettings(float value)
    {
        _tileSettingMetalicSectionRect.sizeDelta = new Vector2(330f,
            _tileSettingMetalicSectionRect.sizeDelta.y + value);
        _tileSettingsPhysicsSectionRect.sizeDelta = new Vector2(330f,
            _tileSettingsPhysicsSectionRect.sizeDelta.y + value);
    }

    private void MagneticStrengthSetupToggle()
    {
        if (_tileSettingsMagneticStrengthLowToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnetStrength = 15f;
            if (!_tileSettingsMagneticRangeDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticRangeDefaultToggle.isOn = true;
        }
        else if (_tileSettingsMagneticStrengthDefaultToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnetStrength = 30f;
            if (!_tileSettingsMagneticRangeDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticRangeDefaultToggle.isOn = true;
        }
        else if (_tileSettingsMagneticStrengthHighToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnetStrength = 50f;
            if (!_tileSettingsMagneticRangeDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticRangeDefaultToggle.isOn = true;
        }
        else if (_tileSettingsMagneticStrengthMaxToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnetStrength = 100f;
            if (!_tileSettingsMagneticRangeDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticRangeDefaultToggle.isOn = true;
        }
        else
        {
            _preparationObjectBehaviour.ObjectMagnetStrength = 0f;
            if (_tileSettingsMagneticRangeDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticRangeDefaultToggle.group
                    .GetFirstActiveToggle().isOn = false;
        }
    }

    private void MagneticRangeSetupToggle()
    {
        if (_tileSettingsMagneticRangeLowToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnetRange = 2f;
            if (!_tileSettingsMagneticStrengthDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticStrengthDefaultToggle.isOn = true;
        }
        else if (_tileSettingsMagneticRangeDefaultToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnetRange = 5f;
            if (!_tileSettingsMagneticStrengthDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticStrengthDefaultToggle.isOn = true;
        }
        else if (_tileSettingsMagneticRangeHighToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnetRange = 10f;
            if (!_tileSettingsMagneticStrengthDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticStrengthDefaultToggle.isOn = true;
        }
        else if (_tileSettingsMagneticRangeMaxToggle.isOn)
        {
            _preparationObjectBehaviour.ObjectMagnetRange = 20f;
            if (!_tileSettingsMagneticStrengthDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticStrengthDefaultToggle.isOn = true;
        }
        else
        {
            _preparationObjectBehaviour.ObjectMagnetRange = 0f;
            if (_tileSettingsMagneticStrengthDefaultToggle.group.AnyTogglesOn())
                _tileSettingsMagneticStrengthDefaultToggle.group
                    .GetFirstActiveToggle().isOn = false;
        }
    }

    private void RedColorToggle() => 
        _blockColor = _tileSettingsRedColorToggle.isOn ?
        BlockColors.red : BlockColors.none;
    private void OrangeColorToggle() =>
        _blockColor = _tileSettingsOrangeColorToggle.isOn ?
        BlockColors.orange : BlockColors.none;
    private void BrownColorToggle() =>
        _blockColor = _tileSettingsBrownColorToggle.isOn ?
        BlockColors.brown : BlockColors.none;
    private void YellowColorToggle() =>
        _blockColor = _tileSettingsYellowColorToggle.isOn ?
        BlockColors.yellow : BlockColors.none;
    private void LightGreyColorToggle() =>
        _blockColor = _tileSettingsLightGreyColorToggle.isOn ?
        BlockColors.lightGrey : BlockColors.none;
    private void GreenColorToggle() =>
        _blockColor = _tileSettingsGreenColorToggle.isOn ?
        BlockColors.green : BlockColors.none;
    private void BlueColorToggle() =>
        _blockColor = _tileSettingsBlueColorToggle.isOn ?
        BlockColors.blue : BlockColors.none;
    private void DarkGreyColorToggle() =>
        _blockColor = _tileSettingsDarkGreyColorToggle.isOn ?
        BlockColors.darkGrey : BlockColors.none;

    private void SetupOtherUIElements()
    {
        _automaticTilePlacingSpeedModifier = _coreAutoTilePlacingSpeedSlider;

        _sizeSlider = _tileSettingsSizeSlider;
        _sizeSlider.interactable = false;
        _sizeText = _tileSettingsSizeSliderText;
        _tileSettingsSizeSlider.onValueChanged.AddListener(
            delegate { SizeSlider(); });

        _rescaleXSlider = _tileSettingsScaleXSlider;
        _rescaleXSlider.interactable = false;
        _rescaleTextX = _tileSettingsScaleXSliderText;
        _tileSettingsScaleXSlider.onValueChanged.AddListener(
            delegate { TileRescaleXSlider(); });

        _rescaleYSlider = _tileSettingsScaleYSlider;
        _rescaleYSlider.interactable = false;
        _rescaleTextY = _tileSettingsScaleYSliderText;
        _tileSettingsScaleYSlider.onValueChanged.AddListener(
            delegate { TileRescaleYSlider(); });

        _rescaleZSlider = _tileSettingsScaleZSlider;
        _rescaleZSlider.interactable = false;
        _rescaleTextZ = _tileSettingsScaleZSliderText;
        _tileSettingsScaleZSlider.onValueChanged.AddListener(
            delegate { TileRescaleZSlider(); });
    }

    private void SizeSlider()
    {
        if (!_tileToPlace)
        {
            _tileSettingsSizeSlider.value = 2;
            return;
        }

        switch (_tileSettingsSizeSlider.value)
        {
            case 0: _gridBuilder.ReInitializeGrid(0.5f); break;
            case 1: _gridBuilder.ReInitializeGrid(0.75f); break;
            case 2: _gridBuilder.ReInitializeGrid(1f); break;
            case 3: _gridBuilder.ReInitializeGrid(1.5f); break;
            case 4: _gridBuilder.ReInitializeGrid(2f); break;
            case 5: _gridBuilder.ReInitializeGrid(3f); break;
            case 6: _gridBuilder.ReInitializeGrid(4f); break;
            case 7: _gridBuilder.ReInitializeGrid(6f); break;
            default: break;
        }

        var source = LocalizationSettings.StringDatabase.SmartFormatter
           .GetSourceExtension<PersistentVariablesSource>();

        ICollection<string> keys = source["globalUI"].Keys;

        using (PersistentVariablesSource.UpdateScope())
        {
            foreach (string key in keys)
            {
                if (key == "Size")
                {
                    StringVariable value = source["globalUI"][key] as StringVariable;
                    value.Value = _gridBuilder.GridTileSize.ToString();
                }
            }
        }

        //_sizeText.text = "Size: " + _gridBuilder.GridTileSize;
    }

    private void TileRescaleXSlider()
    {
        _tileRescaleXSliderValue = _tileSettingsScaleXSlider.value;
        _gridTilePlacer.UpdateMouseHolographicObjScale();
        //_rescaleTextX.text = "X Scale: " +
        //    _tileSettingsScaleXSlider.value.ToString("0.00");

        var source = LocalizationSettings.StringDatabase.SmartFormatter
            .GetSourceExtension<PersistentVariablesSource>();

        ICollection<string> keys = source["globalUI"].Keys;

        using (PersistentVariablesSource.UpdateScope())
        {
            foreach(string key in keys)
            {
                if (key == "XScale")
                {
                    StringVariable value = source["globalUI"][key] as StringVariable;
                    value.Value = _tileSettingsScaleXSlider.value.ToString("0.00");
                }
            }
        }
    }

    private void TileRescaleYSlider()
    {
        _tileRescaleYSliderValue = _tileSettingsScaleYSlider.value;
        _gridTilePlacer.UpdateMouseHolographicObjScale();
        //_rescaleTextY.text = "Y Scale: " +
        //    _tileSettingsScaleYSlider.value.ToString("0.00");

        var source = LocalizationSettings.StringDatabase.SmartFormatter
           .GetSourceExtension<PersistentVariablesSource>();

        ICollection<string> keys = source["globalUI"].Keys;

        using (PersistentVariablesSource.UpdateScope())
        {
            foreach (string key in keys)
            {
                if (key == "YScale")
                {
                    StringVariable value = source["globalUI"][key] as StringVariable;
                    value.Value = _tileSettingsScaleYSlider.value.ToString("0.00");
                }
            }
        }
    }

    private void TileRescaleZSlider()
    {
        _tileRescaleZSliderValue = _tileSettingsScaleZSlider.value;
        _gridTilePlacer.UpdateMouseHolographicObjScale();
        //_rescaleTextZ.text = "Z Scale: " +
        //    _tileSettingsScaleZSlider.value.ToString("0.00");

        var source = LocalizationSettings.StringDatabase.SmartFormatter
           .GetSourceExtension<PersistentVariablesSource>();

        ICollection<string> keys = source["globalUI"].Keys;

        using (PersistentVariablesSource.UpdateScope())
        {
            foreach (string key in keys)
            {
                if (key == "ZScale")
                {
                    StringVariable value = source["globalUI"][key] as StringVariable;
                    value.Value = _tileSettingsScaleZSlider.value.ToString("0.00");
                }
            }
        }
    }

    private void SetupUIToggleTextOnHover()
    {
        foreach (Toggle tileToggle in FindObjectsOfType<Toggle>(true))
        {
            //tileToggle.OnPointerEnter()
            // Setup the entire text hover situation..
        }
    }

    public void ToggleGameInfoPopupWindow()
    {
        if (_gameInfoPopupSection.activeSelf)
        {
            _gameInfoPopupSection.SetActive(false);
        }
        else if (!_gameInfoPopupSection.activeSelf)
        {
            _gameInfoPopupSection.SetActive(true);
            gameInfoUpdatePageCounter();
        }
    }

    public void GameInfoPopupNextFrame()
    {
        if (_gameInfoPopupPageCounter < _gameInfoPopupTotalPageCount)
        {
            _gameInfoPopupPageCounter++;
            GameInfoPopupPageButtonUpdate();
        }

        gameInfoUpdatePageCounter();
    }

    public void GameInfoPopupPreviousFrame()
    {
        if (_gameInfoPopupPageCounter > 1)
        {
            _gameInfoPopupPageCounter--;
            GameInfoPopupPageButtonUpdate();
        }

        gameInfoUpdatePageCounter();
    }

    private void GameInfoPopupTurnOffAllTexts()
    {
        _gameInfoPopupP1Text.gameObject.SetActive(false);
        _gameInfoPopupP2Text.gameObject.SetActive(false);
        _gameInfoPopupP3Text.gameObject.SetActive(false);
        _gameInfoPopupP4Text.gameObject.SetActive(false);
        _gameInfoPopupP5Text.gameObject.SetActive(false);
        _gameInfoPopupP6Text.gameObject.SetActive(false);
    }

    private void GameInfoPopupPageButtonUpdate()
    {
        GameInfoPopupTurnOffAllTexts();

        switch (_gameInfoPopupPageCounter)
        {
            case 1:
                _gameInfoPopupP1Text.gameObject.SetActive(true);
                _gameInfoPopupLeftButton.gameObject.SetActive(false);
                break;
            case 2:
                _gameInfoPopupP2Text.gameObject.SetActive(true);
                _gameInfoPopupLeftButton.gameObject.SetActive(true);
                break;
            case 3:
                _gameInfoPopupP3Text.gameObject.SetActive(true);
                break;
            case 4:
                _gameInfoPopupP4Text.gameObject.SetActive(true);
                break;
            case 5:
                _gameInfoPopupP5Text.gameObject.SetActive(true);
                _gameInfoPopupRightButton.gameObject.SetActive(true);
                break;
            case 6:
                _gameInfoPopupP6Text.gameObject.SetActive(true);
                _gameInfoPopupRightButton.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void KillAllObjects()
    {
        foreach (ObjectBehaviour tile in _gridTilePlacer.PlacedTilesObjectBehaviours.ToList())
        {
            if (tile != null)
            {
                tile.KillMe();
            }
        }
    }

    [SerializeField]
    private LocalizedString gameInfoPageCounterText;

    private void gameInfoUpdatePageCounter()
    {

        var source = LocalizationSettings.StringDatabase.SmartFormatter
      .GetSourceExtension<PersistentVariablesSource>();

        ICollection<string> keys = source["globalUI"].Keys;

        using (PersistentVariablesSource.UpdateScope())
        {
            foreach (string key in keys)
            {
                if (key == "PageCounter")
                {
                    IntVariable value = source["globalUI"][key] as IntVariable;
                    value.Value = _gameInfoPopupPageCounter;
                }
            }
        }
        //variablesGroupAsset.se
        //gameInfoPageCounterText.   

        //_gameInfoPopupPageCounterText.text = "Page " +
        //    (_gameInfoPopupPageCounter + 1) +
        //    "/" +
        //    _gameInfoPopupPageSprites.Length;
    }

    private void Update()
    {
        //print("strength " +_tileSettingsMagneticStrengthDefaultToggle.isOn + " range " + _tileSettingsMagneticRangeDefaultToggle.isOn);
    }
}
