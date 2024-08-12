using UnityEngine;
using UnityEngine.UI;

public class BuyMapCoins : MonoBehaviour
{
    public AudioClip success, fail;
    public GameObject coins1000, coins5000, money0_9, money1_9, cityBtn, megapolisBtn;
    public Animation coinsText;
    public Text coinsCount;

   
    public void BuyNewMap(int needCoins){
        
        
        int coins = PlayerPrefs.GetInt("Coins");
        if(coins < needCoins){
            if(PlayerPrefs.GetString("Music") != "Off"){
                GetComponent<AudioSource>().clip = fail;
                GetComponent<AudioSource>().Play();
                }
            coinsText.Play();
        }else{
            //Buy map
            switch(needCoins){
                case 1000:
                    PlayerPrefs.SetString("City", "Open");
                    PlayerPrefs.SetInt("NowMap", 2);
                    GetComponent<CheckMaps>().selectedMap();
                    coins1000.SetActive(false);
                    money0_9.SetActive(false);
                    cityBtn.SetActive(true);
                break;

                case 5000:
                    PlayerPrefs.SetString("Megapolis", "Open");
                    PlayerPrefs.SetInt("NowMap", 3);
                    GetComponent<CheckMaps>().selectedMap();
                    coins5000.SetActive(false);
                    money1_9.SetActive(false);
                    megapolisBtn.SetActive(true);
                break;

            }
            int nowCoins = coins - needCoins;
            coinsCount.text = nowCoins.ToString();
            PlayerPrefs.SetInt("Coins", nowCoins);

            if(PlayerPrefs.GetString("Music") != "Off"){
                GetComponent<AudioSource>().clip = success;
                GetComponent<AudioSource>().Play();
                }
        }
    }

}
