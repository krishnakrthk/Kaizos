using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KaizosFunctionalServiceLibrary;
using Kaizos.Components.TariffManager;

namespace KaizosFunctionalServiceLibrary
{
  public class KaizosFunctionalService : KaizosFucntionalServiceContract
  {
    #region Tariff Management

      public int GetTarriffID()
      {
        TariffHandler tariffManager = new TariffHandler();
        return tariffManager.GetTariffID();
      }

    #endregion
    
  }
}
