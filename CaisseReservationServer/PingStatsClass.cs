using System;
using System.Collections.Generic;
using System.Text;

namespace CaisseReservationServer
{
	public class PingStatsClass
	{
		public PingStatsClass()//Int32 ClientID)
		{
			//clientID = ClientID;
			sw = new System.Diagnostics.Stopwatch();
			PingCounter = 0;
			PingTimeTotal = 0;
			LongestPing = 0;
			LongestPingDateTimeStamp = DateTime.Now;
		}

		private System.Diagnostics.Stopwatch sw = null;

		public int PingCounter;

		/// <summary>
		/// Time is in milliseconds
		/// </summary>
		public long PingTimeTotal;

		/// <summary>
		/// Time is in milliseconds
		/// </summary>
		public long LongestPing;
		public DateTime LongestPingDateTimeStamp;

		/// <summary>
		/// returns the elapsed ping time in miliseconds
		/// </summary>
		/// <returns></returns>
		public long StopTheClock()
		{
			if (!sw.IsRunning) return sw.ElapsedMilliseconds;
			sw.Stop();

			PingCounter++;

			if (sw.ElapsedMilliseconds > LongestPing)
			{
				LongestPing = sw.ElapsedMilliseconds;
				LongestPingDateTimeStamp = DateTime.Now;
			}

			PingTimeTotal += sw.ElapsedMilliseconds;

			return sw.ElapsedMilliseconds;
		}

		public void StartTheClock()
		{
			sw.Reset();

			if (!sw.IsRunning)
				sw.Start();
			else
				sw.Restart();
		}

		public long GetElapsedTime => sw.ElapsedMilliseconds;
	}
}
