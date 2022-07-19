using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RadioThermLib
{
    /// <summary>
    /// This class handles the discovery of devices via multicast UDP mechanism described
    /// in the RadioThermostat manual named "Marvell Service Discovery Protocol"
    /// </summary>
    public class MarvellDiscovery : IDisposable
    {
        const string udpMultiCastIp = "239.255.255.250";
        const int udpMultiCastPort = 1900;
        private UdpClient udpClient;
        private IPAddress localIp;
        private readonly int timeout;
        private List<IPAddress> discovered;
        private bool disposedValue;

        public MarvellDiscovery(IPAddress localIp, int timeout)
        {
            this.localIp = localIp;
            this.timeout = timeout;
            this.udpClient = new UdpClient(AddressFamily.InterNetwork);
            this.discovered = new List<IPAddress>();
        }

        public List<IPAddress> DiscoveredDevices { get => discovered; set => discovered = value; }

        /// <summary>
        /// Synchronously discovers devices.
        /// 
        /// Devices that respond to the protocol are added in <see cref="DiscoveredDevices"/>.
        /// </summary>
        /// <remarks>
        /// This will block for 5 seconds until finally a timeout occurs. This should give devices
        /// time to respond to the multicast.
        /// </remarks>
        public void Discover()
        {
            const string discoverStr = "TYPE: WM-DISCOVER\r\nVERSION: 1.0\r\n\r\nservices:com.marvell.wm.system*\r\n\r\n";

            SendMessage(discoverStr);

            Receive();
        }

        private void SendMessage(string message)
        {
            var data = Encoding.Default.GetBytes(message);
            
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.ExclusiveAddressUse = false;

            // ttl is 3 according to RadioTherm sample code
            udpClient.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 3);
            
            // bind the socket
            udpClient.Client.Bind(new IPEndPoint(localIp, 0));

            var multicastIpAddress = IPAddress.Parse(udpMultiCastIp);
            var multicastEndPoint = new IPEndPoint(multicastIpAddress, udpMultiCastPort);

            // JoinMulticastGroup did not work....
            //udpClient.JoinMulticastGroup(address);

            // this here worked....
            var mcastOpt = new MulticastOption(multicastIpAddress, localIp);
            udpClient.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOpt);

            udpClient.Send(data, data.Length, multicastEndPoint);
        }

        private void Receive()
        {
            var endPoint = new IPEndPoint(IPAddress.Any, 0);

            // many devices may respond and this should read them all
            while (true)
            {
                udpClient.Client.ReceiveTimeout = this.timeout;
                try
                {
                    var data = udpClient.Receive(ref endPoint);
                    //var msg = Encoding.Default.GetString(data);
                    var remoteIp = endPoint.Address;
                    discovered.Add(remoteIp);
                }
                catch (SocketException sex)
                {
                    // swallow our timeout we just added. re-throw everything else
                    if (sex.SocketErrorCode == SocketError.TimedOut)
                        break;
                    throw;
                }
            }
        }

        #region IDispose Impl
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    udpClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MarvellDiscovery()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        } 
        #endregion
    }
}
