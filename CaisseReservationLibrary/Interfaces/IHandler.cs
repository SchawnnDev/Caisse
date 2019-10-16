using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseReservationLibrary.Interfaces
{
	public interface IHandler<in T>
	{

		void Handle(T packet);

	}
}
