using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kaizos.Entities.Business;

namespace Kaizos.Components.ClaimsManager
{
  public class Claims
  {
      /// <summary>
      /// Insert Claiim details
      /// </summary>
      /// <returns>
      /// 0 =&gt; Sucess
      /// 1 =&gt; Fail
      /// 2 =&gt; Duplicate
      /// </returns>
      public int InsertClaim(Claim Claim)
      {
          throw new System.NotImplementedException();
      }

      /// <summary>
      /// Get the claims which are in Open status
      /// </summary>
      /// <returns>List of Claims</returns>
      public List<Claim> GetClaim()
      {
          throw new System.NotImplementedException();
      }

      public int UpdateClaim(List<Claim> Claim)
      {
          throw new System.NotImplementedException();
      }
  }
}
