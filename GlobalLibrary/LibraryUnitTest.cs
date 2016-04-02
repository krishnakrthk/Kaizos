using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kaizos.Components.GlobalLibrary;

namespace Kaizos.Components.UnitTest
{
  [TestFixture]
  class LibraryUnitTest
  {
    [Test]
    public void TestExtractError()
    {

      //string expectedErrorMessage = "this is inner exception" + "this is outer exception message";
      string expectedErrorMessage = "A"+"B" ;      
      //setup
      Exception inner = new Exception("B");
      Exception outer = new Exception("A",inner);
      
      //execute
      string actualErrorMessage = Library.ExtractError(outer);
      
      //assert
     Assert.AreEqual(expectedErrorMessage,actualErrorMessage,"Same");

    }

  }
}
