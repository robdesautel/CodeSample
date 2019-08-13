using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.SharedObjects.Extensions
{
    public static class LinqExtensions
    {

        public static IEnumerable<T> RemoveNulls<T>(this IEnumerable<T> list)
        {
            if(list == null)
            {
                throw new Exception("The list cannot be null");
            }

            foreach(var item in list)
            {
                if (item == null)
                {
                    continue;
                }

                yield return item;
            }
        }
    }
}
