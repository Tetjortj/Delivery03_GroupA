using System.Collections.Generic;
using UnityEngine;

public enum Language { English, Spanish, Catalan }

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    public Language currentLanguage = Language.English;

    // Diccionario que mapea una clave a sus traducciones por idioma.
    private Dictionary<string, Dictionary<Language, string>> localizedTexts;

    public delegate void LanguageChanged();
    public event LanguageChanged OnLanguageChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLocalization();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadLocalization()
    {
        // Inicializa el diccionario de textos localizados
        localizedTexts = new Dictionary<string, Dictionary<Language, string>>();

        // Clave "welcome"
        localizedTexts["welcome"] = new Dictionary<Language, string>()
    {
        { Language.English, "Welcome" },
        { Language.Spanish, "Bienvenido" },
        { Language.Catalan, "Benvingut" }
    };

        // Clave "buy"
        localizedTexts["buy"] = new Dictionary<Language, string>()
    {
        { Language.English, "Buy" },
        { Language.Spanish, "Comprar" },
        { Language.Catalan, "Comprar" }
    };

        // Clave "sell"
        localizedTexts["sell"] = new Dictionary<Language, string>()
    {
        { Language.English, "Sell" },
        { Language.Spanish, "Vender" },
        { Language.Catalan, "Vendre" }
    };

        // Clave "use" (o "usar")
        localizedTexts["use"] = new Dictionary<Language, string>()
    {
        { Language.English, "Use" },
        { Language.Spanish, "Usar" },
        { Language.Catalan, "Utilitzar" }
    };

        // Clave "use" (o "usar")
        localizedTexts["money"] = new Dictionary<Language, string>()
    {
        { Language.English, "Money: " },
        { Language.Spanish, "Dinero: " },
        { Language.Catalan, "Diners: " }
    };

        // Clave "use" (o "usar")
        localizedTexts["title"] = new Dictionary<Language, string>()
    {
        { Language.English, "My new Mercadona" },
        { Language.Spanish, "Mi nuevo mercadona: " },
        { Language.Catalan, "El meu nou mercadona" }
    };

        // Agrega aquí otras claves según necesites...
    }


    public string GetLocalizedValue(string key)
    {
        if (localizedTexts.ContainsKey(key))
        {
            return localizedTexts[key][currentLanguage];
        }
        return key; // Si no se encuentra, devuelve la key
    }

    public void SetLanguage(Language lang)
    {
        currentLanguage = lang;
        if (OnLanguageChanged != null)
        {
            OnLanguageChanged();
        }
    }
    public void SetEnglish()
    {
        LocalizationManager.Instance.SetLanguage(Language.English);
    }

    public void SetSpanish()
    {
        LocalizationManager.Instance.SetLanguage(Language.Spanish);
    }

    public void SetCatalan()
    {
        LocalizationManager.Instance.SetLanguage(Language.Catalan);
    }


}
