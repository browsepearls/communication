using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CCPOUT
{
    /* key Param1 Param2 Param3 ... Param(N-1) ParamN\r\n
     * 
     */
    internal class MyFilter : TerminatorReceiveFilter<StringPackageInfo>
    {
        public MyFilter():base(Encoding.ASCII.GetBytes("\r\n"))
        {

        }

        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            string body = bufferStream.ReadString((int)bufferStream.Length, Encoding.ASCII);
            body = body.Replace("\r\n", "");
            string[] entities = body.Split();
            string key = entities[0];
            return new StringPackageInfo(key, body, entities);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            EasyClient client = new EasyClient();
            client.Initialize(new MyFilter(), (request) =>
            {
                // handle the received request
                Console.WriteLine(request.Key);
            });
        }
    }

    public class MyClient
    {
        private EasyClient client = new EasyClient();

        public MyClient()
        {
            client.Closed += Client_Closed;
            client.Connected += Client_Connected;
            client.Error += Client_Error;
        }

        public void Connect()
        {
            Task<bool> task = client.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1222));
            bool res = task.Result;
            client.Initialize(new MyFilter(), new Action<StringPackageInfo>(OnReceived));
        }

        public void Send()
        {
            client.Send();
        }

        private void OnReceived(StringPackageInfo stringPackageInfo)
        {

        }

        #region MyRegion
        private void Client_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            EasyClient client = (sender as EasyClient);
            client.Send(Encoding.ASCII.GetBytes($"{client.LocalEndPoint.ToString()}成功连接服务器!"));
        }

        private void Client_Closed(object sender, EventArgs e)
        {
            EasyClient client = (sender as EasyClient);
            client.Send(Encoding.ASCII.GetBytes($"{client.LocalEndPoint.ToString()}已关闭!"));
        }
        #endregion
    }

    public class Message
    {
        public string Key { get; set; }
        public string[] Parameters { get; set; }
    }
}
