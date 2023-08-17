using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface ICellBase
{
    int i { get; set; }
    int j { get; set; }
    //Image img { get; set; }
    //Image lineImg { get; set; }
    //TextMeshProUGUI txt { get; set; }
    //TextMeshProUGUI txtPlaceholder { get; set; }
    Color defaultImgColor { get; set; }
    Color defaultTextColor { get; set; }
    void SetText(int value);
    void SetAvailable(bool isAvailable);
    void SetFixed();
    bool IsActive();
    bool IsEmpty();
    bool IsNumber();
    bool IsWord();
    void SetImageAlpha(float alpha);
    void SetTextAlpha(float alpha);


}
