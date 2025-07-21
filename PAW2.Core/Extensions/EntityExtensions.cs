using PAW2.Models;
using PAW2.Models.Enums;
using System.Diagnostics;


namespace PAW2.Core.Extensions
{
    public static class EntityExtensions
    {
        public static void AddAudit(this IEntity entity, string user)
        {
            if (entity.IsDirty ?? false)
            {
                if (entity.TempID <= 0)
                {
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = user;
                }
                else
                {
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = user;
                }
            }
        }

        public static void AddLogging(this IEntity entity, LoggingType logginType)
        {
            if (logginType == LoggingType.Create)
            {
                Debug.WriteLine("Creating object!");
                return;
            }

            if (logginType == LoggingType.Update)
            {
                Debug.WriteLine("Updating object!");
                return;
            }


            if (logginType == LoggingType.Delete)
            {
                Debug.WriteLine("Deleting object!");
                return;
            }


            if (logginType == LoggingType.Read)
            {
                Debug.WriteLine("Reading object!");
                return;
            }
        }
    }
}
