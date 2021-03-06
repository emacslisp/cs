﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSFtp_CLI.ftp
{
    class SocketHelpers
    {
        public SocketHelpers()
        {
        }

        static public bool Send(TcpClient socket, byte[] abMessage)
        {
            return Send(socket, abMessage, 0, abMessage.Length);
        }

        static public bool Send(TcpClient socket, byte[] abMessage, int nStart)
        {
            return Send(socket, abMessage, nStart, abMessage.Length - nStart);
        }

        static public bool Send(TcpClient socket, byte[] abMessage, int nStart, int nLength)
        {
            /*bool fReturn = true;

            try
            {
                BinaryWriter writer = new BinaryWriter(socket.GetStream());
                writer.Write(abMessage, nStart, nLength);
                writer.Flush();
            }
            catch (IOException)
            {
                fReturn = false;
            }
            catch (SocketException)
            {
                fReturn = false;
            }

            return fReturn;*/
            Console.WriteLine(Encoding.ASCII.GetString(abMessage));
            return true;
        }

        static public bool Send(TcpClient socket, string sMessage)
        {
            byte[] abMessage = System.Text.Encoding.ASCII.GetBytes(sMessage);
            return Send(socket, abMessage);
        }

        static public void Close(TcpClient socket)
        {
            
        }

        static public TcpClient CreateTcpClient(string sAddress, int nPort)
        {
            TcpClient client = null;

            try
            {
                client = new TcpClient(sAddress, nPort);
            }
            catch (SocketException)
            {
                client = null;
            }

            return client;
        }

        static public TcpListener CreateTcpListener(int nPort)
        {
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, nPort);
            }
            catch (SocketException)
            {
                listener = null;
            }

            return listener;
        }

        static public IPAddress GetLocalAddress()
        {
            IPHostEntry hostEntry = Dns.Resolve(Dns.GetHostName());

            if (hostEntry == null || hostEntry.AddressList.Length == 0)
            {
                return null;
            }

            return hostEntry.AddressList[0];
        }
    }
}
