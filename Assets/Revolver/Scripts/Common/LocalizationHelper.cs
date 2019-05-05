using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalizationHelper : MonoBehaviour {
    // todo - make read from file
    public enum LocalizationType
    {
        English = 0,
        Belarussian = 1,
        Russian = 2
    }

    public static LocalizationHelper runtime;
    public Action OnChangeLocalization;
    private LocalizationType _localization = LocalizationType.English;// todo add save
    public LocalizationType Localization
    {
        get { return _localization; }
        set
        {
            _localization = value;
            PlayerPrefs.SetInt("localize", (int)_localization);
            if (OnChangeLocalization != null)
                OnChangeLocalization();
        }
    }


    private Dictionary<string, Dictionary<string, string>> localizationLibrary;
	// Use this for initialization
    private void Awake()
    {
        if (runtime != null)
        {
            Destroy(this);
            return;
        }

        runtime = this;
        Initialize();
    }

    private void Initialize()
    {
        var english = new Dictionary<string, string>
        {
            {"Lanquage", "Lanquage"},
            {"Sound", "Sound"},
            {"Effects", "Effects"},
            {"Support", "Next"},
            {"Load", "Load"},
            {"Start", "Start"},
            {"Art", "Art"},
            {"autor1", "Anatoli Zaicev"},
            {"Programming", "Programming"},
            {"autor2", "Stanislav Nos"},
            {"new", "New Game"},
            {"continue", "Continue"},
            {"Loading", "Loading"},
            {"baraban", "Show/Hide"},
            {"settings", "Settings"},
            {"hide", "Hide"},
            {"back", "Back"}

        };
        var russian = new Dictionary<string, string>
        {
            {"Lanquage", "Языкъ"},
            {"Sound", "Музыка"},
            {"Effects", "Звук"},
            {"Support", "Продолжить"},
            {"Load", "Загрузить"},
            {"Start", "Стреляться"},
            {"Art", "Художник"},
            {"autor1", "Анатолий Зайцев"},
            {"Programming", "Кудесник кода"},
            {"autor2", "Станислав Нос"},
            {"new", "Cначала"},
            {"continue", "Продолжить"},
            {"Loading", "Загрузка"},
            {"baraban", "Показать/Скрыть"},
            {"settings", "Настройки"},
            {"hide", "Скрыть"},
            {"back", "Назад"}
        };
        var belarus = new Dictionary<string, string>
        {  {"Lanquage", "Мова"},
            {"Sound", "Музыка"},
            {"Effects", "іншы гук"},
            {"Support", "Далей"},
            {"Load", "Загрузить"},
            {"Start", "Страляцца"},
            {"Art", "Мастак"},
            {"autor1", "Анатоль Зайцаў"},
            {"Programming", "Праграміст"},
            {"autor2", "Станіслаў Нос"}, 
            {"new", "Спачатку"},
            {"continue", "Застацца"},
            {"Loading", "Пампуецца"},
            {"baraban", "Паказаць / Прыбраць"},
            {"settings", "Налады"},
            {"hide", "Hide"},
            {"back", "Вярнуцца"}
          
        };

        localizationLibrary = new Dictionary<string, Dictionary<string, string>>
        {
                    {"English",english},
                    {"Belarussian",belarus},
                    {"Russian", russian}
        };


        var i = PlayerPrefs.GetInt("localize",0);
        Debug.Log("Load LocalizationType " + i);
        Localization = (LocalizationType) i;
    }


    public string GetTextByKey(string key)
    {
        if (string.IsNullOrEmpty(key)) return string.Empty;

        Dictionary<string, string> dictionary;
        if (localizationLibrary.TryGetValue(_localization.ToString(), out dictionary))
        {
            string result;
            if (dictionary.TryGetValue(key, out result))
                return result;
        }


        return key;
    }
}

