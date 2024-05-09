using UnityEngine;
using UnityEngine.UI;

public class GunStatUI : MonoBehaviour
{
    [SerializeField] private Text statLabelTxt;
    [SerializeField] private Text statValueTxt;
    [SerializeField] private Text statUpValueTxt;

    public void UpdateStat(string label, string value, string upValue)
    {
        if(statLabelTxt != null) statLabelTxt.text = label; 
        if(statValueTxt != null) statValueTxt.text = value;
        if(statUpValueTxt != null) statUpValueTxt.text = upValue;
    }
}
