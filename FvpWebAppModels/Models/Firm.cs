using System;
using System.Collections.Generic;
using System.Text;

namespace FvpWebAppModels.Models
{
    public enum Firm
    {
        StatusNieznany = -1,
        NieSprawdzona = 0,
        FirmaPolska = 1,
        FirmaZagranicznazUniiEuropejskiej = 2,
        OdbiorcaIndywidualny = 4,
        FirmaZagranicznaSpozaUniiEuropejskiej = 8,
    }
}
