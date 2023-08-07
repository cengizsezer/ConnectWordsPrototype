using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public interface IEntityTypeDefinition
{
    public Image img { get; }
    public Image lineImg { get;  }
    public TextMeshProUGUI txt { get;  }
    public TextMeshProUGUI txtPlaceholder { get;  }

}
