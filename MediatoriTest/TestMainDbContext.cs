using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mediatori.Models
{
    [TestClass]
    public class TestMainDbContext
    {
        [TestMethod]
        public void TestGetConnectionByUrl()
        {
            String url = "http://test.localhost.it/Controller/Action";
            String result=MainDbContext.getConnectionByUrl(url);
            Assert.AreEqual(result,"test",String.Format("codifica {0} fallita restituito {1}",url,result));

            url = "https://test.localhost.it/Controller/Action";
            result = MainDbContext.getConnectionByUrl(url);
            Assert.AreEqual(result, "test", String.Format("codifica {0} fallita restituito {1}", url, result));

            url = "http://localhost.it/Controller/Action";
            result = MainDbContext.getConnectionByUrl(url);
            Assert.AreEqual(result, "DefaultConnection", String.Format("codifica {0} fallita restituito {1}", url, result));



        }
    }
}
