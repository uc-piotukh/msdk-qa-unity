using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Usercentrics;

public class UIBehaviour : MonoBehaviour
{


    //INTRODUCING UI TOOLKIT. GOD HELP ME.

    //ve refs
    VisualElement root;
    VisualElement element_ui_main;
    VisualElement element_ui_settings;
    VisualElement element_ui_style;


    //button refs
    Button button_init;
    Button button_reset;
    Button button_fl_remote;
    Button button_sl_remote;
    Button button_fl_pc;
    Button button_fl_pb;
    Button button_fl_s;
    Button button_fl_f;
    Button button_sl_style;
    Button button_settings;
    Button button_styling;
    Button button_store_cid;
    Button button_restore_session;
    Button button_settings_back;
    Button button_style_settings_back;

    //toggle refs
    Toggle toggle_androidbackbutton;
    Toggle toggle_consentmediation;
    Toggle toggle_preview;
    [HideInInspector] public Toggle toggle_limitpurposes;
    [HideInInspector] public Toggle toggle_getconsents_init;
    [HideInInspector] public Toggle toggle_getcmpdata_init;
    [HideInInspector] public Toggle toggle_gettcfdata_init;
    [HideInInspector] public Toggle toggle_cid_init;
    [HideInInspector] public Toggle toggle_getcmpdata;
    [HideInInspector] public Toggle toggle_gettcfdata;
    [HideInInspector] public Toggle toggle_cid;
    [HideInInspector] public Toggle toggle_getconsents;
    [HideInInspector] public Toggle toggle_geo_banner_init;
    [HideInInspector] public Toggle toggle_geo_config_init;

    //label refs
    [HideInInspector] public Label label_initstatus;
    [HideInInspector] public Label label_styling;
    [HideInInspector] public Label label_location;
    [HideInInspector] public Label label_activeconfig;
    [HideInInspector] public Label label_bannerrequired;
    [HideInInspector] public Label label_cid;
    [HideInInspector] public Label label_storedcid;


    //text fields
    TextField textfield_config;
    [HideInInspector] public TextField textfield_textcolor;
    [HideInInspector] public TextField textfield_layerbackgroundcolor;
    [HideInInspector] public TextField textfield_linkcolor;
    [HideInInspector] public TextField textfield_borderscolor;

    DropdownField dropdown_config;

    SliderInt sliderint_fontsize;

    //utils
    public GameObject go_uc;
    CMP_manager manager;
    Dictionary<string, string> bannerSettings = new Dictionary<string, string>();


    //STRINGS
    string ve_ui_main = "ve_ui_main";
    string df_configuration = "df_configuration";
    string tf_customconfig = "tf_customconfig";
    string b_init = "b_init";
    string b_reset = "b_reset";
    string l_initstatus = "l_initstatus";
    string l_styling = "l_styling";
    string b_fl_remote = "b_fl_remote";
    string b_sl_remote = "b_sl_remote";
    string b_fl_pc = "b_fl_pc";
    string b_fl_pb = "b_fl_pb";
    string b_fl_s = "b_fl_s";
    string b_fl_f = "b_fl_f";
    string b_sl_style = "b_sl_style";
    string l_location = "l_location";
    string l_activeconfig = "l_activeconfig";
    string l_bannerrequired = "l_bannerrequired";
    string b_settings = "b_settings";
    string b_styling = "b_styling";
    string b_store_cid = "b_store_cid";
    string b_restore_session = "b_restore_session";
    string l_cid = "l_cid";

    string ve_ui_settings = "ve_ui_settings";
    string t_androidbackbutton = "t_androidbackbutton";
    string t_consentmediation = "t_consentmediation";
    string t_preview = "t_preview";
    string t_limitpurposes = "t_limitpurposes";
    string t_getconsents_init = "t_getconsents_init";
    string t_getcmpdata_init = "t_getcmpdata_init";
    string t_gettcfdata_init = "t_gettcfdata_init";
    string t_cid_init = "t_cid_init";
    string t_getcmpdata = "t_getcmpdata";
    string t_gettcfdata = "t_gettcfdata";
    string t_cid = "t_cid";
    string t_getconsents = "t_getconsents";
    string b_settings_back = "b_settings_back";
    string t_geo_banner_init = "t_geo_banner_init";
    string t_geo_config_init = "t_geo_config_init";

