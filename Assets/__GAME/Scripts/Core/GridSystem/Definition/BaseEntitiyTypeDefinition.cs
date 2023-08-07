using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Base Entity Type Definition")]
public class BaseEntitiyTypeDefinition : ScriptableObject, IEntityTypeDefinition
{
    [BHeader("Base Grid Entity Settings")]
    [SerializeField] protected Image EntityImage;
    [SerializeField] protected Image LineImage;
    [SerializeField] protected TextMeshProUGUI MainText;
    [SerializeField] protected TextMeshProUGUI PlaceholderText;
    public Image img => EntityImage;

    public Image lineImg => LineImage;

    public TextMeshProUGUI txt => MainText;

    public TextMeshProUGUI txtPlaceholder => PlaceholderText;
}
