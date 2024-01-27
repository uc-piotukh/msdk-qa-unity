using System.Collections.Generic;
using Unity.Usercentrics;
using UnityEngine;
using UnityEngine.UI;

public class CMP_manager : MonoBehaviour
{
    //[Header("")]
    [SerializeField] private InputField input_configuration = null;
    [SerializeField] private Dropdown dropdown_init = null;

    [Header("Text fields")]
    [SerializeField] private Text text_controller = null;
    [SerializeField] private Text text_location = null;
    [SerializeField] private Text text_config = null;
    [SerializeField] private Text text_consent = null;
    [SerializeField] private Text text_uc_status = null;

    [Header("Buttons:")]
    [SerializeField] private Button button_init = null;
    [SerializeField] private Button button_reset = null;
    [SerializeField] private Button button_showfirstlayer = null;
    [SerializeField] private Button button_fl_pc = null;
    [SerializeField] private Button button_fl_pb = null;
    [SerializeField] private Button button_fl_s = null;
    [SerializeField] private Button button_fl_f = null;
    [SerializeField] private Button button_showsecondlayer = null;
    [SerializeField] private Button button_settings_back = null;
    [SerializeField] private Button button_general = null;

    [Header("Toggles:")]
    [SerializeField] private Toggle toggle_back_button = null;
    [SerializeField] private Toggle toggle_mediation = null;
    [SerializeField] private Toggle toggle_getconsents = null;
    [SerializeField] private Toggle toggle_gettcfdata = null;
    [SerializeField] private Toggle toggle_getcmpdata = null;
    [SerializeField] private Toggle toggle_preview = null;

    [Header("GOs:")]
    [SerializeField] private GameObject panel_settings_general = null;

    //utils
    bool shouldCollectConsent = false;
    string tcf_preset = "WGSo-AvsCxM5d2";
    string gdpr_preset = "r4Tyqt9N1aLq_d";
    string ccpa_preset = "282E1MUwpv79wz";
    string ruleset_preset = "_d8jnCQKnUC5pV";


    // Start is called before the first frame update
    void Start()
    {
        panel_settings_general.SetActive(false);
        button_init.onClick.AddListener(() => { CMP_Init(); });
        button_reset.onClick.AddListener(() => { CMP_Reset(); });

        button_showfirstlayer.onClick.AddListener(() => { ShowFirstLayer(); });
        button_showsecondlayer.onClick.AddListener(() => { ShowSecondLayer(); });
        button_general.onClick.AddListener(() => {panel_settings_general.SetActive(true);});
        button_settings_back.onClick.AddListener(() => { panel_settings_general.SetActive(false); });

        button_fl_pc.onClick.AddListener(() => { showSpecificFirstLayer(UsercentricsLayout.PopupCenter); });
        button_fl_pb.onClick.AddListener(() => { showSpecificFirstLayer(UsercentricsLayout.PopupBottom); });
        button_fl_s.onClick.AddListener(() => { showSpecificFirstLayer(UsercentricsLayout.Sheet); });
        button_fl_f.onClick.AddListener(() => { showSpecificFirstLayer(UsercentricsLayout.Full); });



        //init toggles
        toggle_back_button.onValueChanged.AddListener((value) => { Usercentrics.Instance.Options.Android.DisableSystemBackButton = value; });
        toggle_mediation.onValueChanged.AddListener((value) => { Usercentrics.Instance.Options.ConsentMediation = value; });
        toggle_preview.onValueChanged.AddListener((value) => { DeterminePreview(value); });

        //input_configuration.text = Usercentrics.Instance.SettingsID;
        ChangeDropdown(0);
        input_configuration.onValueChanged.AddListener((value) => { SetConfig(); });
        dropdown_init.onValueChanged.AddListener((value) => { ChangeDropdown(value); });
    }

    private void DeterminePreview(bool value)
    {
        if (value)
        {
            Usercentrics.Instance.Options.Version = "preview";
        } else
        {
            Usercentrics.Instance.Options.Version = "latest";
        }
    }

