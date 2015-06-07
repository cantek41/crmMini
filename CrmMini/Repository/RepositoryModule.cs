using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using CrmMini.Models;

namespace CrmMini.Repository
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("CrmMini"))
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
   
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterType(typeof(VdbSoftEntities)).As(typeof(DbContext)).InstancePerLifetimeScope();
           // builder.RegisterType(typeof(Repository)).As(typeof(IUnitOfWork)).InstancePerRequest();
        }
    }
}