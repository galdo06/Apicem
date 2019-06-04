using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.ComponentModel.DataAnnotations;

namespace WS.Models
{
    public class ConditionModel
    {
        public static object GetConditionObject(Condition condition)
        {
            return (object)new
            {
                condition.ConditionID,
                condition.ConditionDesc
            };
        }

        public static bool IsNull(Condition requestedCondition, out Condition condition)
        {
            bool isNull = (requestedCondition == null || requestedCondition == new Condition());
            condition = isNull ? null : requestedCondition;
            return isNull;
        }

    }
}