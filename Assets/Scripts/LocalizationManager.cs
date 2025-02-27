using System.Collections.Generic;
using UnityEngine;

public enum Language { English, Spanish, Catalan }

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    public Language currentLanguage = Language.English;

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
        localizedTexts = new Dictionary<string, Dictionary<Language, string>>();

        localizedTexts["welcome"] = new Dictionary<Language, string>()
    {
        { Language.English, "Welcome" },
        { Language.Spanish, "Bienvenido" },
        { Language.Catalan, "Benvingut" }
    };

        localizedTexts["buy"] = new Dictionary<Language, string>()
    {
        { Language.English, "Buy" },
        { Language.Spanish, "Comprar" },
        { Language.Catalan, "Comprar" }
    };

        localizedTexts["sell"] = new Dictionary<Language, string>()
    {
        { Language.English, "Sell" },
        { Language.Spanish, "Vender" },
        { Language.Catalan, "Vendre" }
    };

        localizedTexts["use"] = new Dictionary<Language, string>()
    {
        { Language.English, "Use" },
        { Language.Spanish, "Usar" },
        { Language.Catalan, "Utilitzar" }
    };

        localizedTexts["money"] = new Dictionary<Language, string>()
    {
        { Language.English, "Money: " },
        { Language.Spanish, "Dinero: " },
        { Language.Catalan, "Diners: " }
    };

        localizedTexts["title"] = new Dictionary<Language, string>()
    {
        { Language.English, "My new Mercadona" },
        { Language.Spanish, "Mi nuevo mercadona" },
        { Language.Catalan, "El meu nou mercadona" }
    };

        localizedTexts["start"] = new Dictionary<Language, string>()
    {
        { Language.English, "Start" },
        { Language.Spanish, "Iniciar" },
        { Language.Catalan, "Començar" }
    };

        localizedTexts["end"] = new Dictionary<Language, string>()
    {
        { Language.English, "The end" },
        { Language.Spanish, "Final" },
        { Language.Catalan, "Final" }
    };

    }


    public string GetLocalizedValue(string key)
    {
        if (localizedTexts.ContainsKey(key))
        {
            return localizedTexts[key][currentLanguage];
        }
        return key;
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
