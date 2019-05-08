using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using CaisseReservationServer;

namespace CaisseReservationServer
{
    /// <summary>
    /// A class that listens on a port for client connections, sends and recieves messages from connected clients,
    /// and periodically broadcasts UDP messages.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// A delegate type called when a client initially connects to the server.  Void return type.
        /// </summary>
        /// <param name="clientNumber">A unique identifier of the client that has connected to the server.</param>
        public delegate void ClientConnectCallback(int clientNumber);

        /// <summary>
        /// A delegate type called when a client disconnects from the server.  Void return type.
        /// </summary>
        /// <param name="clientNumber">A unique identifier of the client that has disconnected from the server.</param>
        public delegate void ClientDisconnectCallback(int clientNumber);

        /// <summary>
        /// A delegate type called when the server receives data from a client.
        /// </summary>
        /// <param name="clientNumber">A unique identifier of the client that has disconnected from the server.</param>
        /// <param name="message">A byte array representing the message sent.</param>
        /// <param name="messageSize">The size in bytes of the message.</param>
        public delegate void ReceiveDataCallback(int clientNumber, byte[] message, int messageSize);

        private ClientConnectCallback _clientConnect = null;
	    private ReceiveDataCallback _receive = null;

        private Socket _mainSocket;
        private System.Threading.Timer _broadcastTimer;
        private int _currentClientNumber = 0;

       // public Dictionary<int, Socket > workerSockets = new Dictionary<int, Socket>();
        public Dictionary<int, UserSock> workerSockets = new Dictionary<int, UserSock>();
        

        /// <summary>
        /// Modify the callback function used when a client initially connects to the server.
        /// </summary>
        public ClientConnectCallback OnClientConnect
        {
            get => _clientConnect;

	        set => _clientConnect = value;
        }

        /// <summary>
        /// Modify the callback function used when a client disconnects from the server.
        /// </summary>
        public ClientDisconnectCallback OnClientDisconnect { get; set; } = null;

	    /// <summary>
        /// Whether or not the server is currently listening for new client connections.
        /// </summary>
        public bool IsListening => _mainSocket != null && _mainSocket.IsBound;

	    /// <summary>
        /// Make the server listen for client connections on a specific port.
        /// </summary>
        /// <param name="listenPort">The number of the port to listen for connections on.</param>
        public void Listen(int listenPort)
        {
            try
            {
                Stop();

                _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _mainSocket.Bind(new IPEndPoint(IPAddress.Any, listenPort));
                _mainSocket.Listen(100);
                _mainSocket.BeginAccept(OnReceiveConnection, null);
            }

            catch (SocketException se)
            {
                System.Console.WriteLine(se.Message);
            }
        }

        /// <summary>
        /// Stop listening for new connections and close all currently open connections.
        /// </summary>
        public void Stop()
        {
            lock (workerSockets)
            {
                foreach (var s in workerSockets.Values)
                {
                    if (s.UserSocket.Connected)
                        s.UserSocket.Close();
                }
                workerSockets.Clear();
            }

            if (IsListening)
                _mainSocket.Close();
        }

        /// <summary>
        /// Send a message to all connected clients.
        /// </summary>
        /// <param name="message">A byte array representing the message to send.</param>
        //public void SendMessage(byte[] message)
        //{
        //    try
        //    {
        //        foreach (UserSock s in workerSockets.Values)
        //        {
        //            if (s.UserSocket.Connected)
        //            {
        //                try
        //                {
        //                    s.UserSocket.Send(message);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //    catch (SocketException se)
        //    {
        //        System.Console.WriteLine(se.Message);
        //    }
        //}

