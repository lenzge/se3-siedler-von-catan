using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Enums;
using UnityEngine;
using Networking.Package;

namespace Networking.ClientSide
{
    public class Client
    {
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 50042;
        private static byte[] buffer;

        private static Socket clientSocket;

        private static ClientReceive _clientReceive;


        /// <summary>
        /// Initializes a client instance and tries to connect to the game server using the given IP address.
        /// After 5 attempts the connecting attempts are aborted.
        /// </summary>
        /// <param name="ipAddress">IP address of the server</param>
        /// <returns>true if connection was established successfully. Otherwise false.</returns>
        /// <exception cref="Exception"></exception>
        public static bool initClient(string ipAddress)
        {
            _clientReceive = GameObject.FindWithTag("ClientReceive").GetComponent<ClientReceive>();
                        
            buffer = new byte[BUFFER_SIZE];
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                bool connectionSuccess = connectToServer(ipAddress);
                if (!connectionSuccess)
                {
                    return false;
                }

                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket);
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
        /// <param name="ipAddress">IP address of the server</param>
        /// <returns>true if connection was established successfully. Otherwise false.</returns>
        private static bool connectToServer(string ipAddress)
        {
            int attempts = 0;

            while (!clientSocket.Connected && attempts < 5)
            {
                try
                {
                    attempts++;
                    Debug.Log("Client: Connection attempts: " + attempts);
                    clientSocket.Connect(IPAddress.Parse(ipAddress), PORT);
                }
                catch (SocketException)
                {
                    Debug.Log("Client: Connection Error");
                }
            }

            if (attempts >= 5)
            {
                Debug.Log("Client: Failed connecting to Server!");
                return false;
            }

            Debug.Log("Client: Connected");
            return true;
        }


        /// <summary>
        /// Sends a request to the server.
        /// </summary>
        /// <param name="request">Data to send</param>
        public static void sendRequest(string request)
        {
            Debug.Log("Client: Sending a request" + request);

            byte[] buffer = Encoding.ASCII.GetBytes(request);
            clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, sendCallback, clientSocket);
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
        private static void receiveCallback(IAsyncResult AR)
        {
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
            Packet serverData = PacketSerializer.jsonToObject(Encoding.ASCII.GetString(receievedBuffer));

            delegateIncomingDataToMethods(serverData);

            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback,
                clientSocket); // start listening again
        }


        /// <summary>
        /// map the incoming data by its type to a handle method
        /// </summary>
        /// <param name="incomingData">received data from server</param>
        private static void delegateIncomingDataToMethods(Packet incomingData)
        {
            try
            {
                switch (incomingData.type)
                {
                    case (int) COMMUNICATION_METHODS.HANDLE_CLIENT_JOINED:
                        // todo: eliminate race condition!!!
                        // while (SceneManager.GetActiveScene().name != "Lobby") ; //Waiting for Unity to load lobby
                        
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleClientJoined(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_GAMESTART_INITIALIZE:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleGameStartInitialize(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY_NOTIFICATION:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handlePlayerReadyNotification(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_OBJECT_PLACEMENT:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleObjectPlacement(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_NEXT_PLAYER:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleNextPlayer(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_VICTORY:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleVictory(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_CLIENT_DISCONNECT:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleClientDisconnect(incomingData);
                        });
                        break;


                    case (int) COMMUNICATION_METHODS.HANDLE_REJECTION:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleRejection(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BEGIN_ROUND:
                        Debug.Log("before calling Threadmanager");
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAccpetBeginRound(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_TRADE_BANK:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAcceptTradeBank(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BUILD:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAcceptBuild(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_GET_RESOURCES:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleGetResources(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BUY_DEVELOPMENT_CARD:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAcceptBuyDevelopement(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_PLAY_DEVELOPMENT_CARD:
                        ThreadManager.executeOnMainThread(() =>
                        {  
                            _clientReceive.handleAcceptPlayDevelopement(incomingData);
                        });
                        break;

                    default:
                        Debug.LogWarning($"there was no target method send, invalid data packet. Packet Type: {incomingData.type}");
                        // TODO: trow exception!!!
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
            string receievedText = PacketSerializer.objectToJsonString(incomingData);
            Debug.Log("Client: Incoming Data: " + receievedText);
        }
    }
}