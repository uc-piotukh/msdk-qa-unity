using System.Collections.Generic;
using Unity.Usercentrics;
using UnityEngine;
using UnityEngine.UI;

public class CMP_manager : MonoBehaviour
{
    //[Header("")]
    
    public UIBehaviour ui_ref;

    //utils
    string stored_cid = "";
    string tcf_preset = "WGSo-AvsCxM5d2";
    string gdpr_preset = "r4Tyqt9N1aLq_d";
    string ccpa_preset = "282E1MUwpv79wz";
    string ruleset_preset = "_d8jnCQKnUC5pV";
    [HideInInspector] public string sdk_version = "2.14.0";


    public void ChangeDropdown(string value)
    {
        Usercentrics.Instance.SettingsID = "";
        Usercentrics.Instance.RulesetID = "";

        switch (value)
        {
            //preset tcf
            case "TCF Preset":
                ui_ref.UpdateTextInputValue(tcf_preset);
                ui_ref.DisableTextInput(true);
                Usercentrics.Instance.SettingsID = tcf_preset;
                break;
            //preset gdpr
            case "GDPR Preset":
                ui_ref.UpdateTextInputValue(gdpr_preset);
                ui_ref.DisableTextInput(true);
                Usercentrics.Instance.SettingsID = gdpr_preset;
                break;
            //preset ccpa
            case "CCPA Preset":
                ui_ref.UpdateTextInputValue(ccpa_preset);
                ui_ref.DisableTextInput(true);
                Usercentrics.Instance.SettingsID = ccpa_preset;
                break;
            //preset ruleset
            case "Ruleset Preset":
                ui_ref.UpdateTextInputValue(ruleset_preset);
                ui_ref.DisableTextInput(true);
                Usercentrics.Instance.RulesetID = ruleset_preset;
                break;
            //own sid
            case "Own Config":
                ui_ref.DisableTextInput(false);
                ui_ref.UpdateTextInputValue("");
                break;
            //own ruleset
            case "Own Ruleset":
                ui_ref.DisableTextInput(false);
                ui_ref.UpdateTextInputValue("");
                break;

            default:
                ui_ref.UpdateTextInputValue(tcf_preset);
                ui_ref.DisableTextInput(true);
                Usercentrics.Instance.SettingsID = tcf_preset;
                break;
        }
    }

    
    public void SetConfig(string dropdown_value, string input_value)
    {
        Usercentrics.Instance.SettingsID = "";
        Usercentrics.Instance.RulesetID = "";



        switch (dropdown_value)
        {

            //own sid
            case "Own Config":
                Usercentrics.Instance.SettingsID = input_value;
                break;
            //own ruleset
            case "Own Ruleset":
                Usercentrics.Instance.RulesetID = input_value;
                break;
        }

    }
    

    public void CMP_Init()
    {
        Usercentrics.Instance.Initialize((usercentricsReadyStatus) =>
        {
            UpdateDisplayValues(usercentricsReadyStatus);
            OutputLogsOnInit(usercentricsReadyStatus);
            
            if (usercentricsReadyStatus.shouldCollectConsent)
            {
                ShowFirstLayer();
            }
            else
            {
                UpdateServices(usercentricsReadyStatus.consents);
            }
        },
        (errorMessage) =>
        {
            ui_ref.label_initstatus.text = "UC init failed";
            Debug.Log("Init Error: " + errorMessage);
        });
    }

    public void CMP_ClearCache()
    {
        Usercentrics.Instance.ClearUserSession((usercentricsReadyStatus) =>
        {
            UpdateDisplayValues(usercentricsReadyStatus);
            Debug.Log("Session Cleared Successfully!");

            if (usercentricsReadyStatus.shouldCollectConsent)
            {
                ShowFirstLayer();
            }
            else
            {
                UpdateServices(usercentricsReadyStatus.consents);
            }
        },
        (errorMessage) =>
        {
            Debug.Log("Session Restore Error: " + errorMessage);
        });
    }

    private void UpdateDisplayValues(UsercentricsReadyStatus status)
    {
        ui_ref.label_initstatus.text = "Initialised";
        ui_ref.label_bannerrequired.text = status.geolocationRuleset.bannerRequiredAtLocation.ToString();
        ui_ref.label_activeconfig.text = status.geolocationRuleset.activeSettingsId;
        ui_ref.label_location.text = Usercentrics.Instance.GetCmpData().userLocation.countryCode.ToString() + "/" + Usercentrics.Instance.GetCmpData().userLocation.regionCode.ToString();
        ui_ref.label_cid.text = Usercentrics.Instance.GetControllerId();
    }