    private void ChangeDropdown(int value)
    {
        Usercentrics.Instance.SettingsID = "";
        Usercentrics.Instance.RulesetID = "";

              switch (value)
        {
            //preset tcf
            case 0:
                input_configuration.text = tcf_preset;
                Usercentrics.Instance.SettingsID = input_configuration.text;
                break;
            //preset gdpr
            case 1:
                input_configuration.text = gdpr_preset;

                Usercentrics.Instance.SettingsID = input_configuration.text;
                break;
            //preset ccpa
            case 2:
                input_configuration.text = ccpa_preset;
                Usercentrics.Instance.SettingsID = input_configuration.text;
                break;
            //preset ruleset
            case 3:
                input_configuration.text = ruleset_preset;
                Usercentrics.Instance.RulesetID = input_configuration.text;
                break;
            //own sid
            case 4:
                input_configuration.text = "";
                break;
            //own ruleset
            case 5:
                input_configuration.text = "";
                break;

            default:
                input_configuration.text = tcf_preset;
                Usercentrics.Instance.SettingsID = input_configuration.text;
                break;
        }
    }

    private void SetConfig()
    {
        Usercentrics.Instance.SettingsID = "";
        Usercentrics.Instance.RulesetID = "";        


        
            switch (dropdown_init.value)
            {
                
                //own sid
                case 4:
                    Usercentrics.Instance.SettingsID = input_configuration.text;
                    break;
                //own ruleset
                case 5:
                    Usercentrics.Instance.RulesetID = input_configuration.text;
                    break;
            }
        
    }

    private void CMP_Init()
    {
        Usercentrics.Instance.Initialize((usercentricsReadyStatus) =>
        {
            UpdateDisplayValues();
            if (usercentricsReadyStatus.shouldCollectConsent)
            {
                shouldCollectConsent = usercentricsReadyStatus.shouldCollectConsent;
                ShowFirstLayer();
            }
            else
            {
                UpdateServices(usercentricsReadyStatus.consents);
            }
        },
        (errorMessage) =>
        {
            text_uc_status.text = "UC init failed";
        });
    }

    private void CMP_Reset()
    {
        Usercentrics.Instance.Reset();

        text_consent.text = "n/a";
        text_config.text = "n/a";
        text_location.text = "n/a";
        text_controller.text = "n/a";
        text_uc_status.text = "Not initialised";
    }

    private void UpdateDisplayValues()
    {
        text_uc_status.text = "Initialised";
        text_consent.text = shouldCollectConsent.ToString();
        text_config.text = Usercentrics.Instance.SettingsID;
        text_location.text = Usercentrics.Instance.GetCmpData().userLocation.countryCode.ToString()+"/"+ Usercentrics.Instance.GetCmpData().userLocation.regionCode.ToString();
        text_controller.text = Usercentrics.Instance.GetControllerId().ToString();
    }

    private void ShowFirstLayer()
    {
        Usercentrics.Instance.ShowFirstLayer((usercentricsConsentUserResponse) =>
        {
            UpdateServices(usercentricsConsentUserResponse.consents);
        });

    }

    private void ShowSecondLayer()
    {
        Usercentrics.Instance.ShowSecondLayer(GetBannerSettings(), (usercentricsConsentUserResponse) =>
        {
            UpdateServices(usercentricsConsentUserResponse.consents);
            UpdateDisplayValues();
        });
    }

    private void UpdateServices(List<UsercentricsServiceConsent> consents)
    {
        //update consents
        shouldCollectConsent = false;

        //@TODO refactor & log better needed, need a utils script
        //logging toggles

        if (toggle_getconsents.isOn)
        {
            Debug.Log("GetConsents:");
            Debug.Log(Usercentrics.Instance.GetConsents());
        }

        if (toggle_gettcfdata.isOn)
        {
            UsercentricsVariant activeVariant = Usercentrics.Instance.GetCmpData().activeVariant;
            if (activeVariant == UsercentricsVariant.TCF)
            {

                Debug.Log("GetTCFData:");
                Usercentrics.Instance.GetTCFData((tcfData) =>
                {
                    var purposes = tcfData.purposes;
                    var specialPurposes = tcfData.specialPurposes;
                    var features = tcfData.features;
                    var specialFeatures = tcfData.specialFeatures;
                    var stacks = tcfData.stacks;
                    var vendors = tcfData.vendors;

                    // TCString
                    
                    var tcString = tcfData.tcString;
                    Debug.Log(tcString);
                });
            }
            else
            {
                Debug.Log("GetTCFData is enabled, but active variant is " + activeVariant.ToString());
            }
        }
        if (toggle_getcmpdata.isOn)
        {
            Debug.Log("GetCMPData:");
            Debug.Log(Usercentrics.Instance.GetCmpData());
        }
    }
    //@TODO refactor
    private BannerSettings MakeBannerSettings(UsercentricsLayout layout)
    {
        GeneralStyleSettings general = new GeneralStyleSettings();
        FirstLayerStyleSettings firstLayer =  new FirstLayerStyleSettings(layout: layout);
        return new BannerSettings(generalStyleSettings: general, firstLayerStyleSettings: firstLayer);
    }

