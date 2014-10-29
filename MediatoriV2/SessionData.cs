using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori
{
    public class SessionData : MyManagerCSharp.MySessionData
    {

        public SessionData(long userId)
            : base(userId)
        {

        }

    }
}
