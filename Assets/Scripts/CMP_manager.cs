using System.Collections.Generic;
using Unity.Usercentrics;
using UnityEngine;
using UnityEngine.UI;

public class CMP_manager : MonoBehaviour
{
    //[Header("")]
    [SerializeField] private Dropdown dropdown_init = null;

    [Header("Text fields")]
    [SerializeField] private Text text_controller = null;
    [SerializeField] private Text text_location = null;
    [SerializeField] private Text text_config = null;
    [SerializeField] private Text text_consent = null;
    [SerializeField] private Text text_uc_status_init = null;
    [SerializeField] private Text text_uc_status_styles = null;

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
    [SerializeField] private Button button_style_general = null;
    [SerializeField] private Button button_sl_styled = null;

    [Header("Toggles:")]
    [SerializeField] private Toggle toggle_back_button = null;
    [SerializeField] private Toggle toggle_mediation = null;
    [SerializeField] private Toggle toggle_getconsents = null;
    [SerializeField] private Toggle toggle_gettcfdata = null;
    [SerializeField] private Toggle toggle_getcmpdata = null;
    [SerializeField] private Toggle toggle_preview = null;
    [SerializeField] private Toggle toggle_styles = null;

    [Header("Inputs:")]
    [SerializeField] private InputField input_configuration = null;
    [SerializeField] private InputField input_language = null;
    [SerializeField] private InputField input_font_size = null;

    [Header("GOs:")]
    [SerializeField] private GameObject panel_settings_general = null;
    [SerializeField] private GameObject group_general = null;
    [SerializeField] private GameObject group_style_general = null;
    [HideInInspector] public ManagerUtilities utilities;

    //utils
    bool shouldCollectConsent = false;
    string tcf_preset = "WGSo-AvsCxM5d2";
    string gdpr_preset = "r4Tyqt9N1aLq_d";
    string ccpa_preset = "282E1MUwpv79wz";
    string ruleset_preset = "_d8jnCQKnUC5pV";
    GameObject go_utils;


    void Awake()
    {
        go_utils = new GameObject("utilities");
        utilities = go_utils.AddComponent<ManagerUtilities>();
        utilities.ref_manager = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        panel_settings_general.SetActive(false);
        button_init.onClick.AddListener(() => { CMP_Init(); });
        button_reset.onClick.AddListener(() => { CMP_Reset(); });

        button_showfirstlayer.onClick.AddListener(() => { ShowFirstLayer(); });
        button_showsecondlayer.onClick.AddListener(() => { ShowSecondLayer(); });

        //settings buttons
        button_general.onClick.AddListener(() => { ShowGeneralSettings(true); });
        button_style_general.onClick.AddListener(() => { ShowGeneralStyleSettings(true); });
        button_settings_back.onClick.AddListener(() => { HideAllSettings(); });

        button_fl_pc.onClick.AddListener(() => { showSpecificFirstLayer(UsercentricsLayout.PopupCenter); });
        button_fl_pb.onClick.AddListener(() => { showSpecificFirstLayer(UsercentricsLayout.PopupBottom); });
        button_fl_s.onClick.AddListener(() => { showSpecificFirstLayer(UsercentricsLayout.Sheet); });
        button_fl_f.onClick.AddListener(() => { showSpecificFirstLayer(UsercentricsLayout.Full); });

        //init toggles
        toggle_back_button.onValueChanged.AddListener((value) => { Usercentrics.Instance.Options.Android.DisableSystemBackButton = value; });
        toggle_mediation.onValueChanged.AddListener((value) => { Usercentrics.Instance.Options.ConsentMediation = value; });
        toggle_preview.onValueChanged.AddListener((value) => { DeterminePreview(value); });
        toggle_styles.onValueChanged.AddListener((value) => { DetermineStyles(value); });


        //input_configuration.text = Usercentrics.Instance.SettingsID;
        ChangeDropdown(0);
        input_configuration.onValueChanged.AddListener((value) => { SetConfig(); });
        input_language.onValueChanged.AddListener((value) => { Usercentrics.Instance.Options.DefaultLanguage = value; });
        dropdown_init.onValueChanged.AddListener((value) => { ChangeDropdown(value); });
    }



