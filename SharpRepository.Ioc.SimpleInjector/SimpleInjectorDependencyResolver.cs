using SharpRepository.Repository.Ioc;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRepository.Ioc.SimpleInjector
{
    public class SimpleInjectorDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly Container _container;

        public SimpleInjectorDependencyResolver(Container container)
        {
            _container = container;
        }

        protected override T ResolveInstance<T>()
        {
           return (T)ResolveInstance(typeof(T));
        }

        protected override object ResolveInstance(Type type)
        {
            return _container.GetInstance(type); ;
        }
    }

}
