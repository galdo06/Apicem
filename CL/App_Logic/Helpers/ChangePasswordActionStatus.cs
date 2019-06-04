using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eisk.Web.App_Logic.Helpers
{
    public enum ChangePasswordActionStatus
    {
        Default = 0,
        // Summary:
        //     The ScientificName was successfully created.
        Success,
        //
        // Summary:
        //     The ScientificName name already exists in the database for the application.
        OldPasswordIncorrect,
        //
        // Summary:
        //     The provider returned an error that is not described by other enumeration values.
        NewPasswordsMissmatch 
    }
}
