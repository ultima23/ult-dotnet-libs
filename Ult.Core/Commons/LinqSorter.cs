using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;



namespace Ult.Core.Commons
{
    public class LinqSorter<E>
    {

        private readonly IQueryable<E> unsorted;
        private readonly FirstPasses firstPasses = new FirstPasses();
        private readonly NextPasses nextPasses = new NextPasses();



        public LinqSorter(IQueryable<E> unsorted)
        {
            this.unsorted = unsorted;
        }




        private IOrderedQueryable<E> SortFirst(string name, IQueryable<E> source)
        {
            return firstPasses[name].Invoke(source);
        }

        private IOrderedQueryable<E> SortNext(string name, IOrderedQueryable<E> source)
        {
            return nextPasses[name].Invoke(source);
        }


        public void Define<P>(string name, Expression<Func<E, P>> selector)
        {
            firstPasses.Add(name, s => s.OrderBy(selector));
            nextPasses.Add(name, s => s.ThenBy(selector));
        }

        public IOrderedQueryable<E> SortBy(params string[] names)
        {
            IOrderedQueryable<E> result = null;

            foreach (var name in names)
            {
                result = result == null
                             ? SortFirst(name, unsorted)
                             : SortNext(name, result);
            }

            return result;
        }





        private class FirstPasses : Dictionary<string, Func<IQueryable<E>, IOrderedQueryable<E>>> { }
        private class NextPasses : Dictionary<string, Func<IOrderedQueryable<E>, IOrderedQueryable<E>>> { }

    }
}
