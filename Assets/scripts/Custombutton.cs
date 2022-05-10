using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ButtonAction{
    leave,
    leave_selectPlayers,
    leave_shop,
    leave_redeem,
    exitGame,
    mute,
    graphics,
    openlink,
    shareapp,
    shareroomid,
    reconnect,
    reconnectingame,
    playWithBot

}
public class Custombutton : MonoBehaviour
{
   
   public Button actionbtn;
   public InputField inputField;
   public ButtonAction onClick;
   public int valume;
   public string link;
    void Start()
    {
        actionbtn=GetComponent<Button>();
        actionbtn.onClick.AddListener(()=>{
            switch (onClick){
                 case ButtonAction.mute:

                    settings._Instance.mute(valume);
                break;
                case ButtonAction.leave:

                    settings._Instance.leave();
                break;
                case ButtonAction.leave_selectPlayers:

                    settings._Instance.leave_options();
                break;
                case ButtonAction.leave_shop:

                    settings._Instance.leave_shop();
                break;
                case ButtonAction.leave_redeem:

                    settings._Instance.leave_redeem();
                break;
                 case ButtonAction.exitGame:

                    settings._Instance.Exit();
                break;
                 case ButtonAction.graphics:

                    settings._Instance.graphics(valume);
                break;
                 case ButtonAction.openlink:

                    settings._Instance.openlink(link);
                break;
                 case ButtonAction.shareapp:

                    settings._Instance.shareapp();
                break;
                 case ButtonAction.shareroomid:

                    settings._Instance.shareroomid(inputField.text);
                break;
                 case ButtonAction.reconnect:

                    settings._Instance.reconnectgame();
                break;
                case ButtonAction.reconnectingame:
                break;
                case ButtonAction.playWithBot:
                    settings._Instance.PlayWithBot();
                break;

            }
        });
    }
}
