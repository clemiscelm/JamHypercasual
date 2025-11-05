using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class ShopManager : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    // 🔹 IDs configurés dans Unity IAP Catalog
    public const string PRODUCT_CURRENCY_SMALL = "currency_small"; // ex : 1€
    public const string PRODUCT_CURRENCY_MEDIUM = "currency_medium"; // ex : 5€
    public const string PRODUCT_CURRENCY_LARGE = "currency_large"; // ex : 10€

    public event Action<float, int> OnPurchaseCompleted; 
    // (prixEnEuros, quantitéDeCurrency)

    void Start()
    {
        if (storeController == null)
            InitializePurchasing();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // 🔹 Enregistre tes produits ici :
        builder.AddProduct(PRODUCT_CURRENCY_SMALL, ProductType.Consumable);
        builder.AddProduct(PRODUCT_CURRENCY_MEDIUM, ProductType.Consumable);
        builder.AddProduct(PRODUCT_CURRENCY_LARGE, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    // 🔹 Appelé par un bouton dans l’UI
    public void BuyCurrencySmall() => BuyProductID(PRODUCT_CURRENCY_SMALL);
    public void BuyCurrencyMedium() => BuyProductID(PRODUCT_CURRENCY_MEDIUM);
    public void BuyCurrencyLarge() => BuyProductID(PRODUCT_CURRENCY_LARGE);

    void BuyProductID(string productId)
    {
        if (storeController == null) return;

        Product product = storeController.products.WithID(productId);
        if (product != null && product.availableToPurchase)
        {
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.LogWarning("Produit non disponible ou introuvable : " + productId);
        }
    }

    // === IStoreListener ===

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("IAP Init Failed: " + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Product product = args.purchasedProduct;

        // 🔹 Récupère le prix (en euros)
        float priceInEuro = (float)product.metadata.localizedPrice;

        int currencyAmount = 0;
        switch (product.definition.id)
        {
            case PRODUCT_CURRENCY_SMALL:
                currencyAmount = 100; // exemple
                break;
            case PRODUCT_CURRENCY_MEDIUM:
                currencyAmount = 600;
                break;
            case PRODUCT_CURRENCY_LARGE:
                currencyAmount = 1500;
                break;
        }

        Debug.Log($"Achat réussi : {product.definition.id} | {priceInEuro}€ | +{currencyAmount} currency");
        PlayerData.IncriseHardCurrency(currencyAmount);
        // 🔹 Appelle un event si tu veux le traiter ailleurs
        OnPurchaseCompleted?.Invoke(priceInEuro, currencyAmount);

        // 🔹 Ajoute la currency au joueur ici
        PlayerPrefs.SetInt("currency", PlayerPrefs.GetInt("currency", 0) + currencyAmount);

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogWarning($"Achat échoué : {product.definition.id}, raison : {failureReason}");
    }
}
