using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using System;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    IStoreController m_StoreController;

    // ID ваших товаров
    public const string REMOVE_ADS = "remove_ads";
    public const string OPEN_CITY = "open_city";
    public const string OPEN_MEGAPOLIS = "open_megapolis";
    public string environment = "testing";

    public BuyMapCoins shopCont;
    public GameObject adsBtn;

    async void Start()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
            
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Добавляем товары, которые хотим купить
            builder.AddProduct(REMOVE_ADS, ProductType.NonConsumable);
            builder.AddProduct(OPEN_CITY, ProductType.NonConsumable);
            builder.AddProduct(OPEN_MEGAPOLIS, ProductType.NonConsumable);
        

            UnityPurchasing.Initialize(this, builder);  
        }
        catch (Exception exception)
        {
            // An error occurred during services initialization.
        }
    
        
        
    }

    // Это метод для покупки чего-либо
    public void BuyAds()
    {
        m_StoreController.InitiatePurchase(REMOVE_ADS);
    }
        public void BuyCity()
    {
        m_StoreController.InitiatePurchase(OPEN_CITY);
    }
        public void BuyMegapolis()
    {
        m_StoreController.InitiatePurchase(OPEN_MEGAPOLIS);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Инициализация завершена");
        m_StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Инициализация ошибка. Причина: {error}.";

        if (message != null)
        {
            errorMessage += $" Больше деталей: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // Получаем купленный товар
        var product = args.purchasedProduct;

        // Проверяем id купленного товара
        if (product.definition.id == REMOVE_ADS)
        {
            PlayerPrefs.SetString("NoAds", "Yes");
            Destroy(adsBtn);
        }else if(product.definition.id == OPEN_CITY){
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1000);
            shopCont.BuyNewMap(1000);
        }
        else if(product.definition.id == OPEN_MEGAPOLIS){
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 5000);
            shopCont.BuyNewMap(5000);
        }


        Debug.Log($"Покупка завершена. Товар: {product.definition.id}");
        
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Покупка ошибка - Товар: '{product.definition.id}', Причина: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Покупка ошибка - Товар: '{product.definition.id}'," +
            $" Причина: {failureDescription.reason}," +
            $" Больше информации: {failureDescription.message}");
    }

    
}
