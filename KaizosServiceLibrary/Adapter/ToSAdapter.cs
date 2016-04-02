using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kaizos.Entities.Business;
using KaizosServiceLibrary.Model;

namespace KaizosServiceLibrary.Adapter
{
  public class ToSAdapter
  {

    /// <summary>
    /// Convert Service Entity To Business Entity
    /// </summary>
    /// <param name="sToS"></param>
    /// <returns></returns>
    public  BToS ConvertServiceToBusinessEntity(SToS sToS)
    {
        BToS bToS = new BToS();
        bToS.TermsOfService = sToS.TermsOfService;
        bToS.Active = (BEnumFlag)sToS.Active;
        bToS.LastUpdatedDate = sToS.LastUpdate;
        bToS.AccountNo = sToS.AccountNo;
        return bToS;
    }

    /// <summary>
    /// Convert Business Entity to Service Entity
    /// </summary>
    /// <param name="bToS"></param>
    /// <returns></returns>
    public SToS ConvertBusinessToServiceEntity(BToS bToS)
    {
        SToS sToS = new SToS();
        sToS.TermsOfService = bToS.TermsOfService;
        sToS.Active = (SEnumFlag)bToS.Active;
        sToS.LastUpdate = bToS.LastUpdatedDate;
        return sToS;
    }

    /// <summary>
    /// Convert list of Business Entity to list of Service Entity (used to send back record set of a table)
    /// </summary>
    /// <param name="lsBToS"></param>
    /// <returns></returns>
    public List<SToS> GetAllTOS(List<BToS> lsBToS)
    {
       List<SToS> lsSToS= new List<SToS>();

      for (int i = 0; i < lsBToS.Count; i++)
      {
        lsSToS.Add(ConvertBusinessToServiceEntity(lsBToS[i]));
      }
      return lsSToS;
    }

  }
}
