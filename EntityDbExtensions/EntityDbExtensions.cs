using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EntityDbExtensions
{
    public static class EntityDbExtensions
    {
        public async static Task UpdateAndHandleDeletedChildren(this DbContext context, object entityUpdated, object entityFromDb, bool execUpdate = true)
        {
            var child_entityUpdated = entityUpdated.GetType()
                                                   .GetProperties()
                                                   .FirstOrDefault(p => p.GetGetMethod().IsVirtual && p.PropertyType.IsGenericType);

            if (child_entityUpdated is not null)
            {
                var value_entityUpdated = (System.Collections.IEnumerable)child_entityUpdated.GetValue(entityUpdated);
                var value_entityFromDb = (System.Collections.IEnumerable)child_entityUpdated.GetValue(entityFromDb);

                var deletedChildren = value_entityFromDb.Cast<object>()
                                                        .Where(childFromDb => !value_entityUpdated.Cast<object>()
                                                                                                  .Any(childUpdated => Compare(childFromDb, childUpdated)))
                                                        .ToList();
                if (deletedChildren.Any())
                {
                    foreach (var deletedChild in deletedChildren)
                        context.Entry(deletedChild).State = EntityState.Deleted;
                }

                var nonDeletedChildrenFromDb = value_entityFromDb.Cast<object>()
                                                                 .Where(childFromDb => !deletedChildren.Cast<object>()
                                                                                                       .Any(deletedChild => Compare(childFromDb, deletedChild)))
                                                                 .ToList();

                foreach (var nonDeletedChildFromDb in nonDeletedChildrenFromDb)
                {
                    var childUpdated = ((System.Collections.IEnumerable)value_entityUpdated).Cast<object>()
                                                                                            .FirstOrDefault(chilUpdated => Compare(chilUpdated, nonDeletedChildFromDb));
                    if (childUpdated is not null)
                        await context.UpdateAndHandleDeletedChildren(childUpdated, nonDeletedChildFromDb, execUpdate);
                }

                if (execUpdate)
                    context.Update(entityUpdated);
            }
        }

        private static bool Compare(object childFromDb, object childUpdated) =>  GetPropertyValue(childFromDb, "Id") == GetPropertyValue(childUpdated, "Id");

        private static string GetPropertyValue(object objeto, string nomePropriedade) => Convert.ToString(objeto.GetType().GetProperty(nomePropriedade)?.GetValue(objeto));
    }
}
