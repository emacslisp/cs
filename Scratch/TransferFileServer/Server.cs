﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace TransferFileServer
{
    public partial class Server : Form
    {
        Thread t1 = null;
        int flag = 0;
        string receivedPath = "yok";
        public delegate void MyDelegate();
        public event EventHandler<TEventArgs> eventArgs;

        public Server()
        {
            InitializeComponent();
            eventArgs += Event_Method;
        }

        public class StateObject
        {
            // Client socket.
            public Socket workSocket = null;

            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
        }

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public void StartListening()
        {
            byte[] bytes = new Byte[1024];
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 9050);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(ipEnd);
                listener.Listen(100);
                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();

                }
            }
            catch (Exception ex)
            { }
        }

        public void AcceptCallback(IAsyncResult ar)
        {

            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);


            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
            flag = 0;
        }

        public void ReadCallback(IAsyncResult ar)
        {

            int fileNameLen = 1;
            String content = String.Empty;
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0)
            {

                if (flag == 0)
                {
                    fileNameLen = BitConverter.ToInt32(state.buffer, 0);
                    string fileName = Encoding.UTF8.GetString(state.buffer, 4, fileNameLen);
                    receivedPath = @"D:\workspace\test\socketTest\" + fileName;
                    flag++;
                    if (this.eventArgs != null)
                    {
                        eventArgs(null, new TEventArgs("Get File" + fileName+" in "+receivedPath));
                    }
                }
                if (flag >= 1)
                {
                    BinaryWriter writer = new BinaryWriter(File.Open(receivedPath, FileMode.Append));
                    if (flag == 1)
                    {
                        writer.Write(state.buffer, 4 + fileNameLen, bytesRead - (4 + fileNameLen));
                        flag++;
                    }
                    else
                        writer.Write(state.buffer, 0, bytesRead);
                    writer.Close();
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }

            }
            else
            {
                Invoke(new MyDelegate(LabelWriter));
            }

        }

        public void LabelWriter()
        {
            //label1.Text = "Data has been received";
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            t1.Abort();
        }

        private void Server_Load(object sender, EventArgs e)
        {

        }

        private void Event_Method(object sender, TEventArgs e)
        {
            label1.Text = e.Info;
        }

        private void btnServerStart_Click(object sender, EventArgs e)
        {
            if (t1 == null)
            {
                t1 = new Thread(new ThreadStart(StartListening));
                t1.Start();
            }
        }
    }
}
