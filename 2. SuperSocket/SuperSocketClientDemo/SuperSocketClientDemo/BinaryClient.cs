using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;

namespace SuperSocketClientDemo
{
    public class BinaryPackageInfo : IPackageInfo
    {
        public BinaryPackageInfo() { }

        public BinaryPackageInfo(Int32 msgType,Int32 commandId,UInt32 errorCode,UInt32 channelId,params float[] parameters)
        {
            this.MsgType = msgType;
            this.CommandId = commandId;
            this.ErrorCode = errorCode;
            this.ChannelId = channelId;
            if(parameters.Length > 5)
            {
                throw new ArgumentException("The count of the param must not be more than 5.");
            }
            switch (parameters.Length)
            {
                case 1:
                    Param1 = parameters[0];
                    break;
                case 2:
                    Param1 = parameters[0];
                    Param2 = parameters[1];
                    break;
                case 3:
                    Param1 = parameters[0];
                    Param2 = parameters[1];
                    Param3 = parameters[2];
                    break;
                case 4:
                    Param1 = parameters[0];
                    Param2 = parameters[1];
                    Param3 = parameters[2];
                    Param4 = parameters[3];
                    break;
                case 5:
                    Param1 = parameters[0];
                    Param2 = parameters[1];
                    Param3 = parameters[2];
                    Param4 = parameters[3];
                    Param5 = parameters[4];
                    break;
            }
        }

        public Int32 MsgType { get; set; }
        public Int32 CommandId { get; set; }
        public float Param1 { get; set; }
        public float Param2 { get; set; }
        public float Param3 { get; set; }
        public float Param4 { get; set; }
        public float Param5 { get; set; }
        public UInt32 ErrorCode { get; set; }
        public UInt32 ChannelId { get; set; }

        private byte[] message;
        public byte[] Message
        {
            get
            {
                if (message == null || message.Length != 32)
                {
                    message = new byte[32];
                }
                return message;
            }
            set
            {
                if(message == null || message.Length != 32)
                {
                    throw new ArgumentException("The length of the message represented by a binary array must be 32.");
                }
            }
        }

        public byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(this.MsgType);
            bw.Write(this.CommandId);
            bw.Write(this.Param1);
            bw.Write(this.Param2);
            bw.Write(this.Param3);
            bw.Write(this.Param4);
            bw.Write(this.Param5);
            bw.Write()
            byte[] bytes = new byte[68];

            return bytes;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class ReqBinaryPackageInfo: BinaryPackageInfo
    {
        public ReqBinaryPackageInfo() : base() { }
    }

    public class RspBinaryPackageInfo : BinaryPackageInfo
    {
        public RspBinaryPackageInfo() : base() { }
    }

    public class BinaryFilter : FixedSizeReceiveFilter<BinaryPackageInfo>
    {
        public BinaryFilter(int size) : base(size) { }

        public override BinaryPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            return new BinaryPackageInfo()
            {

            }
        }
    }

    class BinaryClient:EasyClient<>
    {
        protected override void HandlePackage(IPackageInfo package)
        {
            base.HandlePackage(package);
        }

    }
}
