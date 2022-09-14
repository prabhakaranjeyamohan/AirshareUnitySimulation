using System.Net.Sockets;
using System.Threading;
using IUX;

/// <summary>
/// Class responsible for sending / receiving data from the Ballistic Solution Server
/// </summary>
public class BSSHandler
{
	private BSSRequest _BSSRequest;
	public BSSRequest BSSRequest { get { return _BSSRequest; } }

	private BSSReply _BSSReply;
	public BSSReply BSSReply { get { return _BSSReply; } }

	private Thread _BSSListenThread;
	
	private UDPHandler _UDPHandler;

	private bool _listenBSS;

	public readonly object replyLock = new object();

	private readonly object _listenLock = new object();

	public BSSHandler(BSSRequest BSSRequest)
	{
		_listenBSS = true;

		_UDPHandler = new UDPHandler(AADManager.Instance.MissileLauncherManager.BSS_IP, AADManager.Instance.MissileLauncherManager.BSS_PORT);

		_BSSRequest = BSSRequest;
		_BSSRequest.ReturnPort = _UDPHandler.ReceiverPort;

		_BSSListenThread = new Thread(ListenBSS);
		_BSSListenThread.Start();
	}

	/// <summary>
	/// Sends the BSS Request through UDP
	/// </summary>
	public void SendRequest()
	{
		_UDPHandler.Send(MessageHandler.ConvertToDgram(new Message { BssRequest = _BSSRequest }));
	}

	/// <summary>
	/// Stops the listening thread
	/// </summary>
	public void Close()
	{
		lock (_listenLock) _listenBSS = false;
	}

	/// <summary>
	/// Keeps listening and reading incoming BSS replies, executed on a separate thread
	/// </summary>
	private void ListenBSS()
	{
		while (true)
		{
			BSSReply reply;

			try
			{
				reply = MessageHandler.ParseDgram(_UDPHandler.Receive()).BssReply;
			}
			catch (SocketException)
			{
				continue;
			}

			lock (replyLock) _BSSReply = reply;

			lock (_listenLock)
			{
				if (!_listenBSS)
				{
					_UDPHandler.Close();
					return;
				}
			}
		}
	}
}
