using Gtk;
using Npgsql;
using PArticulo;
using Serpis.Ad;
using System;
using System.Collections.Generic;
using System.Data;


public partial class MainWindow: Gtk.Window
{
	private IDbConnection dbConnection;
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		string connectionString = "Server=localhost;Database=prueba;User Id=dbprueba;Password=sistemas";
		ApplicationContext.Instance.DbConnection = new NpgsqlConnection(connectionString);
		dbConnection = ApplicationContext.Instance.DbConnection;
		dbConnection.Open ();
		
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = 
			"select a.id, a.nombre, a.precio, c.nombre as categoria " +
			"from articulo a left join categoria c " +
			"on a.categoria = c.id";
		
		IDataReader dataReader = dbCommand.ExecuteReader ();
		
		TreeViewExtensions.Fill (treeView, dataReader);
		dataReader.Close ();
		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		dbConnection.Close ();

		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnClearActionActivated (object sender, System.EventArgs e)
	{
		ListStore listStore = (ListStore)treeView.Model;
		listStore.Clear ();
	}

	protected void OnEditActionActivated (object sender, System.EventArgs e)
	{
		long id = getSelectedId();
		
		

		
		ArticuloView articuloView = new ArticuloView( id );
		articuloView.Show ();
		}
	
	
	private long getSelectedId() {
		TreeIter treeIter;
		treeView.Selection.GetSelected(out treeIter);
		
		ListStore listStore = (ListStore)treeView.Model;
		return long.Parse (listStore.GetValue (treeIter, 0).ToString ()); 
	}

	protected void OnAddActionActivated (object sender, System.EventArgs e)
	{
		ArticuloView articuloView = new ArticuloView (-1);
		articuloView.Show ();
	}


	protected void OnRemoveActionActivated (object sender, System.EventArgs e)
	{
		IDbCommand dbDeleteCommand = dbConnection.CreateCommand ();
	   dbDeleteCommand.CommandText = "delete from articulo where id=:id;";
		
		DbCommandExtensions.AddParameter (dbDeleteCommand, "id", getSelectedId());
	
				dbDeleteCommand.ExecuteNonQuery ();
	}
		
	private void refresh()
	{
		IDbCommand dbCommand = ApplicationContext.Instance.DbConnection.CreateCommand ();
		dbCommand.CommandText =
		"select a.id, a.nombre, a.precio, c.nombre as categoria " +
		"from articulo a left join categoria c " +
		"on a.categoria = c.id";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		TreeViewExtensions.Fill (treeView, dataReader);
		dataReader.Close ();
	}


	protected void OnRefreshActionActivated (object sender, System.EventArgs e)
	{
	refresh();

	}
	
}
