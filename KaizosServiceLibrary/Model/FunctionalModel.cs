using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosServiceLibrary.Model
{

  [DataContract]
  public class Functionality
  {
    int functional_Code;
    string functional_Name;
    string descrption;

    [DataMember]
    public int FunctionalCode
    {
      get { return functional_Code; }
      set { functional_Code = value; }
    }

    [DataMember]
    public string FunctionalName
    {
      get { return functional_Name; }
      set { functional_Name = value; }
    }

    [DataMember]
    public string Description
    {
      get { return descrption;}
      set { descrption = value; }
    }


  }
}
