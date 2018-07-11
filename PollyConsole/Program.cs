using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace PollyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAsync().Wait();

            Console.ReadLine();
        }


        static async Task TestAsync()
        {
           var timeoutPoly =  Policy.TimeoutAsync(2, TimeoutStrategy.Pessimistic,
                 (context, timespan, task) =>
                {
                    Console.WriteLine("time out");//No.2
                    return Task.FromResult("");//没啥用
                });
            var fallBackPolicy = Policy<string>.Handle<TimeoutRejectedException>()
                .FallbackAsync(cancelToken =>
                {
                    Console.WriteLine("begin to handle timeout "); ////No.4
                    return Task.FromResult("handled ");
                },  async r =>
                {
                    Console.WriteLine(r.Exception.Message); ////No.3
                });

         var result = await   fallBackPolicy.WrapAsync(timeoutPoly).ExecuteAsync(async () =>
            {
                Console.WriteLine("begin trans");//No.1
                await Task.Delay(3000);
                return "ok";
            });

            Console.WriteLine("Result: "+ result);////No.5


        }

        static void TestTimeout()
        {
            //timeout 策略无法单独使用，必须结合其他策略
          var timeoutPolicy =  Policy.Timeout(2, TimeoutStrategy.Pessimistic);

            var fallBackPolicy = Policy.Handle<Exception>().Fallback(() => { Console.WriteLine("fall back"); });
            //先timeout 处理，然后再 fallback
            var newPolicy = fallBackPolicy.Wrap(timeoutPolicy);
            while (true)
            {
                try
                {
                    newPolicy.Execute(() =>
                    {
                        Console.WriteLine("开始调用");
                        Thread.Sleep(3000);
                        Console.WriteLine("最终还是完成了");
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("调用出错了");
                }

                Thread.Sleep(1000);
            }
        }

        static void TestRetryThreeTimeThenFallback()
        {
            var retryPolicy = Policy.Handle<Exception>().Retry(3);
            var fallBackPolicy = Policy.Handle<Exception>().Fallback(() => { Console.WriteLine("fall back"); });
            //Wrap 是有顺序的，内部的policy 先起作用
            var  retryWithFallback = fallBackPolicy.Wrap(retryPolicy);

            while (true)
            {
                try
                {
                    retryWithFallback.Execute(() =>
                    {
                        Console.WriteLine("开始调用");
                        throw new Exception();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("调用出错了");
                }

                Thread.Sleep(1000);
            }
        }

        static void TestCircuitBreaker()
        {
           var policy = Policy.Handle<Exception>().CircuitBreaker(3, TimeSpan.FromSeconds(5));
            while (true)
            {
                try
                {
                    policy.Execute(() =>
                    {
                        Console.WriteLine("开始调用");
                        throw new Exception();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("调用出错了");
                }
                Thread.Sleep(1000);
            }
        }

        static void Test1()
        {
            Policy<string>.Handle<ArgumentException>().WaitAndRetry(3, i=> TimeSpan.FromMinutes(1));

            Policy<string> policy = Policy<string>.Handle<ArgumentException>().Fallback(ex =>
            {

                Console.WriteLine("执行错误" + ex);
                return "default 降级后返回的值";
            });
            var str = policy.Execute(() => {
                throw new ArgumentException("e");
                return "555";
            });
            Console.WriteLine(str);
        }
    }
}