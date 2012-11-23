using Gtk;
using Npgsql;
using Serpis.Ad;
using System;
using System.Collections.Generic;
using System.Data;



public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		
		Build ();							
				
		
	string connectionString="Server=localhost;Database=prueba;User id=dbprueba; Password=sistemas";
		IDbConnection dbConnection=new NpgsqlConnection(connectionString);
			dbConnection.Open();
			
					IDbCommand dbCommand=dbConnection.CreateCommand();
			dbCommand.CommandText="select * from articulo order by id";
		
			IDataReader dataReader=dbCommand.ExecuteReader();
		
		TreeViewExtensions.Fill (treeView,dataReader);
		
	dataReader.Close();
		dbConnection.Close();
		
			
		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnClearActionActivated (object sender, System.EventArgs e)
	{
		ListStore listStore = (ListStore)treeView.Model;
		listStore.Clear();
	}

	
}
