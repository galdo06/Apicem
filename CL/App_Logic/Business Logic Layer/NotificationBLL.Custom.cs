
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.Helpers;
using System.Web;

namespace Eisk.BusinessLogicLayer
{
    public partial class NotificationBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification> GetNotificationByFilter(string searchText = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int groupID = 0, string userID = null)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "notificationSubject";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            Guid userIDGuid = Guid.Parse(userID);

            List<Notification> notifications = (
                    from notificationUsersList in
                        _DatabaseContext.Notification_User
                    where
                        (
                            (
                                groupID == 0 ||
                                notificationUsersList.GroupID == groupID ||
                                notificationUsersList.GroupID == null
                            )
                            &&
                            (
                                userID == null ||
                                notificationUsersList.UserID == userIDGuid ||
                                notificationUsersList.UserID == null
                            )
                        )
                    select notificationUsersList.Notification
                )
                .Where(i =>
                        string.IsNullOrEmpty(searchText) 
                        || i.NotificationSubject.ToUpper().Trim().Contains(searchText.ToUpper().Trim())
                        || i.NotificationDesc.ToUpper().Trim().Contains(searchText.ToUpper().Trim())
                      )
                .OrderByDescending(ii => ii.EditedDate)
                .ToList();
            
            var notificationNamesList = notifications.Skip(startRowIndex).Take(maximumRows).ToList();

            HttpContext.Current.Items["notifications_Count"] = notifications.Count;

            return notificationNamesList;
        }

        public int GetTotalCountForAllNotifications(string searchText = default(string), string orderBy = default(string), int startRowIndex = default(int), int maximumRows = 1, int groupID = 0, string userID = null)
        {
            var count = (int)HttpContext.Current.Items["notifications_Count"];
            HttpContext.Current.Items.Remove("notifications_Count");
            return count;
        }

    }
}