        public void SendMessage(byte[] message, bool testConnections = false)
        {
            if (testConnections)
            {
                var ClientsToRemove = new List<int>();
                foreach (var clientId in workerSockets.Keys)
                {
                    if (workerSockets[clientId].UserSocket.Connected)
                    {
                        try
                        {
                            workerSockets[clientId].UserSocket.Send(message);
                        }
                        catch
                        {
                            ClientsToRemove.Add(clientId);
                        }

                        Thread.Sleep(10);// this is for a client Ping so stagger the send messages
                    }
                    else
                        ClientsToRemove.Add(clientId);
                }

                //lock (workerSockets)//Already locked from the caller
                {
                    if (ClientsToRemove.Count > 0)
                    {
                        foreach (var cID in ClientsToRemove)
                        {
	                        //Socket gets closed and removed from OnClientDisconnect
	                        OnClientDisconnect?.Invoke(cID);
                        }
                    }
                }
                ClientsToRemove.Clear();
                ClientsToRemove = null;
            }
            else
            {
                foreach (UserSock s in workerSockets.Values)
                {
                    try
                    {
                        if (s.UserSocket.Connected)
                            s.UserSocket.Send(message);
                    }
                    catch (SocketException se)
                    {
                        System.Console.WriteLine(se.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Send a message to a specific client.
        /// </summary>
        /// <param name="clientNumber">A unique identifier of the client that has connected to the server.</param>
        /// <param name="message">A byte array representing the message to send.</param>
        public void SendMessage(int clientNumber, byte[] message)
        {
            if (!workerSockets.ContainsKey(clientNumber))
            {
                //throw new ArgumentException("Invalid Client Number", "clientNumber");
                Console.WriteLine("Invalid Client Number");
                return;
            }
            try
            {
                //workerSockets[clientNumber].Send(message);
                workerSockets[clientNumber].UserSocket.Send(message);
            }
            catch (SocketException se)
            {
                System.Console.WriteLine(se.Message);
            }
        }

        /// <summary>
        /// Begin broadcasting a message over UDP every several seconds.
        /// </summary>
        /// <param name="message">A byte array representing the message to send.</param>
        /// <param name="port">The port over which to send the message.</param>
        /// <param name="frequency">Frequency to send the message in seconds.</param>
        public void BeginBroadcast(byte[] message, int port, int frequency)
        {
	        var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
	        {
		        EnableBroadcast = true
	        };

	        var pack = new Packet(sock, port) {DataBuffer = message};

	        if (_broadcastTimer != null)
                _broadcastTimer.Dispose();

            _broadcastTimer = new Timer(BroadcastTimerCallback, pack, 0, frequency * 1000);
        }

        /// <summary>
        /// Stop broadcasting UDP messages.
        /// </summary>
        public void EndBroadcast()
        {
            if (_broadcastTimer != null)
                _broadcastTimer.Dispose();
        }

        /// <summary>
        /// A callback called by the broadcast timer.  Broadcasts a message.
        /// </summary>
        /// <param name="state">An object representing the byte[] message to be broadcast.</param>
        private void BroadcastTimerCallback(object state)
        {
            ((Packet)state).CurrentSocket.SendTo(((Packet)state).DataBuffer, new IPEndPoint(IPAddress.Broadcast, ((Packet)state).ClientNumber));
        }

        /// <summary>
        /// An internal callback triggered when a client connects to the server.
        /// </summary>
        /// <param name="asyn"></param>
        private void OnReceiveConnection(IAsyncResult asyn)
        {
            try
            {
                lock (workerSockets)
                {
                    Interlocked.Increment(ref _currentClientNumber); // Thread Safe
                    var us = new UserSock(_currentClientNumber, _mainSocket.EndAccept(asyn));
                    workerSockets.Add(_currentClientNumber, us);
                }

                if (_clientConnect != null)
                    _clientConnect(_currentClientNumber);

                WaitForData(_currentClientNumber);
                _mainSocket.BeginAccept(new AsyncCallback(OnReceiveConnection), null);
            }
            catch (ObjectDisposedException)
            {
                System.Console.WriteLine("OnClientConnection: Socket has been closed");
            }
            catch (SocketException se)
            {
                //Console.WriteLine("SERVER EXCEPTION in OnReceiveConnection: " + se.Message);
                System.Diagnostics.Debug.WriteLine("SERVER EXCEPTION in OnReceiveConnection: " + se.Message);//pe 4-22-2015

                if (workerSockets.ContainsKey(_currentClientNumber))
                {
                    Console.WriteLine("RemoteEndPoint: " + workerSockets[_currentClientNumber].UserSocket.RemoteEndPoint.ToString());
                    Console.WriteLine("LocalEndPoint: " + workerSockets[_currentClientNumber].UserSocket.LocalEndPoint.ToString());

                    Console.WriteLine("Closing socket from OnReceiveConnection");
                }

                //Socket gets closed and removed from OnClientDisconnect
	            OnClientDisconnect?.Invoke(_currentClientNumber);
            }
        }

        /// <summary>
        /// Begins an asynchronous wait for data for a particular client.
        /// </summary>
        /// <param name="clientNumber">A unique identifier of the client that has connected to the server.</param>
        private void WaitForData(int clientNumber)
        {
            if (!workerSockets.ContainsKey(clientNumber))
            {
                //Console.WriteLine("NO KEY: " + clientNumber.ToString());
                return;
            }

            try
            {
                Packet pack = new Packet(workerSockets[clientNumber].UserSocket, clientNumber);
                workerSockets[clientNumber].UserSocket.BeginReceive(pack.DataBuffer, 0, pack.DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), pack);
            }
            catch (SocketException se)
            {
                try
                {
                    //Socket gets closed and removed from OnClientDisconnect
	                OnClientDisconnect?.Invoke(clientNumber);

	                //Console.WriteLine("SERVER EXCEPTION in WaitForClientData: " + se.Message);
                    System.Diagnostics.Debug.WriteLine($"SERVER EXCEPTION in WaitForClientData: {se.Message}");//pe 4-22-2015
                }
                catch { }
            }
            catch (Exception ex)
            {
                //Socket gets closed and removed from OnClientDisconnect
	            OnClientDisconnect?.Invoke(clientNumber);

	            string msg = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                System.Diagnostics.Debug.WriteLine($"SERVER EXCEPTION in WaitForClientData2: {msg}");//pe 5-3-2017
            }
        }

        /// <summary>
        /// An internal callback triggered when the server recieves data from a client.
        /// </summary>
        /// <param name="asyn"></param>
        private void OnDataReceived(IAsyncResult asyn)
        {
            Packet socketData = (Packet)asyn.AsyncState;

            try
            {
                int dataSize = socketData.CurrentSocket.EndReceive(asyn);

                if (dataSize.Equals(0))
                {
                    //System.Diagnostics.Debug.WriteLine($"OnDataReceived datasize is 0, zerocount = {((UserSock)workerSockets[socketData.ClientNumber]).ZeroDataCount}");//pe 5-3-2017

                    if (workerSockets.ContainsKey(socketData.ClientNumber))
                    {
                        if (((UserSock)workerSockets[socketData.ClientNumber]).ZeroDataCount++ == 10)
                        {
                            if (OnClientDisconnect != null)
                                OnClientDisconnect(socketData.ClientNumber);
                        }
                    }
                }
                else
                {
                    //if (_receive != null)
                        _receive(socketData.ClientNumber, socketData.DataBuffer, dataSize);

                    ((UserSock)workerSockets[socketData.ClientNumber]).ZeroDataCount = 0;
                }

                WaitForData(socketData.ClientNumber);
            }
            catch (ObjectDisposedException)
            {
                System.Console.WriteLine("OnDataReceived: Socket has been closed");

                //Socket gets closed and removed from OnClientDisconnect
	            OnClientDisconnect?.Invoke(socketData.ClientNumber);
            }
            catch (SocketException se)
            {
                //10060 - A connection attempt failed because the connected party did not properly respond after a period of time,
                //or established connection failed because connected host has failed to respond.
                if (se.ErrorCode == 10054 || se.ErrorCode == 10060) //10054 - Error code for Connection reset by peer
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("SERVER EXCEPTION in OnClientDataReceived, ServerObject removed:(" + se.ErrorCode.ToString() +  ") " + socketData.ClientNumber + ", (happens during a normal client exit)");
                        System.Diagnostics.Debug.WriteLine("RemoteEndPoint: " + workerSockets[socketData.ClientNumber].UserSocket.RemoteEndPoint.ToString());
                        System.Diagnostics.Debug.WriteLine("LocalEndPoint: " + workerSockets[socketData.ClientNumber].UserSocket.LocalEndPoint.ToString());
                    }
	                catch
	                {
		                // ignored
	                }

	                //Socket gets closed and removed from OnClientDisconnect
					OnClientDisconnect?.Invoke(socketData.ClientNumber);

					Console.WriteLine("Closing socket from OnDataReceived");
                }
                else
                {
                    string mess = "CONNECTION BOOTED for reason other than 10054: code = " + se.ErrorCode.ToString() + ",   " + se.Message;
                    Console.WriteLine(mess);
                    ToFile(mess);
                }
            }
        }

        private void ToFile(string message)
        {
            var AppPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);//CommonClassLibs.GeneralFunction.GetAppPath;//System.Windows.Forms.Application.StartupPath;

			System.IO.StreamWriter sw = null;
            try
            {
                sw = System.IO.File.AppendText(System.IO.Path.Combine(AppPath, "ServerSocketIssue.txt"));
                var logLine = $"{System.DateTime.Now:G}: {message}.";
                sw.WriteLine(logLine);
            }
            catch// (Exception ex)
            {
                //Console.WriteLine("\n\nError in ToFile:\n" + message + "\n" + ex.Message + "\n\n");
               // System.Windows.Forms.MessageBox.Show("ERROR:\n\n" + ex.Message, "Possible Permissions Issue!");
            }
            finally
            {
                try
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw.Dispose();
                    }
                }
                catch
                { }
            }
        }
    }

    
}
