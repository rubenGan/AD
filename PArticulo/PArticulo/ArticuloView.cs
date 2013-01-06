using Gtk;
using Serpis.Ad;
using System;
using System.Data;

namespace PArticulo
{
	public partial class ArticuloView : Gtk.Window
	{
		private IDbConnection dbConnection;
		public ArticuloView (long id) : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			
			
			dbConnection = ApplicationContext.Instance.DbConnection;
			if(id !=-1)
			{
			editar(id);
				
			}
			else if(id==-1)
			{
				nuevo ();
		}
			
	}
		private void editar(long id){
		
		IDbCommand dbCommand = dbConnection.CreateCommand();
			
			dbCommand.CommandText = string.Format ("select * from articulo where id={0}", id);
			
			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();
			
			entryNombre.Text = (string)dataReader["nombre"];
			spinButtonPrecio.Value = Convert.ToDouble( (decimal)dataReader["precio"] );
			object categoriaData = dataReader["categoria"];
			
			long? categoria = null;
			if (!(categoria is DBNull))
				categoria=(long)categoriaData;
			
			dataReader.Close ();
			
			saveAction.Activated += delegate {
				Console.WriteLine("saveAction.Activated");
				
				IDbCommand dbUpdateCommand = dbConnection.CreateCommand ();
				dbUpdateCommand.CommandText = "update articulo set nombre=:nombre, precio=:precio where id=:id";
				
				DbCommandExtensions.AddParameter (dbUpdateCommand, "nombre", entryNombre.Text);
				DbCommandExtensions.AddParameter (dbUpdateCommand, "precio", Convert.ToDecimal (spinButtonPrecio.Value ));
				DbCommandExtensions.AddParameter (dbUpdateCommand, "id", id);
				fillComboBox();
	
				dbUpdateCommand.ExecuteNonQuery ();
				
				
				
				Destroy ();
			};
		}
		
		private void nuevo(){
			
			
				saveAction.Activated += delegate {
				Console.WriteLine("saveAction.Activated Nuevo");
				
				IDbCommand dbUpdateCommand = dbConnection.CreateCommand ();
				dbUpdateCommand.CommandText = "insert into articulo (nombre,precio) values (:nombre,:precio)";
				
				DbCommandExtensions.AddParameter (dbUpdateCommand, "nombre", entryNombre.Text);
				DbCommandExtensions.AddParameter (dbUpdateCommand, "precio", Convert.ToDecimal (spinButtonPrecio.Value ));
				fillComboBox();
				
				
				
	
				dbUpdateCommand.ExecuteNonQuery ();
								
				

				
				Destroy ();
			
			
			};
		
		}
		
		private void fillComboBox(){
		CellRenderer cellRenderer = new CellRendererText();
		comboBoxCategoria.PackStart(cellRenderer,false); //expand false
		comboBoxCategoria.AddAttribute(cellRenderer,"text",1);
		
		ListStore listStore = new ListStore(typeof(string),typeof(string));
		comboBoxCategoria.Model=listStore;
		
		
		
		IDbCommand dbCommand= dbConnection.CreateCommand();
		dbCommand.CommandText="select id,nombre from categoria";
		
		IDataReader dataReader = dbCommand.ExecuteReader();
	
		while(dataReader.Read ())
		listStore.AppendValues(dataReader["id"].ToString(),dataReader["nombre"].ToString());
		
		dataReader.Close();
		
		
	
	
	}
}
}
