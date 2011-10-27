using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace FirstGiving.Sdk.Test.Integration
{
    public static class CredentialHelper
    {
        public static NetworkCredential GetCredentials()
        {
            if (!File.Exists("../../credentials.txt"))
            {
                throw new ApplicationException(@"Credentials file not found!
Create a credentials.txt at the root of the project with your key on the first line and the secret on the second line!
Best way I could find to keep credentials out of the source control!");
            }

            string[] lines = File.ReadAllLines("../../credentials.txt");
            return new NetworkCredential(lines[0], lines[1]);
        }
    }
}