    private void ShowGeneralSettings(bool showing)
    {
        panel_settings_general.SetActive(showing);
        group_general.SetActive(showing);
        group_style_general.SetActive(!showing);
    }

    private void ShowGeneralStyleSettings(bool showing)
    {
        panel_settings_general.SetActive(showing);
        group_general.SetActive(!showing);
        group_style_general.SetActive(showing);
    }


    private void HideAllSettings()
    {
        panel_settings_general.SetActive(false);
        group_general.SetActive(false);
        group_style_general.SetActive(!false);
    }

    private void DeterminePreview(bool value)
    {
        if (value)
        {
            Usercentrics.Instance.Options.Version = "preview";
        }
        else
        {
            Usercentrics.Instance.Options.Version = "latest";
        }
    }

    private void DetermineStyles(bool value)
    {
        if (value)
        {
            text_uc_status_styles.text = "Using styles";
        }
        else
        {
            text_uc_status_styles.text = "Not using styles";
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
            text_uc_status_init.text = "UC init failed";
            Debug.Log("Init Error: " + errorMessage);
        });
    }

    private void CMP_Reset()
    {
        Usercentrics.Instance.Reset();

        text_consent.text = "n/a";
        text_config.text = "n/a";
        text_location.text = "n/a";
        text_controller.text = "n/a";
        text_uc_status_init.text = "Not initialised";
    }

    private void UpdateDisplayValues()
    {
        text_uc_status_init.text = "Initialised";
        text_consent.text = shouldCollectConsent.ToString();
        text_config.text = Usercentrics.Instance.SettingsID;
        text_location.text = Usercentrics.Instance.GetCmpData().userLocation.countryCode.ToString() + "/" + Usercentrics.Instance.GetCmpData().userLocation.regionCode.ToString();
        text_controller.text = Usercentrics.Instance.GetControllerId().ToString();
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
            List<UsercentricsServiceConsent> uc_consents = Usercentrics.Instance.GetConsents();
            foreach(var consent in uc_consents)
            {
                Debug.Log("Consent: " + consent.dataProcessor + " / " + consent.status.ToString());
            }
            
        }

        if (toggle_gettcfdata.isOn)
        {
            UsercentricsVariant activeVariant = Usercentrics.Instance.GetCmpData().activeVariant;
            if (activeVariant == UsercentricsVariant.TCF)
            {

                Debug.Log("GetTCFData:");
                Usercentrics.Instance.GetTCFData((tcfData) =>
                {
                    /*
                    var purposes = tcfData.purposes;
                    var specialPurposes = tcfData.specialPurposes;
                    var features = tcfData.features;
                    var specialFeatures = tcfData.specialFeatures;
                    var stacks = tcfData.stacks;
                    var vendors = tcfData.vendors;

                    */


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

    private void showSpecificFirstLayer(UsercentricsLayout layout)
    {
        BannerSettings bannerSettings;
        if (toggle_styles.isOn) //isOn = uses styles
        {
            var titleSettings = new TitleSettings(textSize: float.Parse(input_font_size.text), alignment: SectionAlignment.Center);

            var messageSettings = new MessageSettings(textSize: float.Parse(input_font_size.text), alignment: SectionAlignment.Start, textColor: "#170087", linkTextColor: "#5633ff", underlineLink: true);

            bannerSettings = new BannerSettings(generalStyleSettings: new GeneralStyleSettings(androidDisableSystemBackButton: true),
                                  firstLayerStyleSettings: new FirstLayerStyleSettings(layout: layout,
                                           title: titleSettings,
                                           message: messageSettings),
        secondLayerStyleSettings: new SecondLayerStyleSettings(showCloseButton: true),
                                  variantName: "");
        }
        else
        {
            bannerSettings = new BannerSettings();
        }


        Usercentrics.Instance.ShowFirstLayer(bannerSettings, usercentricsConsentUserResponse =>
        {
            UpdateServices(usercentricsConsentUserResponse.consents);
        });
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
        Usercentrics.Instance.ShowSecondLayer(new BannerSettings(), (usercentricsConsentUserResponse) =>
        {
            UpdateServices(usercentricsConsentUserResponse.consents);
            UpdateDisplayValues();
        });

    }


}
