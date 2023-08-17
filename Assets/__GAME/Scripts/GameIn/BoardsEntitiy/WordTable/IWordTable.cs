using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IWordTable:IGenericBase<WordTableCell>
{
    bool hasMixed { get; set; }

    void SetConnectionColors(WordTableCell wtc);

    void MixTable();

    Task SetInitialPosWithY(Task signal);

    void SetRows();
}
