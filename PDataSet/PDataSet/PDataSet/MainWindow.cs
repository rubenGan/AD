using Gtk;
using Npgsql;
using System;
using System.Data;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnExecuteActionActivated (object sender, System.EventArgs e)
	{
		NpgsqlConnection dbConnection = new NpgsqlConnection("Server=localhost;Database=prueba;User Id=dbprueba; Password=sistemas");
		NpgsqlCommand selectCommand = dbConnection.CreateCommand();	
		selectCommand.CommandText ="select * from categoria";
		NpgsqlDataAdapter dbDataAdapter = new NpgsqlDataAdapter ();
		
		new NpgsqlCommandBuilder(dbDataAdapter);
		dbDataAdapter.SelectCommand=selectCommand;
				
		DataSet dataSet = new DataSet();
		
		dbDataAdapter.Fill(dataSet);
		
		Console.WriteLine("Tables.Count={0}",dataSet.Tables.Count);
		foreach (DataTable dataTable in dataSet.Tables)
			show (dataTable);
		
		DataRow dataRow =dataSet.Tables[0].Rows[0];
		dataRow["nombre"]=DateTime.Now.ToString ();
		Console.WriteLine("Tabla con los cambios");
		show (dataSet.Tables[0]);
		
		
		dbDataAdapter.Update(dataSet.Tables[0]);
		
	}
	
	private void show(DataTable datatable){
		
		/*foreach(DataColumn dataColumn in datatable.Columns)
			Console.WriteLine("Column.Name={0}",dataColumn.ColumnName);*/
		
		foreach(DataRow dataRow in datatable.Rows){
			foreach(DataColumn dataColumn in datatable.Columns)
				Console.Write(" [{0}={1}] ",dataColumn.ColumnName,dataRow[dataColumn]);
			
			Console.WriteLine();
		}
			
		
	
	}

}
