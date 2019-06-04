 
using System.IO;
using Eisk.Helpers;

namespace Eisk.DataAccessLayer
{
    public sealed class SqlScriptRunner
    {
        public static void RunScript(string scriptPath)
        {
            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                _DatabaseContext.ExecuteStoreCommand(Filer.ReadFromFile(scriptPath));
            }

        }
       
    }
}
