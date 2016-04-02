using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KaizosServiceInvokers;
using NUnit.Framework;
using KaizosServiceInvokers.KaizosServiceReference;
using log4net;

namespace KaizosServiceInvokers
{
  [TestFixture]
  public class ServiceAgentUnitTest
  {


    [Test]
    public void TestInsertProfile()
    {

      log4net.Config.XmlConfigurator.Configure();
       
      //Profile profile = new Profile();
      //profile.ProfileCode = 110;
      //profile.ProfileName = "Test 111";
      //profile.Description = "Test 111";
      //profile.UserType = "nn";

      //KaizosServiceAgent agent = new KaizosServiceAgent();
      //agent.InserProfile(profile);
    }
    
  }
}
