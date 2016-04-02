using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using KaizosEntities;
using Kaizos.Entities.Business;
using System.Data.Objects;

namespace Kaizos.Components.ToSManager
{
  public class TermsOfService
  {
    
    /// <summary>
    /// Insert new Terms of Service
    /// </summary>
    /// <returns>
    /// 0 =&gt; Success
    /// 1 =&gt; Fail
    /// </returns>
    public void InsertToS(BToS tos)
    {
 
            DToS dToS = new DToS();

            dToS.ACCOUNT_NO = (string)tos.AccountNo;
            
            if (tos.Active == BEnumFlag.Yes)
                dToS.ACTIVE = "Y";
            else
                dToS.ACTIVE = "N";

            dToS.LAST_UPDATE_DATE = tos.LastUpdatedDate;
            
            dToS.TERMS_OF_SERVICE = tos.TermsOfService;
            
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            
            context.AddToDTOS(dToS);
            context.SaveChanges();
            
    }

    /// <summary>
    /// Insert new Terms of Service through stored procedure
    /// </summary>
    /// <returns>
    /// 0 =&gt; Success
    /// 1 =&gt; Fail
    /// </returns>
    /// 
    public int SpInsertToS(BToS tos)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        
        string ActiveFlag;

        if (tos.Active==BEnumFlag.Yes)
            ActiveFlag="Y";
        else
           ActiveFlag="N";

        System.Nullable<int> iReturnValue = context.uSP_TOS_INSERT(tos.TermsOfService, tos.LastUpdatedDate, ActiveFlag, tos.AccountNo).SingleOrDefault();

        return (int)iReturnValue;

    }

    public BToS GetActiveToS()
    {
        BToS bToS = new BToS();
        DToS dToS = new DToS();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        if (context.DTOS.Count() == 0)
        {
            bToS.AccountNo = "1";
            if (dToS.ACTIVE == "Yes")
                bToS.Active = BEnumFlag.Yes;
            else
                bToS.Active = BEnumFlag.No;
            bToS.LastUpdatedDate = DateTime.Now;
            bToS.TermsOfService = "";
        }
        else
        {
            dToS = context.DTOS.First(e => e.ACTIVE == "Y");
            bToS.AccountNo = dToS.ACCOUNT_NO;
            if (dToS.ACTIVE == "Y")
                bToS.Active = BEnumFlag.Yes;
            else
                bToS.Active = BEnumFlag.No;
            bToS.LastUpdatedDate = dToS.LAST_UPDATE_DATE;
            
            bToS.TermsOfService = dToS.TERMS_OF_SERVICE;
        }

        return bToS;
    }       

  }
    
    
 
}
