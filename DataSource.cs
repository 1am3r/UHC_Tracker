using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.ServiceModel.Channels;
//using System.ServiceModel;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace DataSource
{
	public class DS
	{
		//private static int SHORT_BYTES = 2;
		private static int INT_BYTES = 4;
		//private static int LONG_BYTES = 8;
		private static int FLOAT_BYTES = 4;
		private static int DOUBLE_BYTES = 8;
        private const byte GET_PLAYER_LOCATION = 0;
        private const byte GET_WORLD_PATH = 20;

		/// <summary>
		/// True if connected.
		/// </summary>
		public static bool isConnected
		{
			get
			{
				return client != null && client.Connected;
			}
		}
		public static void ShutDown()
		{
			shuttingDown = true;
			client.Disconnect();
		}
		private static bool shuttingDown = false;
		protected static BytesEventTcpClient client;

		protected static DSHandlerPlayerLocation playerLocationHandler;
		public static void InitializePlayerLocation(DSHandlerPlayerLocation handlerparam)
		{
			playerLocationHandler = handlerparam;
		}
		protected static DSHandlerConnectionStatusChanged connectionStatusChangedHandler;
		public static void InitializeConnectionStatusChanged(DSHandlerConnectionStatusChanged handlerparam)
		{
			connectionStatusChangedHandler = handlerparam;
		}
        protected static DSHandlerWorldPath worldPathHandler;
        public static void InitializeWorldPath(DSHandlerWorldPath handlerparam)
        {
            worldPathHandler = handlerparam;
        }

		private static void ConnectionStatusChanged(ConnectionStatus connectionStatus)
		{
			if (connectionStatusChangedHandler != null)
				connectionStatusChangedHandler.ConnectionStatusChanged(connectionStatus);
		}

		/// <summary>
		/// Requests from the Minecraft Client the player's location.
		/// </summary>
		public static void GetPlayerLocation()
		{
			if (isConnected)
			{
				client.Send(new byte[] { GET_PLAYER_LOCATION });
			}
		}

        /// <summary>
        /// Requests from the Minecraft Client the player's location.
        /// </summary>
        public static void GetWorldPath()
        {
            if (isConnected)
            {
                client.Send(new byte[] { GET_WORLD_PATH });
            }
        }

		//private static System.Diagnostics.Stopwatch sw;
		/// <summary>
		/// Handles a message received from the Minecraft Client.
		/// </summary>
		/// <param name="message">The string received from the Minecraft Client.</param>
		private static void HandleMessage(byte[] message)
		{
			if (message.Length <= 0)
				return;
			byte messageType = message[0];
			if (messageType == GET_PLAYER_LOCATION)
			{
				//if (sw == null)
				//    sw = new System.Diagnostics.Stopwatch();
				//else
				//{
				//    sw.Stop();
				//    File.AppendAllText("getplayerlocationlog.txt", sw.ElapsedTicks + Environment.NewLine);
				//    sw.Reset();
				//}
				HandlePlayerLocation(message);
				//sw.Start();
            }
            else if (messageType == GET_WORLD_PATH)
            {
                HandleWorldPath(message);
            }
		}

		private static void HandlePlayerLocation(byte[] message)
		{
			try
			{
				if (playerLocationHandler == null)
					return;
				List<Entity> players = new List<Entity>();
				int reqIdx = 1;
				int sizeOfPlayerName = ByteConverter.ToInt32(message, reqIdx);
				reqIdx += INT_BYTES;
				string playerName = ByteConverter.ToString(message, reqIdx, sizeOfPlayerName);
				reqIdx += sizeOfPlayerName;
				Entity playerMe = null;
				while (reqIdx < message.Length)
				{
					Entity p = new Entity();
					p.z = -ByteConverter.ToDouble(message, reqIdx);
					reqIdx += DOUBLE_BYTES;
					p.x = ByteConverter.ToDouble(message, reqIdx);
					reqIdx += DOUBLE_BYTES;
					p.y = ByteConverter.ToDouble(message, reqIdx);
					reqIdx += DOUBLE_BYTES;
					p.rotation = ByteConverter.ToSingle(message, reqIdx);
					reqIdx += FLOAT_BYTES;
					p.pitch = ByteConverter.ToSingle(message, reqIdx);
					reqIdx += FLOAT_BYTES;
					int namesize = ByteConverter.ToInt32(message, reqIdx);
					reqIdx += INT_BYTES;
					p.name = ByteConverter.ToString(message, reqIdx, namesize);
					reqIdx += namesize;
					p.isNPC = ByteConverter.ToBoolean(message, reqIdx);
					reqIdx++;
					p.Calc();
					AddEntity(players, p, playerName);
					if (p.name == playerName)
						playerMe = p;
				}
				if (playerMe != null)
				{
					try
					{
						playerLocationHandler.PlayerLocationReceived(players, playerMe);
					}
					catch (Exception ex)
					{
						File.AppendAllText("errordump.txt", "\r\nAn error occurred while handling a player location.\r\n" + ex.ToString() + "\r\n");
						System.Windows.Forms.MessageBox.Show("An error occurred while handling a player location." + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
					}
				}
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.ToString());
				return;
			}
		}

        private static void HandleWorldPath(byte[] message)
        {
            string worldInfo = ByteConverter.ToString(message, 1, message.Length - 1);
            if (worldInfo == "Unknown")
                if (worldPathHandler != null)
                    try
                    {
                        worldPathHandler.WorldPathReceived("Unknown", 0, "Unknown", 0, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText("errordump.txt", "\r\nAn error occurred while handling a world path.\r\n" + ex.ToString() + "\r\n");
                        System.Windows.Forms.MessageBox.Show("An error occurred while handling a world path." + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                    }

            string[] parts = worldInfo.Split('|');
            if (parts.Length < 6)
                return;
            else if (parts.Length > 6)
            {
                // In case the path SOMEHOW had a | in it, the string array will be messed up.  Maybe I am just paranoid.
                StringBuilder sbPathParts = new StringBuilder();
                for (int i = 0; i <= parts.Length - 6; i++)
                    sbPathParts.Append(parts[i]);
                string[] partsNew = new string[6];
                partsNew[0] = sbPathParts.ToString();
                for (int i = parts.Length - 5, j = 1; i < parts.Length; i++, j++)
                    partsNew[j] = parts[i];
                parts = partsNew;
            }
            int dimension;
            if (!int.TryParse(parts[1], out dimension))
                dimension = 0;
            int spawnX;
            if (!int.TryParse(parts[3], out spawnX))
                spawnX = 0;
            int spawnY;
            if (!int.TryParse(parts[4], out spawnY))
                spawnY = 0;
            int spawnZ;
            if (!int.TryParse(parts[5], out spawnZ))
                spawnZ = 0;
            if (worldPathHandler != null)
                try
                {
                    worldPathHandler.WorldPathReceived(parts[0], dimension, parts[2], spawnX, spawnY, spawnZ);
                }
                catch (Exception ex)
                {
                    File.AppendAllText("errordump.txt", "\r\nAn error occurred while handling a world path.\r\n" + ex.ToString() + "\r\n");
                    System.Windows.Forms.MessageBox.Show("An error occurred while handling a world path." + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                }
        }

		/// <summary>
		/// Checks PermissionManager and adds the entity to the list if allowed.
		/// </summary>
		/// <param name="players"></param>
		/// <param name="p"></param>
		private static void AddEntity(List<Entity> entities, Entity e, string playerName)
		{
			if (!e.isNPC)
			{
					entities.Add(e);
			}
		}

		public static void InsertBytes(byte[] destination, int destinationOffset, byte[] source)
		{
			if (destination.Length - destinationOffset < source.Length)
				return;
			for (int i = 0; i < source.Length; i++, destinationOffset++)
				destination[destinationOffset] = source[i];
		}

		#region Networking
		protected static string lastHost = "";
		protected static ushort lastPort = 12345;
		public static void Connect(string host, ushort port, bool isRetry = false)
		{
			if (shuttingDown)
				return;
			if (!isRetry)
				ConnectionStatusChanged(ConnectionStatus.Connecting);
			client = new BytesEventTcpClient();
			client.OnConnect += new EventTcpClient.OnConnectEventHandler(client_OnConnect);
			client.OnDataReceived += new BytesEventTcpClient.BytesReceivedEventHandler(client_OnDataReceived);
			client.OnDisconnect += new EventTcpClient.OnDisconnectEventHandler(client_OnDisconnect);
			client.OnRefused += new EventTcpClient.OnRefusedEventHandler(client_OnRefused);
			lastHost = host;
			lastPort = port;
			client.Connect(host, port);
		}

		/// <summary>
		/// Called if a connection is refused (unable to connect).
		/// </summary>
		static void client_OnRefused()
		{
			ConnectionStatusChanged(ConnectionStatus.Refused);
			//System.Windows.Forms.MessageBox.Show("Connection refused.  If Minecraft is already running and a world is loaded, you may have forgotten to install Risugami's ModLoader, or the Automap mod may not be installed correctly.");
			Console.WriteLine("Connection Refused");
			Connect(lastHost, lastPort, true);
		}

		/// <summary>
		/// Called when we get disconnected.
		/// </summary>
		static void client_OnDisconnect()
		{
			ConnectionStatusChanged(ConnectionStatus.Disconnected);
			//if (!shuttingDown)
			//	System.Windows.Forms.MessageBox.Show("Disconnected");
			Console.WriteLine("Disconnected");
			Connect(lastHost, lastPort, true);
		}

		/// <summary>
		/// Called when data is received.
		/// </summary>
		/// <param name="message">A byte array that has been received.</param>
		static void client_OnDataReceived(byte[] message)
		{
			HandleMessage(message);
		}

		/// <summary>
		/// Called when we establish a connection.
		/// </summary>
		static void client_OnConnect()
		{
			ConnectionStatusChanged(ConnectionStatus.Connected);
			Console.WriteLine("Connected");
		}
		#endregion
	}

    public interface DSHandlerPlayerLocation
    {
        void PlayerLocationReceived(List<Entity> players, Entity userPlayer);
    }
    public enum ConnectionStatus { Idle, Connecting, Connected, Refused, Disconnected };
    public interface DSHandlerConnectionStatusChanged
    {
        void ConnectionStatusChanged(ConnectionStatus newStatus);
    }
    public interface DSHandlerWorldPath
    {
        void WorldPathReceived(string worldPath, int dimension, string worldName, int spawnX, int spawnY, int spawnZ);
    }

    public class Entity
    {
        public string name = "default";
        public double x = 0;
        public double y = 0;
        public double z = 0;
        public int ix = 0;
        public int iy = 0;
        public int iz = 0;
        public double rotation = 0;
        public float pitch = 0;
        public bool isNPC = false;

        public Entity()
        {

        }

        public void Calc()
        {
            ix = (int)Math.Floor(x);
            iy = (int)Math.Floor(y);
            iz = (int)Math.Floor(z);
            this.rotation = (Math.PI / 180) * (rotation - 90);
        }
    }

    public static class ByteConverter
    {
        public static byte[] GetBytes(int data)
        {
            return new byte[]
                {
                    (byte) ((data >> 24) & 0xff),
                    (byte) ((data >> 16) & 0xff),
                    (byte) ((data >> 8) & 0xff),
                    (byte) ((data >> 0) & 0xff),
                };
        }

        public static byte[] GetBytes(string data)
        {
            try
            {
                return Encoding.UTF8.GetBytes(data);
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }

        public static int ToInt32(byte[] data, int startIndex)
        {
            if (data == null || data.Length - startIndex < 4)
                return 0;
            // ----------
            return (int)( // NOTE: type cast not necessary for int
                    (0xff & data[startIndex]) << 24
                    | (0xff & data[startIndex + 1]) << 16
                    | (0xff & data[startIndex + 2]) << 8
                    | (0xff & data[startIndex + 3]) << 0);
        }



        public static float ToSingle(byte[] data, int startIndex)
        {
            byte[] temp = new byte[4];
            temp[0] = data[startIndex + 3];
            temp[1] = data[startIndex + 2];
            temp[2] = data[startIndex + 1];
            temp[3] = data[startIndex + 0];

            return System.BitConverter.ToSingle(temp, 0);

        }

        public static double ToDouble(byte[] data, int startIndex)
        {
            byte[] temp = new byte[8];
            temp[0] = data[startIndex + 7];
            temp[1] = data[startIndex + 6];
            temp[2] = data[startIndex + 5];
            temp[3] = data[startIndex + 4];
            temp[4] = data[startIndex + 3];
            temp[5] = data[startIndex + 2];
            temp[6] = data[startIndex + 1];
            temp[7] = data[startIndex + 0];

            return System.BitConverter.ToDouble(temp, 0);

        }

        public static bool ToBoolean(byte[] data, int startIndex)
        {
            return (data == null || data.Length - startIndex < 1) ? false : data[startIndex] != 0x00;
        }

        public static string ToString(byte[] message, int startIndex, int bytesToConvert)
        {
            try
            {
                return Encoding.UTF8.GetString(message, startIndex, bytesToConvert);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
