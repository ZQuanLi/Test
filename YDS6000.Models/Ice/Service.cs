using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataProcess;
using Ice;

namespace YDS6000.Models
{
    public class ProcessSevice
    {
        private static bool run = false;
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="iceObj"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool Start(Ice.Object iceObj, int port = 10000)
        {
            if (iceObj == null)
                throw new ApplicationException("Invalid obj");
            if (run == false)
            {
                Ice.Communicator ic = null;
                //初始化Ice runtime ,将args传递给这个调用，是因为服务器可能
                //有run time感兴趣的命令行参数，返回的是一个Ice.Communicator
                //它是Ice run time的主句柄
                ic = Ice.Util.initialize();
                //调用createObjectAdapterWithEndpoints创建一个对象适配器，传入的参数为SimplePrinter(适配器名称）
                //和default -p 10000或者是适配器使用缺省协议TCP/IP在端口10000侦听到来的请求
                Ice.ObjectAdapter adapter = ic.createObjectAdapterWithEndpoints("SimplePrinter", "default -p " + port);
                //这时，服务器端run time 已经初始化，我们实例化一个PrinterI 对
                //象，为我们的Printer 接口创建一个servant。
                //Ice.Object obj = new PrinterI();
                //我们调用适配器的add，告诉它有了一个新的servant ；传给add 的参
                //数是我们刚才实例化的servant，再加上一个标识符。在这里，
                //"SimplePrinter" 串是servant 的名字（如果我们有多个打印机，每个
                //打印机都会有不同的名字，更正确的说法是，都会有不同的对象标识）。
                adapter.add(iceObj, Ice.Util.stringToIdentity("SimplePrinter"));
                //我们调用适配器的activate 方法激活适配器（适配器一开
                //始是在扣留（holding）状态创建的；这种做法在下面这样的情况下很
                //有用：如果我们有多个servant，它们共享同一个适配器，而在所有
                //servant 实例化之前我们不想处理请求）。一旦适配器被激活，服务器就
                //会开始处理来自客户的请求。
                adapter.activate();
                run = true;
            }
            return run;
        }

        /// <summary>
        /// 服务对象
        /// </summary>
        private static communicationPrx service = null;

        /// <summary>
        /// 启动客户端并得到服务对象
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static communicationPrx Client(int port = 10000)
        {
            if (service == null)
            {
                //string[] args = null;
                //int status = 0;
                Ice.Communicator ic = null;
                //调用Ice::initialize 初始化Ice run time。
                ic = Ice.Util.initialize();
                //
                //获取远地打印机的代理。我们调用通信器的stringToProxy
                //创建一个代理，所用参数是"SimplePrinter:default -
                //p 10000"。注意，这个串包含的是对象标识和服务器所用的端口号
                Ice.ObjectPrx obj = ic.stringToProxy("SimplePrinter:default -p " + port);
                //stringToProxy 返回的代理的类型是Ice::ObjectPrx，这种类型
                //位于接口和类的继承树的根部。但要实际与我们的打印机交谈，我们需
                //要的是Printer 接口、而不是Object 接口的代理。为此，我们需要调
                //用PrinterPrxHelper.checkedCast 进行向下转换。这个方法会
                //发送一条消息给服务器，实际询问“这是Printer 接口的代理吗？”如
                //果是，这个调用就会返回Printer 的一个代理；如果代理代表的是其他
                //类型的接口，这个调用就会返回一个空代理。
                service = communicationPrxHelper.checkedCast(obj);
                //测试向下转换是否成功，如果不成功，就抛出出错消息，终止客户。
                if (service == null)
                    throw new ApplicationException("Invalid proxy");
                //我们的地址空间里有了一个活的代理，可以调用printString 方法，
                //把享誉已久的 "Hello World!" 串传给它。服务器会在它的终端上打印这个串。
            }
            return service;
        }
    }
}
