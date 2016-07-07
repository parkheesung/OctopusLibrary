using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary
{
    public interface ILogger
    {
        void ErrorFormat(string msg, params object[] parameters);
        void InfoFormat(string msg, params object[] parameters);
        void WarnFormat(string msg, params object[] parameters);
        void DebugFormat(string msg, params object[] parameters);
        void FatalFormat(string msg, params object[] parameters);
    }
}
