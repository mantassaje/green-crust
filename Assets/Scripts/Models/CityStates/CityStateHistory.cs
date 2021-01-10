using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CityStateHistory
{
    public string Story { get; set; }
    public int Turn { get; set; }

    public string GetFullText()
    {
        return "Age " + Turn + ". " + Story;
    }
}

