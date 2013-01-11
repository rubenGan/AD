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
		IDbConnection dbConnection = new NpgsqlConnection("Server=localhost;Database=prueba;User Id=dbprueba; Password=sistemas");
		IDbCommand selectCommand = dbConnection.CreateCommand();	
		selectCommand.CommandText ="select * from articulo";
		IDbDataAdapter dbDataAdapter = new NpgsqlDataAdapter ();
		dbDataAdapter.SelectCommand=selectCommand;
				
		DataSet dataSet = new DataSet();
		
		dbDataAdapter.Fill(dataSet);
		
		Console.WriteLine("Tables.Count={0}",dataSet.Tables.Count);
		foreach (DataTable dataTable in dataSet.Tables)
			show (dataTable);
		
	}
	
	private void show(DataTable datatable){
		
		foreach(DataColumn dataColumn in datatable.Columns)
			Console.WriteLine("Column.Name={0}",dataColumn.ColumnName);
		
		foreach(DataRow dataRow in datatable.Rows){
			foreach(DataColumn dataColumn in datatable.Columns)
				Console.Write(" [{0}={1}] ",dataColumn.ColumnName,dataRow[dataColumn]);
			
			Console.WriteLine();
		}
			
		
	
	}

}
