using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace KaizosFunctionalServiceLibrary
{
  [ServiceContract]
  public interface KaizosFucntionalServiceContract
  {
    [OperationContract]
    int GetTarriffID();
  }
}
