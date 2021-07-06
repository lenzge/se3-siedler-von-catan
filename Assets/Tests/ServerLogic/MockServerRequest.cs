using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Enums;
using Networking.Interfaces;
using NUnit.Framework;

namespace Tests.ServerLogic
{
    /// <summary>
    /// All Methods in here are called indirectly from ServerReceive.
    /// All Parameters of Methods are saved in a field to be Asserted from ServerReceiveTest.
    /// </summary>
    public class MockServerRequest : ServerToClientCommunication
    {
        // List of all Parameters that can be passed to any method in MockServerRequest
        public static ArrayList notifyClientJoinedPlayerInformation;
        public static string notifyClientJoinedLobbyIP;
        
        public static Hexagon[][] gamestartInitializeGameBoard;


        public static int notifyNextPlayerPlayerIndex;
        public static int notifyNextPlayerPreviousPlayerIndex;
        
        public static int notifyPlayerReadyCurrentClientID;
        public static string notifyPlayerReadyPlayerName;
        public static bool notifyPlayerReadyReadyStatus;

        public void notifyClientJoined(ArrayList playerInformation, string lobbyIP)
        {
            notifyClientJoinedPlayerInformation = playerInformation;
            notifyClientJoinedLobbyIP = lobbyIP;
        }

        public void gamestartInitialize(Hexagon[][] gameBoard)
        {
            gamestartInitializeGameBoard = gameBoard;
        }

        public void notifyObjectPlacement(BUYABLES buildType, int buildID, PLAYERCOLOR color)
        {
            throw new System.NotImplementedException();
        }

        public void notifyNextPlayer(int playerIndex, int previousPlayerIndex)
        {
            notifyNextPlayerPlayerIndex = playerIndex;
            notifyNextPlayerPreviousPlayerIndex = previousPlayerIndex;
        }

        public void notifyVictory(string playerName, PLAYERCOLOR playerColor)
        {
            throw new System.NotImplementedException();
        }

        public void notifyClientDisconnect(string playerName, PLAYERCOLOR playerColor)
        {
            throw new System.NotImplementedException();
        }

        public void notifyRejection(int playerID, string errorMessage)
        {
            throw new System.NotImplementedException();
        }

        public void notifyPlayerReady(int currentClientID, string playerName, bool readyStatus)
        {
            notifyPlayerReadyCurrentClientID = currentClientID;
            notifyPlayerReadyPlayerName = playerName;
            notifyPlayerReadyReadyStatus = readyStatus;
        }

        public void notifyRollDice(int[] diceResult)
        {
            throw new System.NotImplementedException();
        }

        public void acceptBuyDevelopement(int leftDevCards)
        {
            throw new System.NotImplementedException();
        }

        public void notifyAcceptPlayDevelopement(int playerID, DEVELOPMENT_TYPE developmentCard, string playerName)
        {
            throw new System.NotImplementedException();
        }

        public void notifyAcceptTradeOffer(int playerID, int buttonNumber)
        {
            throw new System.NotImplementedException();
        }

        public void updateOwnPlayer(int[] updateLeftBuildings, Dictionary<RESOURCETYPE, int> updateResources, Dictionary<DEVELOPMENT_TYPE, int> updateDevCards, int playerID)
        {
            throw new System.NotImplementedException();
        }

        public void updateRepPlayers(int[][] updateNumbers)
        {
            throw new System.NotImplementedException();
        }
    }
}