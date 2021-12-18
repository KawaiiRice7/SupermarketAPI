using System;
using Unity;
using System.Collections.Generic;

namespace Common
{
    public static class ObjectFactory
    {
        private static UnityContainer container = new UnityContainer();
        public static void RegisterInstance<T>(T obj)
        {
            container.RegisterInstance<T>(obj);
        }

        public static T GetInstance<T>()
        {
            return container.Resolve<T>();
        }
    }
}
