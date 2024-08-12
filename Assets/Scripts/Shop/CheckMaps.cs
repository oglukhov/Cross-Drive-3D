using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheckMaps : MonoBehaviour
{
    public Image[] maps;
    public Sprite selected, notSelected;
    private BuyMapCoins _mapCoins;

    private void Start(){
    
        selectedMap();
        _mapCoins = GetComponent<BuyMapCoins>();
        if(PlayerPrefs.GetString("City") == "Open"){
            _mapCoins.coins1000.SetActive(false);
            _mapCoins.money0_9.SetActive(false);
            _mapCoins.cityBtn.SetActive(true);
        }
        if(PlayerPrefs.GetString("Megapolis") == "Open"){
            _mapCoins.coins5000.SetActive(false);
            _mapCoins.money1_9.SetActive(false);
            _mapCoins.megapolisBtn.SetActive(true);  
        }
    }

    public void selectedMap(){
        switch(PlayerPrefs.GetInt("NowMap")){
        case 2:
            maps[0].sprite = notSelected;
            maps[1].sprite = selected;
            maps[2].sprite = notSelected;
            break;
        case 3:
            maps[0].sprite = notSelected;
            maps[1].sprite = notSelected;
            maps[2].sprite = selected;
        break;
        default:
            maps[0].sprite = selected;
            maps[1].sprite = notSelected;
            maps[2].sprite = notSelected;
        break;}
    }
}
