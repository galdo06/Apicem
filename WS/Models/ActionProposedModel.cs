using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.ComponentModel.DataAnnotations;

namespace WS.Models
{
    public class ActionProposedModel
    {
        public static object GetActionProposedObject(ActionProposed actionProposed)
        {
            return (object)new
            {
                actionProposed.ActionProposedID,
                actionProposed.ActionProposedDesc,
                actionProposed.ColorID,
                actionProposed.Color.ColorDesc,
                actionProposed.Color.Code
            };
        }

        public static bool IsNull(ActionProposed requestedActionProposed, out ActionProposed actionProposed)
        {
            bool isNull = (requestedActionProposed == null || requestedActionProposed == new ActionProposed());
            actionProposed = isNull ? null : requestedActionProposed;
            return isNull;
        }

    }
}