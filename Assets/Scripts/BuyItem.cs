using UnityEngine;

public class BuyItem : MonoBehaviour
{
    public enum Types{
        REMOVE_ADS, OPEN_CITY, OPEN_MEGAPOLIS
    }
    public Types type;
    public IAPManager iapManager;
    public void BuyItems(){
        switch(type){
            case Types.REMOVE_ADS:
                iapManager.BuyAds();
                break; 
            case(Types.OPEN_CITY):
                iapManager.BuyCity();
                break;
            case(Types.OPEN_MEGAPOLIS):
                iapManager.BuyMegapolis();
                break;
        }
    }
}
