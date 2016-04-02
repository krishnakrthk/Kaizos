using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosServiceLibrary.Model
{
  [DataContract]   
  public class SProfile
  {
    int profile_Code;
    string profile_Name;
    string description;
    string user_Type;
    
    [DataMember]
    public int ProfileCode
    {
      get {return profile_Code; }
      set { profile_Code = value; }
    }

    [DataMember]
    public string ProfileName
    {
      get { return profile_Name; }
      set { profile_Name = value; }
    }

    [DataMember]
    public string Description
    {
      get { return description; }
      set { description = value; }
    }

    [DataMember]
    public string UserType
    {
      get { return user_Type; }
      set { user_Type = value; }
    }







  }
}