    private void showSpecificFirstLayer(UsercentricsLayout layout)
    {
        Usercentrics.Instance.ShowFirstLayer(MakeBannerSettings(layout), usercentricsConsentUserResponse =>
        {
            UpdateServices(usercentricsConsentUserResponse.consents);
        });
    }


    //---------------------BANNER SETTINGS CRAP WE DONT TOUCH THAT YET---------------------------

    private BannerSettings GetBannerSettings()
    {
        return new BannerSettings(generalStyleSettings: GetGeneralStyleSettings(),
                                  firstLayerStyleSettings: GetFirstLayerStyleSettings(),
                                  secondLayerStyleSettings: new SecondLayerStyleSettings(showCloseButton: true),
                                  variantName: "");
    }

    private GeneralStyleSettings GetGeneralStyleSettings()
    {
        return new GeneralStyleSettings(androidDisableSystemBackButton: true,
                                        androidStatusBarColor: "#f51d7e");
    }

    private FirstLayerStyleSettings GetFirstLayerStyleSettings()
    {
        var headerImageSettings = HeaderImageSettings.Extended(imageUrl: "https://drive.google.com/uc?export=download&id=1eGO0eowmuc1oLB75ZNktunHnVcZQVBUN");

        var buttons = new List<ButtonSettings> {
                new ButtonSettings(type: ButtonType.AcceptAll, textSize: 13f, textColor: "#ffffff", backgroundColor: "#350aab", cornerRadius: 12, isAllCaps: true),
                new ButtonSettings(type: ButtonType.More, textSize: 13f, textColor: "#ffffff", backgroundColor: "#350aab", cornerRadius: 12, isAllCaps: true)
            };
        var buttonLayout = ButtonLayout.Row(buttons);

        var titleSettings = new TitleSettings(textSize: 20f, alignment: SectionAlignment.Center, textColor: "#ffffff");

        var messageSettings = new MessageSettings(textSize: 12f, alignment: SectionAlignment.Start, textColor: "#bababa", linkTextColor: "#f51d7e", underlineLink: true);

        return new FirstLayerStyleSettings(layout: UsercentricsLayout.PopupCenter,
                                           headerImage: headerImageSettings,
                                           title: titleSettings,
                                           message: messageSettings,
                                           buttonLayout: buttonLayout,
                                           backgroundColor: "#000000",
                                           cornerRadius: 30f,
                                           overlayColor: "#350aab",
                                           overlayAlpha: 0.5f);
    }

    private ButtonLayout GridButtonLayoutExample()
    {
        var buttons = new List<ButtonSettingsRow> {
                new ButtonSettingsRow(
                    new List<ButtonSettings> {
                        new ButtonSettings(type: ButtonType.AcceptAll, textSize: 13f, textColor: "#ffffff", backgroundColor: "#350aab", cornerRadius: 12f, isAllCaps: true),
                        new ButtonSettings(type: ButtonType.Save, textSize: 13f, textColor: "#ffffff", backgroundColor: "#350aab", cornerRadius: 12f, isAllCaps: true)
                    }
                ),
                new ButtonSettingsRow(
                    new List<ButtonSettings> {
                        new ButtonSettings(type: ButtonType.More, textSize: 13f, textColor: "#ffffff", backgroundColor: "#350aab", cornerRadius: 12f, isAllCaps: true)
                    }
                )
            };
        return ButtonLayout.Grid(buttons);
    }

    private ButtonLayout ColumnButtonLayoutExample()
    {
        var buttons = new List<ButtonSettings> {
                new ButtonSettings(type: ButtonType.AcceptAll, textSize: 13f, textColor: "#ffffff", backgroundColor: "#350aab", cornerRadius: 12, isAllCaps: true),
                new ButtonSettings(type: ButtonType.More, textSize: 13f, textColor: "#ffffff", backgroundColor: "#350aab", cornerRadius: 12, isAllCaps: true)
            };
        return ButtonLayout.Column(buttons);
    }

}