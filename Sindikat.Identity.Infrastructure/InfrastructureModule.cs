﻿using System.Reflection;
using Autofac;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Infrastructure.Auth;
using Module = Autofac.Module;

namespace Sindikat.Identity.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Factory")).SingleInstance();
            builder.RegisterType<JwtFactory>().As<IJwtFactory>();
        }
    }
}