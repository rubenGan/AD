using Gtk;
using System;
using System.Collections.Generic;
using System.Data;



namespace Serpis.Ad
{
	public static class ListStoreExtensions
	{
		public static void Fill(ListStore listStore, IDataReader dataReader){
		
		
			while (dataReader.Read ())	{					
				
				List<string>values = new List<string>();
				
				for(int i=0;i<dataReader.FieldCount;i++)
					values.Add(dataReader[i].ToString());
										
				listStore.AppendValues(values.ToArray());
			
				
				
			}
		
		}
	}
}

