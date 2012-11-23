using System;
using Gtk;
using System.Data;


namespace Serpis.Ad
{
	public static class TreeViewExtensions
	{
		public static void AppendColumns(TreeView treeView, IDataReader dataReader){
		
			for (int i=0;i<dataReader.FieldCount;i++){			
			treeView.AppendColumn(dataReader.GetName(i),new CellRendererText(),"text",i);
		}
	}

	public static void Fill(TreeView treeView, IDataReader dataReader){
		TreeViewExtensions.ClearColumns(treeView);
			TreeViewExtensions.AppendColumns(treeView,dataReader);
		
		Type[] types = TypeExtensions.GetTypes(typeof(string),dataReader.FieldCount);
		ListStore listStore =new ListStore(types);
		treeView.Model=listStore;
		ListStoreExtensions.Fill (listStore,dataReader);
		
		}
	
		public static void ClearColumns(TreeView treeView){
		
		TreeViewColumn[] treeViewColumns = treeView.Columns;
			foreach(TreeViewColumn treeViewColumn in treeViewColumns)
				treeView.RemoveColumn(treeViewColumn);
			
		}
			
		
	}
}
