using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;

namespace CaisseLibrary.Data
{
	interface IData
	{

		void From<T>(T parent, CaisseServerContext context);


	}
}
