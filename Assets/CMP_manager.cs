using System.Collections;
using System.Collections.Generic;
using Unity.Usercentrics;
using UnityEngine;
using UnityEngine.UI;

public class CMP_manager : MonoBehaviour
{
   
    [SerializeField] private InputField input_configuration = null;
    [SerializeField] private Dropdown dropdown_init = null;

    //debug texts:
    [SerializeField] private Text text_controller = null;
    [SerializeField] private Text text_location = null;
    [SerializeField] private Text text_config = null;
    [SerializeField] private Text text_consent = null;



    [SerializeField] private Button button_apply = null;
    [SerializeField] private Button button_init = null;
    [SerializeField] private Button button_reset = null;
    [SerializeField] private Button button_showfirstlayer = null;
    [SerializeField] private Button button_showsecondlayer = null;
    [SerializeField] private Button button_settings_back = null;
    [SerializeField] private Button button_general = null;

    [SerializeField] private Toggle toggle_back_button = null;
    [SerializeField] private GameObject panel_settings_general = null;



    //utils
    bool shouldCollectConsent = false;


    // Start is called before the first frame update
    void Start()
    {
        panel_settings_general.SetActive(false);
        button_apply.onClick.AddListener(() => { SetConfig(); });
        button_init.onClick.AddListener(() => { CMP_Init(); });
        button_reset.onClick.AddListener(() => { CMP_Reset(); });

        button_showfirstlayer.onClick.AddListener(() => { ShowFirstLayer(); });
        button_showsecondlayer.onClick.AddListener(() => { ShowSecondLayer(); });
        button_general.onClick.AddListener(() => {
            panel_settings_general.SetActive(true);
        });
        button_settings_back.onClick.AddListener(() => {
            panel_settings_general.SetActive(false);
        });

        toggle_back_button.onValueChanged.AddListener((value) => { Usercentrics.Instance.Options.Android.DisableSystemBackButton = value; });
        input_configuration.text = Usercentrics.Instance.SettingsID;
    }

    private void SetConfig()
    {
        Usercentrics.Instance.SettingsID = "";
        Usercentrics.Instance.RulesetID = "";        


        if (input_configuration.text == "")
        {
            Debug.Log("Please enter a configuration");
        }
        else
        {
            switch (dropdown_init.value)
            {
                case 0:
                    Usercentrics.Instance.SettingsID = input_configuration.text;
                    break;

                case 1:
                    Usercentrics.Instance.RulesetID = input_configuration.text;
                    break;

                default:
                    Usercentrics.Instance.SettingsID = input_configuration.text;
                    break;
            }
        }

        //UC init

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
            // Log and collect the error...
        });
    }

    private void CMP_Reset()
    {
        Usercentrics.Instance.Reset();

        text_consent.text = "n/a";
        text_config.text = "n/a";
        text_location.text = "n/a";
        text_controller.text = "n/a";

    }


    private void UpdateDisplayValues()
    {
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
    }


    private void ShowSettings()
    {

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