    public void showSpecificFirstLayer(UsercentricsLayout layout)
    {
        Dictionary<string, string> customBannerSettings = ui_ref.SetBannerSettings();

        //TODO put banner settings back in with the styling menu
        //general style
        float textSize = float.Parse(customBannerSettings["textSize"]);
        string textColor = customBannerSettings["textColor"];
        string linkColor = customBannerSettings["linkColor"];
        string layerBackgroundColor = customBannerSettings["layerBackgroundColor"];
        //bool fullscreen = ui_ref.toggle_fullscreen.value;

        BannerSettings bannerSettings;
      
        var titleSettings = new TitleSettings(textSize: textSize, alignment: SectionAlignment.Center);

        var messageSettings = new MessageSettings(textSize: textSize, alignment: SectionAlignment.Start, textColor: textColor, linkTextColor: linkColor, underlineLink: true);

        bannerSettings = new BannerSettings(generalStyleSettings: new GeneralStyleSettings(
                androidDisableSystemBackButton: Usercentrics.Instance.Options.Android.DisableSystemBackButton,
                layerBackgroundColor: layerBackgroundColor
                ),
                                  firstLayerStyleSettings: new FirstLayerStyleSettings(layout: layout,
                                           title: titleSettings,
                                           message: messageSettings
                                           ),
        secondLayerStyleSettings: new SecondLayerStyleSettings(showCloseButton: true),
                                 variantName: "");
        
        
        Usercentrics.Instance.ShowFirstLayer(bannerSettings, usercentricsConsentUserResponse =>
        {
            UpdateServices(usercentricsConsentUserResponse.consents);
            displayUserResponseValues(usercentricsConsentUserResponse);
        });
    }

    public void ShowFirstLayer()
    {
        Usercentrics.Instance.ShowFirstLayer((usercentricsConsentUserResponse) =>
        {
            UpdateServices(usercentricsConsentUserResponse.consents);
            displayUserResponseValues(usercentricsConsentUserResponse);
        });

    }

    public void ShowSecondLayer()
    {
        Usercentrics.Instance.ShowSecondLayer(new BannerSettings(), (usercentricsConsentUserResponse) =>
        {
            UpdateServices(usercentricsConsentUserResponse.consents);
            displayUserResponseValues(usercentricsConsentUserResponse);
        });
    }

    private void UpdateServices(List<UsercentricsServiceConsent> consents)
    {
        //logging toggles
        if (ui_ref.toggle_getconsents.value)
        {
            Debug.Log("GetConsents:");
            List<UsercentricsServiceConsent> uc_consents = Usercentrics.Instance.GetConsents();
            foreach (var consent in uc_consents)
            {
                Debug.Log("Consent: " + consent.dataProcessor + " / " + consent.status.ToString());
            }
        }

        if (ui_ref.toggle_gettcfdata.value)
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
        if (ui_ref.toggle_getcmpdata.value)
        {
            Debug.Log("GetCMPData:");
            Debug.Log(Usercentrics.Instance.GetCmpData());
        }
    }

    private void OutputLogsOnInit(UsercentricsReadyStatus status) {
        if (ui_ref.toggle_getconsents_init.value)
        {
            Debug.Log("GetConsents:");
            List<UsercentricsServiceConsent> uc_consents = Usercentrics.Instance.GetConsents();
            foreach (var consent in uc_consents)
            {
                Debug.Log("Consent: " + consent.dataProcessor + " / " + consent.status.ToString());
            }

        }

        if (ui_ref.toggle_getcmpdata_init.value)
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
        if (ui_ref.toggle_getcmpdata_init.value)
        {
            Debug.Log("GetCMPData:");
            Debug.Log(Usercentrics.Instance.GetCmpData());
        }
        if (ui_ref.toggle_geo_banner_init.value)
        {
            Debug.Log("status.geolocationRuleset.bannerRequiredAtLocation:");
            Debug.Log(status.geolocationRuleset.bannerRequiredAtLocation.ToString());
        }
        if (ui_ref.toggle_geo_config_init.value)
        {
            Debug.Log("status.geolocationRuleset.activeSettingsId:");
            Debug.Log(status.geolocationRuleset.activeSettingsId);
        }
    }

    public void StoreController()
    {
        stored_cid = Usercentrics.Instance.GetControllerId();
        ui_ref.label_storedcid.text = stored_cid;
    }

    public void RestoreUserSession()
    {
        RestoreSession(stored_cid);
    }

    private void RestoreSession(string cid) {
        if (cid == "")
        {
            Debug.Log("CID empty, please first set CID using the -Store Controller ID- button, this will take current CID");
            return;
        }

        Usercentrics.Instance.RestoreUserSession(cid, (status => {

            UpdateDisplayValues(status);

            if (status.shouldCollectConsent)
            {
                Debug.Log("status.shouldCollectConsent: "+ status.shouldCollectConsent.ToString());
            }
            else
            {
                Debug.Log("status.shouldCollectConsent: " + status.shouldCollectConsent.ToString());
            }
        }), (errorString => {
            Debug.LogError("Session Restore Error: " + errorString);
        }));
    }

    private void displayUserResponseValues(UsercentricsConsentUserResponse response)
    {
        if (ui_ref.toggle_cid.value)
        {
            Debug.Log("Controller ID from User Response: "+response.controllerId);
        }
    }
}
