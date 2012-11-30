using Gtk;
using Npgsql;
using PArticulo;
using Serpis.Ad;
using System;
using System.Collections.Generic;
using System.Data;



public partial class MainWindow: Gtk.Window
{	private IDbConnection dbConnection;
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		
		Build ();							
				
		
	string connectionString="Server=localhost;Database=prueba;User id=dbprueba; Password=sistemas";
		dbConnection=new NpgsqlConnection(connectionString);
			dbConnection.Open();
			
					IDbCommand dbCommand=dbConnection.CreateCommand();
			dbCommand.CommandText=
			    "select articulo.id, articulo.nombre,articulo.precio,categoria.nombre as categoria " +
				"from articulo left join categoria " +
				"on articulo.categoria = categoria.id ";
		
			IDataReader dataReader=dbCommand.ExecuteReader();
		
		TreeViewExtensions.Fill (treeView,dataReader);
		
	dataReader.Close();
		
		
			
		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		dbConnection.Close();
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnClearActionActivated (object sender, System.EventArgs e)
	{
		ListStore listStore = (ListStore)treeView.Model;
		listStore.Clear();
	}

	
	protected void OnEditActionActivated (object sender, System.EventArgs e)
	{
		long id= getSelectedId();
		Console.WriteLine("id={0}",id);
		IDbCommand dbCommand = dbConnection.CreateCommand();
		dbCommand.CommandText="Select * from articulo where id=:id";
		IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
		dbDataParameter.ParameterName="id";
		dbCommand.Parameters.Add (dbDataParameter);
		dbDataParameter.Value=id;
		
		IDataReader dataReader = dbCommand.ExecuteReader();
		dataReader.Read ();
		
		
		ArticuloView articuloView= new ArticuloView();
		articuloView.Nombre=(string)dataReader["nombre"];
		articuloView.Precio=double.Parse (dataReader["precio"].ToString());
		articuloView.Show();
		
		
		
		dataReader.Close();
		articuloView.SaveAction.Activated+=delegate {
			Console.WriteLine("articuloView.SaveAction.Activated");
			IDbCommand dbUpdateCommand = dbCommand.CreateCommand();
			dbUpdateCommand.CommandText="update articulo set nombre=:nombre , precio=:precio where id=:id";
			IDbDataParameter nombreParameter = dbUpdateCommand.CreateParameter();
			IDbDataParameter precioParameter = dbUpdateCommand.CreateParameter();
			IDbDataParameter idParameter = dbUpdateCommand.CreateParameter();
			nombreParameter.ParameterName="nombre";
			precioParameter.ParameterName="precio";
			idParameter.ParameterName="id";
			dbUpdateCommand.Parameters.Add (nombreParameter);
			dbUpdateCommand.Parameters.Add (precioParameter);
			dbUpdateCommand.Parameters.Add (idParameter);


			
			articuloView.Destroy();
			
		};
	}
	
	private long getSelectedId(){
		TreeIter treeIter;
	treeView.Selection.GetSelected(out treeIter);
		ListStore listStore =(ListStore)treeView.Model;
		return long.Parse (listStore.GetValue(treeIter,0).ToString());
		
	
	}
	
}