    string ve_ui_style = "ve_ui_style";
    string b_style_back = "b_style_back";
    string si_fontsize = "si_fontsize";
    string tf_textColor = "tf_textColor";
    string tf_layerBackgroundColor = "tf_layerBackgroundColor";
    string tf_linkColor = "tf_linkColor";
    string tf_bordersColor = "tf_bordersColor";
    string l_storedcid = "l_storedcid";

    /*PLACEHOLDER BLOCK
     
    string ve_ui_settings = "";
    string ve_ui_settings = "";
    string ve_ui_settings = "";
    */

    private void OnEnable()
    {
        PrepareReferencing();
        PrepareListeners();
        SetValues();
    }

    private void Awake()
    {
        manager = go_uc.GetComponent<CMP_manager>();
    }

    private void PrepareReferencing()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        element_ui_main = root.Q(ve_ui_main);
        element_ui_settings = root.Q(ve_ui_settings);
        element_ui_style = root.Q(ve_ui_style);

        //button refs
        button_init = root.Q<Button>(b_init);
        button_reset = root.Q<Button>(b_reset);
        button_fl_remote = root.Q<Button>(b_fl_remote);
        button_sl_remote = root.Q<Button>(b_sl_remote);
        button_fl_pc = root.Q<Button>(b_fl_pc);
        button_fl_pb = root.Q<Button>(b_fl_pb);
        button_fl_s = root.Q<Button>(b_fl_s);
        button_fl_f = root.Q<Button>(b_fl_f);
        button_sl_style = root.Q<Button>(b_sl_style);
        button_settings = root.Q<Button>(b_settings);
        button_styling = root.Q<Button>(b_styling);
        button_store_cid = root.Q<Button>(b_store_cid);
        button_restore_session = root.Q<Button>(b_restore_session);
        button_settings_back = root.Q<Button>(b_settings_back);
        button_style_settings_back = root.Q<Button>(b_style_back);

        //toggle refs
        toggle_androidbackbutton = root.Q<Toggle>(t_androidbackbutton);
        toggle_consentmediation = root.Q<Toggle>(t_consentmediation);
        toggle_preview = root.Q<Toggle>(t_preview);
        toggle_limitpurposes = root.Q<Toggle>(t_limitpurposes);
        toggle_getconsents_init = root.Q<Toggle>(t_getconsents_init);
        toggle_getcmpdata_init = root.Q<Toggle>(t_getcmpdata_init);
        toggle_gettcfdata_init = root.Q<Toggle>(t_gettcfdata_init);
        toggle_cid_init = root.Q<Toggle>(t_cid_init);
        toggle_getcmpdata = root.Q<Toggle>(t_getcmpdata);
        toggle_gettcfdata = root.Q<Toggle>(t_gettcfdata);
        toggle_cid = root.Q<Toggle>(t_cid);
        toggle_getconsents = root.Q<Toggle>(t_getconsents);
        toggle_geo_banner_init = root.Q<Toggle>(t_geo_banner_init);
        toggle_geo_config_init = root.Q<Toggle>(t_geo_config_init);

        //label refs
        label_initstatus = root.Q<Label>(l_initstatus);
        label_styling = root.Q<Label>(l_styling);
        label_location = root.Q<Label>(l_location);
        label_activeconfig = root.Q<Label>(l_activeconfig);
        label_bannerrequired = root.Q<Label>(l_bannerrequired);
        label_cid = root.Q<Label>(l_cid);
        label_storedcid = root.Q<Label>(l_storedcid);

        //text fields
        textfield_config = root.Q<TextField>(tf_customconfig);
        textfield_textcolor = root.Q<TextField>(tf_textColor);
        textfield_layerbackgroundcolor = root.Q<TextField>(tf_layerBackgroundColor);
        textfield_linkcolor = root.Q<TextField>(tf_linkColor);
        textfield_borderscolor = root.Q<TextField>(tf_bordersColor);

