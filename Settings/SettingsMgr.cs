using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMgr : MonoBehaviour
{
    public bool isTest;

    public GameObject panelSettings;

    public GameObject bgmSlider;
    public GameObject efmSlider;

    Slider bgm_s;
    Slider efm_s;

    public Text txtBgm;
    public Text txtEfm;

    public Text txtVersion;
    public Text txtUserNum;

    public GameObject panelCoupon;      //  쿠폰 입력 패널
    public InputField couponInput;      //  쿠폰 입력 필드

    public GameObject confirmCoupon;    //  쿠폰 사용 후 패널
    public Text messageCoupon;

    public GameObject resPanel;         //  자원 패널

    void Start()
    {
        if (isTest)
            testInit();

        initSettings();

        bgm_s = bgmSlider.GetComponent<Slider>();
        bgm_s.onValueChanged.AddListener(OnBgmChanged);

        efm_s = efmSlider.GetComponent<Slider>();
        efm_s.onValueChanged.AddListener(OnEfmChanged);
    }

    void testInit()
    {
        Game.current = new Game();

        Game.current.setVibrate(false);
        Game.current.setBgm(45);
        Game.current.setEfm(60);

        userData.data = new userData();
    }

    void initSettings()
    {
        float bgmValue = (float)Game.current.getBgm() / 100;
        float efmValue = (float)Game.current.getEfm() / 100;
        bgmSlider.GetComponent<Slider>().value = bgmValue;
        efmSlider.GetComponent<Slider>().value = efmValue;

        int bgm = (int)(bgmValue*100);
        txtBgm.text = "" + bgm;

        int efm = (int)(efmValue*100);
        txtEfm.text = "" + efm;

        txtVersion.text = Game.current.getVersion();
        txtUserNum.text = userData.data.getUserNum();
    }

    public void OnVibrateOn(bool vibrate)
    {
        Game.current.setVibrate(true);
    }

    public void OnVibrateOff(bool vibrate)
    {
        Game.current.setVibrate(false);
    }

    public void OnBgmChanged(float value)
    {
        int bgm = (int)(value * 100);
        txtBgm.text = "" + bgm;

        Game.current.setBgm(bgm);
    }

    public void OnEfmChanged(float value)
    {
        int efm = (int)(value * 100);
        txtEfm.text = "" + efm;

        Game.current.setEfm(efm);
    }

    public void OnClickBtn_Coupon()
    {
        panelCoupon.SetActive(true);
    }

    public void OnClickCouponUse()
    {
        string input = couponInput.text.ToString();
        if (Game.current.CheckCoupon(input))
        {
            // 쿠폰번호가 맞을 경우
            int tmpNum = Game.current.getCouponNum();

            if (userData.data.getUsedCoupon(tmpNum))
            {               
                // 이미 사용한 경우 (쿠폰 미적용)
                messageCoupon.text = "이미 해당 쿠폰을 사용하셨습니다.";
            }
            else
            {
                // 처음 사용일 경우 (쿠폰 적용)
                switch (tmpNum)
                {
                    case 0:
                        // iimin :     +500,000 gold   +500 ruby
                        userData.data.incGold(500000);
                        userData.data.incRuby(500);
                        break;
                    case 1:
                        // batcastle :  +100 ruby (10,000원 상당)
                        userData.data.incRuby(100);
                        break;
                    case 2:
                        // jinp12 :     +100,000 gold
                        userData.data.incGold(100000);
                        break;
                    case 3:
                        // jjangcyc :   +100,000 gold   +100 ruby
                        userData.data.incGold(100000);
                        userData.data.incRuby(100);
                        break;
                    case 4:

                        break;
                }
                
                resPanel.GetComponent<ResPanelMgr>().UpdateMoney();
                userData.data.useCoupon(tmpNum);

                messageCoupon.text = "쿠폰 보상이 지급되었습니다.";
            }
        }
        else
        {
            // 쿠폰 번호가 틀릴 경우
            messageCoupon.text = "잘못된 쿠폰 번호입니다.";
        }

        confirmCoupon.SetActive(true);
    }

    public void OnClickBtn_ExitCoupon()
    {
        confirmCoupon.SetActive(false);
        panelCoupon.SetActive(false);
    }

    public void OnClickBtn_ExitSettings()
    {
        panelSettings.SetActive(false);
    }
}
