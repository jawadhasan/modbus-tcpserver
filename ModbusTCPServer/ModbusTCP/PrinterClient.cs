using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace ModbusTCP
{
    public class PrinterClient
    {
        TcpListener server = null;
        private readonly string _ipAddress;

        //ctor
        public PrinterClient(string ipAddress)
        {
            _ipAddress = ipAddress;
        }


        public void Start()
        {
            try
            {
                Int32 port = 502;
                IPAddress localAddr = IPAddress.Parse(_ipAddress);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("TCP Server Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    stream.ReadTimeout = 10000;

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        //TODO: Process the data sent by the client.
                        //data = data.ToUpper();

                        //Prepare Response to send back to caller
                        byte ACK = Convert.ToByte("06", 16);
                        byte NAK = Convert.ToByte("15", 16);
                        byte[] msg = new byte[1];


                        //Reply back to caller
                        Random random = new Random();
                        var rand = random.Next(1, 3);

                        if (rand > 1)
                        {
                            msg[0] = ACK; //NAK;
                        }
                        else
                            msg[0] = ACK;


                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        //Console.WriteLine("Sent: {0}", Utils.GetAsciiStringFromByteArray(msg));
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server?.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        public void RefreshData()
        {
            //TODO: Simulate data refresh
            Console.Write("TODO: Simulate data refresh");
        }
    }
}
