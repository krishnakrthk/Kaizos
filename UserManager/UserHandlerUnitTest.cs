using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kaizos.Components.UserManager;
using KaizosEntities;
using System.Configuration;
using log4net;
namespace Kaizos.Components.UnitTest
{
  [TestFixture]  
  class UserHandlerUnitTest
  {
    [Test]
    public void TestInsertProfile()
    {

      //Load the Log4Net configuration under the Log4 section in App.config file.
      //Its mandatory or else no error, no output !!
      log4net.Config.XmlConfigurator.Configure();

      //To print (see in Nunit output window) the current configuration file name.
      Console.WriteLine(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToString());

      //Walkthrough the Log4Net section in configuration file.
      /*
      log4net.Repository.ILoggerRepository RootRep;

      RootRep = log4net.LogManager.GetRepository();

      foreach (log4net.Appender.IAppender iApp in RootRep.GetAppenders())
      {
        if (iApp is log4net.Appender.FileAppender)
        {
          log4net.Appender.FileAppender fApp = (log4net.Appender.FileAppender)iApp;
          Console.WriteLine("Filename = {0}", fApp.File);
        }
      }
      */

     // PROFILE profile = new PROFILE();
      //profile.PROFILE_CODE = 109;
    //  profile.PROFILE_NAME = "Residential";
    //  profile.DESCRIPTION = "This profile is used for Kaizos admin";
    //  profile.USER_TYPE = "N";
      
     // UserHandler userHandler = new UserHandler();

    //  int result = userHandler.InsertProfile(profile);

    //  Assert.AreEqual(1, result, "same");

    }
  }
}
