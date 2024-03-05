using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dropdownControl : MonoBehaviour
{
    private TMP_Dropdown drop;
    private TextMeshProUGUI txt;
    TMP_Dropdown.OptionData opt1, opt2,opt3,opt4,opt5,opt6;
    List<TMP_Dropdown.OptionData> messages = new List<TMP_Dropdown.OptionData>();
    int msgIndex;
    public SceneController SceneCtrl;

    // Start is called before the first frame update
    void Start()
    {
        SceneCtrl = GameObject.Find("Controller").GetComponent<SceneController>();
        txt = GameObject.Find("Label").GetComponent<TextMeshProUGUI>();
        //Fetch the Dropdown GameObject the script is attached to
        drop = GetComponent<TMP_Dropdown>();
        //Clear the old options of the Dropdown menu
        drop.ClearOptions();

        opt1 = new TMP_Dropdown.OptionData();
        opt1.text = "2 x 3";
        messages.Add(opt1);

        opt2 = new TMP_Dropdown.OptionData();
        opt2.text = "2 x 4";
        messages.Add(opt2);

        opt3 = new TMP_Dropdown.OptionData();
        opt3.text = "2 x 5";
        messages.Add(opt3);

        opt4 = new TMP_Dropdown.OptionData();
        opt4.text = "3 x 4";
        messages.Add(opt4);

        opt5 = new TMP_Dropdown.OptionData();
        opt5.text = "4 x 4";
        messages.Add(opt5);

        opt6 = new TMP_Dropdown.OptionData();
        opt6.text = "4 x 5";
        messages.Add(opt6);

        foreach (TMP_Dropdown.OptionData message in messages)
        {
            //Add each entry to the Dropdown
            drop.options.Add(message);
            msgIndex = messages.Count - 1;
        }

        drop.onValueChanged.AddListener(delegate { ValueChanged(drop); });
        drop.value = PlayerPrefs.GetInt("dropdown_value", 0);
        drop.RefreshShownValue();
        txt.text = drop.options[drop.value].text;
    }
  
    void Update()
    {
        
    }

    void ValueChanged(TMP_Dropdown change)
    {
        txt.text = "Option " + change.value + ": " + change.options[change.value].text;
        switch (change.value)
        {
            case 0: //2 x 3
                SceneCtrl.SetSize(2, 3);
                PlayerPrefs.SetInt("rows", 2);
                PlayerPrefs.SetInt("columns", 3);
                PlayerPrefs.SetInt("dropdown_value", 0);
                break;
            case 1: //2 x 3
                SceneCtrl.SetSize(2, 4);
                PlayerPrefs.SetInt("rows", 2);
                PlayerPrefs.SetInt("columns", 4);
                PlayerPrefs.SetInt("dropdown_value", 1);
                break;
            case 2: //2 x 5
                SceneCtrl.SetSize(2, 5);
                PlayerPrefs.SetInt("rows", 2);
                PlayerPrefs.SetInt("columns", 5);
                PlayerPrefs.SetInt("dropdown_value", 2);
                break;                                   
            case 3: //3 x 4
                SceneCtrl.SetSize(3, 4);
                PlayerPrefs.SetInt("rows", 3);
                PlayerPrefs.SetInt("columns", 4);
                PlayerPrefs.SetInt("dropdown_value", 3);
                break;         
            case 4: //4 x 4
                SceneCtrl.SetSize(4, 4);
                PlayerPrefs.SetInt("rows", 4);
                PlayerPrefs.SetInt("columns", 4);
                PlayerPrefs.SetInt("dropdown_value", 4);
                break;         
            case 5: //4 x 5
                SceneCtrl.SetSize(4, 5);
                PlayerPrefs.SetInt("rows", 4);
                PlayerPrefs.SetInt("columns", 5);
                PlayerPrefs.SetInt("dropdown_value", 5);
                break;         
     
        }
        PlayerPrefs.Save();
    }
}