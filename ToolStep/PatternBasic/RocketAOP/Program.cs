using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketAOP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize the dependency resolver
            //初始化依赖解析器
            DependencyResolver.Initialize();

            //resolve the type:Rocket
            var rocket = DependencyResolver.For<IRocket>();

            //method call
            try
            {
                rocket.Launch(5);
            }
            catch (Exception ex)
            {

            }
            System.Console.ReadKey();
        }
    }
}
