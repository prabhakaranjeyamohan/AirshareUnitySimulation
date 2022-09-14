using System.IO;
using Google.Protobuf;

namespace IUX
{
    /// <summary>
	/// Helper class for converting / parsing IUX Protobuf messages
	/// </summary>
    public static class MessageHandler
    {
        /// <summary>
		/// Converts the IUX Protobuf Message to datagram
		/// </summary>
		/// <param name="message">IUX Protobuf Message</param>
		/// <returns>Datagram</returns>
        public static byte[] ConvertToDgram(Message message)
		{
            byte[] dgram;
            int messageSize = message.CalculateSize();

            using (var stream = new MemoryStream())
            {
                stream.Write(new byte[] { (byte)(messageSize >> 8), (byte)messageSize }, 0, 2);
                message.WriteTo(stream);

                dgram = stream.ToArray();
            }

            return dgram;
        }

        /// <summary>
		/// Converts the datagram to IUX Protobuf Message
		/// </summary>
		/// <param name="dgram">Datagram</param>
		/// <returns>IUX Protobuf Message</returns>
        public static Message ParseDgram(byte[] dgram)
        {
            return Message.Parser.ParseFrom(dgram, 2, dgram.Length - 2);
        }
    }
}
