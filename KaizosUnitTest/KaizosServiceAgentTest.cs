using System;
using KaizosServiceInvokers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KaizosServiceInvokers.KaizosServiceReference;

namespace KaizosUnitTest
{
    
    
    /// <summary>
    ///This is a test class for KaizosServiceAgentTest and is intended
    ///to contain all KaizosServiceAgentTest Unit Tests
    ///</summary>
  [TestClass()]
  public class KaizosServiceAgentTest
  {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for UpdateProfileFunctions
    ///</summary>
    [TestMethod()]
    public void UpdateProfileFunctionsTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      ProfileFunctions[] profileFunctions = null; // TODO: Initialize to an appropriate value
      int expected = 0; // TODO: Initialize to an appropriate value
      int actual;
      actual = target.UpdateProfileFunctions(profileFunctions);
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for UpdateProfile
    ///</summary>
    [TestMethod()]
    public void UpdateProfileTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      Profile profile = null; // TODO: Initialize to an appropriate value
      int expected = 0; // TODO: Initialize to an appropriate value
      int actual;
      actual = target.UpdateProfile(profile);
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for UpdateFunctionality
    ///</summary>
    [TestMethod()]
    public void UpdateFunctionalityTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      Functionality functionality = null; // TODO: Initialize to an appropriate value
      int expected = 0; // TODO: Initialize to an appropriate value
      int actual;
      actual = target.UpdateFunctionality(functionality);
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for InsertToS
    ///</summary>
    [TestMethod()]
    public void InsertToSTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      ToS tos = new ToS();
      tos.Condition = "test condition from VS unit test";
      tos.Active = true;
      tos.LastUpdate = DateTime.Now;
      int expected = 0; // TODO: Initialize to an appropriate value
      int actual =0;
      actual = target.InsertToS(tos);
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for InsertFunctionalty
    ///</summary>
    [TestMethod()]
    public void InsertFunctionaltyTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      Functionality functionality = null; // TODO: Initialize to an appropriate value
      int expected = 0; // TODO: Initialize to an appropriate value
      int actual;
      actual = target.InsertFunctionalty(functionality);
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for InserProfile
    ///</summary>
    [TestMethod()]
    public void InserProfileTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      Profile profile = null; // TODO: Initialize to an appropriate value
      int expected = 0; // TODO: Initialize to an appropriate value
      int actual;
      actual = target.InserProfile(profile);
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for GetToS
    ///</summary>
    [TestMethod()]
    public void GetToSTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      ToS[] expected = null; // TODO: Initialize to an appropriate value
      ToS[] actual;
      actual = target.GetToS();
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for GetAllProfileFunctions
    ///</summary>
    [TestMethod()]
    public void GetAllProfileFunctionsTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      ProfileFunctions[] expected = null; // TODO: Initialize to an appropriate value
      ProfileFunctions[] actual;
      actual = target.GetAllProfileFunctions();
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for DeleteProfile
    ///</summary>
    [TestMethod()]
    public void DeleteProfileTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      int profileCode = 0; // TODO: Initialize to an appropriate value
      int expected = 0; // TODO: Initialize to an appropriate value
      int actual;
      actual = target.DeleteProfile(profileCode);
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for DeleteFunctionality
    ///</summary>
    [TestMethod()]
    public void DeleteFunctionalityTest()
    {
      KaizosServiceAgent target = new KaizosServiceAgent(); // TODO: Initialize to an appropriate value
      int functionalCode = 0; // TODO: Initialize to an appropriate value
      int expected = 0; // TODO: Initialize to an appropriate value
      int actual;
      actual = target.DeleteFunctionality(functionalCode);
      Assert.AreEqual(expected, actual);
      Assert.Inconclusive("Verify the correctness of this test method.");
    }
  }
}
