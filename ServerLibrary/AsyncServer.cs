using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerLibrary
{
    public class AsyncServer : AbstractServer
    {
        Regex regexLogin = new Regex(@"[a-zA-Z0-9]+\s[a-zA-Z0-9]+");
        Regex regexRegister = new Regex(@"register\s[a-zA-Z0-9]+\s[a-zA-Z0-9]+");
        Regex regexDisconnect = new Regex(@"disconnect");
        Regex regexLogout = new Regex(@"logout");
        bool logged = false;

        public delegate void TransmissionDataDelegate(NetworkStream nStream);
        public AsyncServer(IPAddress IP, int port) : base(IP, port)
        {
        }
        protected override void AcceptClient()
        {
            while (true)
            {
                tcpClient = TcpListener.AcceptTcpClient();
                stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);
                transmissionDelegate.BeginInvoke(stream, TransmissionCallback, tcpClient);
            }
        }

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[buffer_size];
            
            string hello = "Login status: "+logged+"\n\rTo log in just type your login and password separated by space.\n\rTo register type: \"register user password\"\n\r" +
                "To disconnect type: \"disconnect\"\n\r";
            var helloB = Encoding.UTF8.GetBytes(hello);
            stream.Write(helloB, 0, helloB.Length);
            while (true)
            {
                string[] users = System.IO.File.ReadAllLines(@"users.txt");
                try
                {   
                    
                    stream.Read(buffer, 0, buffer_size);
                    string req = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                    Match login = regexLogin.Match(req);
                    Match register = regexRegister.Match(req);
                    Match disconnect = regexDisconnect.Match(req);
                    Match logout = regexLogout.Match(req);
                    if (register.Success)
                    {
                        bool userExists = false;
                        foreach (string user in users)
                        {
                            if (req.Split(' ')[1] + " " + req.Split(' ')[2] == user)
                            {
                                hello = "We have already this user in our system!\n\r";
                                helloB = Encoding.UTF8.GetBytes(hello);
                                stream.Write(helloB, 0, helloB.Length);
                                userExists = true;
                                break;
                            }
                        }
                        if(!userExists)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"users.txt", true))
                            {
                                file.WriteLine(req.Split(' ')[1] +" "+ req.Split(' ')[2]);
                            }
                            hello = "User has been added to the system!\n\r";
                            helloB = Encoding.UTF8.GetBytes(hello);
                            stream.Write(helloB, 0, helloB.Length);
                            userExists = true;
                        }
                    }
                    else if (login.Success)
                    {
                        if (logged)
                        {
                            hello = "You are already logged in! To log out type \"logout\".\n\r";
                            helloB = Encoding.UTF8.GetBytes(hello);
                            stream.Write(helloB, 0, helloB.Length);
                        }
                        else
                        {
                            foreach (string user in users)
                            {
                                Console.WriteLine(user);
                                if (user.Contains(req))
                                {
                                    logged = true;
                                    hello = "Success!\n\r";
                                    helloB = Encoding.UTF8.GetBytes(hello);
                                    stream.Write(helloB, 0, helloB.Length);
                                    break;
                                }
                            }
                            if (!logged)
                            {
                                hello = "No such user!\n\r";
                                helloB = Encoding.UTF8.GetBytes(hello);
                                stream.Write(helloB, 0, helloB.Length);
                            }
                        }
                    }
                    else if (disconnect.Success)
                    {
                        logged = false;
                        hello = "Disconnecting...\n\r";
                        helloB = Encoding.UTF8.GetBytes(hello);
                        stream.Write(helloB, 0, helloB.Length);
                        tcpClient.Close();
                        break;
                    }
                    else if (logout.Success)
                    {
                        logged = false;
                    }
                    hello = "Login status: " + logged + "\n\n\r";
                    helloB = Encoding.UTF8.GetBytes(hello);
                    stream.Write(helloB, 0, helloB.Length);
                    
                    buffer = new byte[buffer_size];
                }
                catch (IOException e)
                {
                    break;
                }
            }
        }
        private void TransmissionCallback(IAsyncResult ar)
        {
        }
        /// <summary>
        /// Overrided comment.
        /// </summary>
        public override void Start()
        {
            StartListening();
            //transmission starts within the accept function
            AcceptClient();
        }
    }
}
