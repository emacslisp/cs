﻿using CSFtp_CLI.ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFtp_CLI.FtpCommands
{
    public class UserCommandHandler : FtpCommandHandler
    {
        public UserCommandHandler(FtpConnectionObject connectionObject)
            : base("USER", connectionObject)
        {

        }

        protected override string OnProcess(string sMessage)
        {
            ConnectionObject.User = sMessage;

            return GetMessage(331, string.Format("User {0} logged in, needs password", sMessage));
        }
    }
}
