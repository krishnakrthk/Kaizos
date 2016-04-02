using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosFunctionalServiceLibrary.Model
{
  [DataContract]
  class Tariff
  {
      int tariffID;
      
      [DataMember]
      public int TariffID
      {
        get { return tariffID; }
        set { tariffID = value; }
      }
  }
}
