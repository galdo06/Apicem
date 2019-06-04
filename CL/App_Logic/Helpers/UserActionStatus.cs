using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eisk.Web.App_Logic.Helpers
{
    public enum GroupActionStatus
    {
        Default = 0,
        // Summary:
        //     The Group was successfully created.
        Success = 1,
        //
        // Summary:
        //     The Group name already exists in the database for the application.
        Duplicate = 2,
        //
        // Summary:
        //     The provider returned an error that is not described by other enumeration values.
        GroupError = 3
    }
}
