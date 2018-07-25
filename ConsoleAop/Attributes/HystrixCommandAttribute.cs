using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace ConsoleAop.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HystrixCommandAttribute : AbstractInterceptorAttribute
    {
        private string fallBackMethod;

        public HystrixCommandAttribute(string fallBackMethod)
        {
            this.fallBackMethod = fallBackMethod;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("HystrixCommand Catch ex: " + ex.Message);
                var fallbackMethod = context.Implementation.GetType().GetMethod(fallBackMethod);
                var returnValue = fallbackMethod.Invoke(context.Implementation, context.Parameters);
                context.ReturnValue = returnValue;
            }
        }
    }
}