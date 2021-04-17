using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Networking
{
    public class Client
    {
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 50042;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        private static readonly Socket clientSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public static bool initClient(string ipAddress)
        {
            try
            {
                //Console.Title = "Client 1";
                connectToServer(ipAddress);
                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket);

                Thread sendThread = new Thread(() =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        sendRequest();
                        Thread.Sleep(1000);
                    }
                    
                });
                sendThread.Start();

            } 
            catch (Exception e)
            {
                Debug.Log("Client could not start");
                throw e;
            }
            
            return true;
        }
        

        /// <summary>
        /// Try to connect to the server socket via user input ip address.
        /// Blocks for that time.
        /// </summary>
        private static bool connectToServer(string ipAddress)
        {
            int attempts = 0;
            
            //Console.WriteLine("Please enter ip address of host: ");
            //string ipAddress = Console.ReadLine();
            //string ipAddress = "127.0.0.1";
            
            //TODO: dont let the client end up in an endless connection loop trying to connect to the wrong address (attempts limit? + exception)
            while(!clientSocket.Connected) {
                try
                {
                    attempts++;
                    Debug.Log("Client: Connection attempts: " + attempts);
                    clientSocket.Connect(IPAddress.Parse(ipAddress), PORT);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            } 
            
            //Console.Clear();
            Debug.Log("Client: Connected");
            return true;
        }
        
        
        /// <summary>
        /// Sends a request to the server
        /// </summary>
        private static void sendRequest()
        {
            Debug.Log("Client: Send a request: (Hallo Welt)");
            //string request = Console.ReadLine();
            string request = "Hallo Welt!";
            
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, sendCallback, clientSocket);
            
            //TODO: send game data and messages -> may send JSON
        }

        
        /// <summary>
        /// Callback method is called when the client has finished sending data.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void sendCallback(IAsyncResult AR)
        {
            clientSocket.EndSend(AR);
        }
        
        
        /// <summary>
        /// Callback method is called in case of data being sent to the client.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void receiveCallback(IAsyncResult AR) {
            Socket currentServerSocket = (Socket) AR.AsyncState;
            int receivedBufferSize;

            try
            {
                receivedBufferSize = currentServerSocket.EndReceive(AR);
            }
            catch (SocketException)
            {
                Debug.Log("Client: Server forcefully disconnected");
                //todo handle connection loss
                return;
            }
            
            byte[] receievedBuffer = new byte[receivedBufferSize];
            Array.Copy(buffer, receievedBuffer, receivedBufferSize);
            //may recieve JSON
            string receievedText = Encoding.ASCII.GetString(receievedBuffer);
            Debug.Log("Client: Incoming Data: " + receievedText);
            
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket); // start listening again
        }
    }
}