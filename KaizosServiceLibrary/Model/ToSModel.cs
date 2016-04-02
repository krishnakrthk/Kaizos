using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosServiceLibrary.Model
{

  [DataContract]  
  public class SToS
  {

    [DataMember]
    public int ID { get; set; }

    [DataMember]
    public string TermsOfService {get;set;}

    [DataMember]
    public DateTime LastUpdate {get;set;}

    [DataMember]
    public SEnumFlag Active {get;set;}

    [DataMember]
    public string AccountNo { get; set; }

  }



}
