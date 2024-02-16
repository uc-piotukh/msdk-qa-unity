using System.Collections;
using System.Collections.Generic;
using Unity.Usercentrics;
using UnityEngine;

public class ManagerUtilities : MonoBehaviour
{
    public CMP_manager ref_manager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //---------------------BANNER SETTINGS CRAP WE DONT TOUCH THAT YET---------------------------

    public BannerSettings GetBannerSettings()
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
    

