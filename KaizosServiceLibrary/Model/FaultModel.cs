using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

          
namespace KaizosServiceLibrary

{   
    [DataContract]
    public class SGeneralFault
    {
        [DataMember]
        public string Issue { get; set; }

        [DataMember]
        public string Details { get; set; }
    }
  
}
