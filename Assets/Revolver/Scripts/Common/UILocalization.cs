using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// helper

public class UILocalization : MonoBehaviour
{
    public string _key;
    public string Key {
        get { return _key; }
        set
        {
            _key = value;
            Localize();
        }
    }

    private Text Label;
    // Use this for initialization
	void Start()
	{
	    LocalizationHelper.runtime.OnChangeLocalization += OnChangeLocalization;
	    Label = gameObject.GetComponent<Text>();
	    OnChangeLocalization();
	}

    private void OnChangeLocalization()
    {
        Localize();
    }

    private void Localize()
    {
        Label.text = LocalizationHelper.runtime.GetTextByKey(_key);
    }

    private void OnDestroy()
    {
        LocalizationHelper.runtime.OnChangeLocalization -= OnChangeLocalization;
    }

    // Update is called once per frame
	void Update () {
	
	}
}
