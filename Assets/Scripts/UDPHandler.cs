using System.Net;
using System.Net.Sockets;

/// <summary>
/// Simple UDP handler capable of sending and receiving packets
/// </summary>
public class UDPHandler
{
	private static int ReceiveTimeout = 1000;

	public uint ReceiverPort { get; }

	private IPEndPoint _serverIPEndPoint;

	private UdpClient _senderClient;

	private UdpClient _receiverClient;

	public UDPHandler(string serverIP, int serverPort)
	{
		_senderClient = new UdpClient(0);
		_receiverClient = new UdpClient(0);

		ReceiverPort = (uint)((IPEndPoint)_receiverClient.Client.LocalEndPoint).Port;

		_receiverClient.Client.ReceiveTimeout = ReceiveTimeout;

		_serverIPEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

		_senderClient.Connect(_serverIPEndPoint);
	}

	/// <summary>
	/// Sends the datagram to the server
	/// </summary>
	/// <param name="dgram">datagram</param>
	public void Send(byte[] dgram)
	{
		_senderClient.Send(dgram, dgram.Length);
	} 

	/// <summary>
	/// Receives the datagram from the server
	/// </summary>
	/// <returns>datagram</returns>
	public byte[] Receive()
	{
		return _receiverClient.Receive(ref _serverIPEndPoint);
	}

	/// <summary>
	/// Closes sender and receiver sockets
	/// </summary>
	public void Close()
	{
		_senderClient.Close();
		_receiverClient.Close();
	}
}
