using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosServiceLibrary.Model
{
  [DataContract]
  public class ProfileFunctions
  {
    int profile_Code;
    int functional_Code;
    bool enabled;

    [DataMember]
    public int ProfileCode
    {
      get { return profile_Code; }
      set { profile_Code = value; }
    }

    [DataMember]
    public int FunctionalCode
    {
      get { return functional_Code; }
      set { functional_Code = value; }
    }
    
    [DataMember]
    public bool Enabled
    {
      get { return enabled; }
      set { enabled = value; }
    }

  }
}