        dropdown_config = root.Q<DropdownField>(df_configuration);
        sliderint_fontsize = root.Q<SliderInt>(si_fontsize);
    }

    private void PrepareListeners()
    {
        button_init.clicked += InitialiseUC;
        button_reset.clicked += ResetUC;
        button_fl_remote.clicked += OpenFLRC;
        button_sl_remote.clicked += OpenSLR;
        button_fl_pc.clicked += OpenFLPC;
        button_fl_pb.clicked += OpenFLPB;
        button_fl_s.clicked += OpenFLS;
        button_fl_f.clicked += OpenFLF;
        button_sl_style.clicked += OpenSLSC;
        button_settings.clicked += OpenSettings;
        button_styling.clicked += OpenStylingOptions;
        button_store_cid.clicked += StoreControllerID;
        button_restore_session.clicked += RestoreSession;
        button_settings_back.clicked += CloseSettingsWindow;
        button_style_settings_back.clicked += CloseSettingsWindow;
        textfield_config.RegisterCallback<InputEvent>(callback => manager.SetConfig(dropdown_config.value, callback.newData));

        toggle_androidbackbutton.RegisterCallback<ChangeEvent<bool>>(callback => Usercentrics.Instance.Options.Android.DisableSystemBackButton = callback.newValue);
        toggle_consentmediation.RegisterCallback<ChangeEvent<bool>>(callback => Usercentrics.Instance.Options.ConsentMediation = callback.newValue);
        toggle_preview.RegisterCallback<ChangeEvent<bool>>(callback => Usercentrics.Instance.Options.Version = callback.newValue ? "latest" : "preview");
        toggle_consentmediation.RegisterCallback<ChangeEvent<bool>>(callback => Usercentrics.Instance.Options.ConsentMediation = callback.newValue);

        dropdown_config.RegisterValueChangedCallback(evt => UpdateInputValue(evt));
    }

    private void SetValues()
    {
        CloseSettingsWindow();
        dropdown_config.choices.Add("GDPR Preset");
        dropdown_config.choices.Add("CCPA Preset");
        dropdown_config.choices.Add("Ruleset Preset");
        dropdown_config.choices.Add("Own Config");
        dropdown_config.choices.Add("Own Ruleset");
        manager.ChangeDropdown(dropdown_config.value);
        label_storedcid.text = "n/a";
        label_cid.text = "n/a";

        bannerSettings = SetBannerSettings();
    }

    private void UpdateInputValue(ChangeEvent<string> e)
    {

        manager.ChangeDropdown(e.newValue);
    }

    private void InitialiseUC()
    {
        manager.CMP_Init();
    }

    private void ResetUC()
    {
        manager.CMP_Reset();
    }
    private void OpenFLRC()
    {
        manager.ShowFirstLayer();
    }
    private void OpenSLR()
    {
        manager.ShowSecondLayer();
    }
    private void OpenFLPC()
    {
        manager.showSpecificFirstLayer(Unity.Usercentrics.UsercentricsLayout.PopupCenter);

    }
    private void OpenFLPB()
    {
        manager.showSpecificFirstLayer(Unity.Usercentrics.UsercentricsLayout.PopupBottom);
    }
    private void OpenFLS()
    {
        manager.showSpecificFirstLayer(Unity.Usercentrics.UsercentricsLayout.Sheet);

    }
    private void OpenFLF()
    {
        manager.showSpecificFirstLayer(Unity.Usercentrics.UsercentricsLayout.Full);

    }
    private void OpenSLSC()
    {
        manager.ShowSecondLayer();

    }
    private void OpenSettings()
    {
        element_ui_main.style.display = DisplayStyle.None;
        element_ui_settings.style.display = DisplayStyle.Flex;
    }
    private void OpenStylingOptions()
    {
        element_ui_main.style.display = DisplayStyle.None;
        element_ui_style.style.display = DisplayStyle.Flex;
    }
   
    private void StoreControllerID()
    {
        manager.StoreController();
    }
    private void RestoreSession()
    {
        manager.RestoreUserSession();

    }
    private void CloseSettingsWindow()
    {
        element_ui_main.style.display = DisplayStyle.Flex;
        element_ui_settings.style.display = DisplayStyle.None;
        element_ui_style.style.display = DisplayStyle.None;
    }


    public void UpdateTextInputValue(string input)
    {
        textfield_config.value = input;
    }

    public void DisableTextInput(bool isDisabled)
    {
        textfield_config.isReadOnly = isDisabled;
    }

    public Dictionary<string, string> SetBannerSettings()
    {
        bannerSettings[sliderint_fontsize.label] = sliderint_fontsize.value.ToString();
        bannerSettings[textfield_textcolor.label] = textfield_textcolor.value;
        bannerSettings[textfield_linkcolor.label] = textfield_linkcolor.value;
        bannerSettings[textfield_layerbackgroundcolor.label] = textfield_layerbackgroundcolor.value;
        bannerSettings[textfield_borderscolor.label] = textfield_borderscolor.value;

        return bannerSettings;
    }
}
