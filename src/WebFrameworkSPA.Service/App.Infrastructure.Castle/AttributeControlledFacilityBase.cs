using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.Core.Interceptor;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using App.Common;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;

namespace App.Infrastructure.Castle
{
    public abstract class AttributeControlledFacilityBase : AbstractFacility {
        private readonly Type interceptorType;
        private readonly LifestyleType lifestyleType;

        public AttributeControlledFacilityBase(Type interceptorType, LifestyleType lifestyleType) {
            Check.IsNotNull(interceptorType, "interceptorType");

            this.interceptorType = interceptorType;
            this.lifestyleType = lifestyleType;
        }

        protected override void Init() {
            Kernel.Register(Component.For<IInterceptor>().ImplementedBy(interceptorType).Named(interceptorType.Name).LifeStyle.Is(lifestyleType));//AddComponent(interceptorType.Name, typeof(IInterceptor), interceptorType, lifestyleType);
            Kernel.ComponentRegistered += KernelComponentRegistered;
        }

        private void KernelComponentRegistered(string key, IHandler handler) {
            AddInterceptorIfNeeded(handler, GetAttributes(handler));
        }

        protected abstract List<Attribute> GetAttributes(IHandler handler);

        private bool AddInterceptorIfNeeded(IHandler handler, List<Attribute> attributes) {
            foreach (Attribute attribute in attributes) {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(interceptorType.Name));
                return true;
            }

            return false;
        }
    }
}