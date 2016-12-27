using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace WinForm_TDPS_2016_Test
{
	public partial class ZedGraphForm : Form
	{
		private double[,] DataSet;
		private GraphPane myPane;

		public ZedGraphForm(double[,] argData)
		{
			InitializeComponent();

			for (int i = 0; i < argData.GetLength(0); i++)
			{
				comboBoxDataSelect.Items.Add(string.Format("Data Set: {0:D}", i));
			}
			DataSet = argData;
			myPane = zedGraphTable.GraphPane;
			myPane.Title.Text = "X - Y";
			myPane.XAxis.Title.Text = "Point";
			myPane.YAxis.Title.Text = "Value";
			if (comboBoxDataSelect.Items.Count == 1)
			{
				comboBoxDataSelect.SelectedIndex = 0;
			}
		}

		private void comboBoxDataSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			FlashData();
		}

		#region Func

		private void FlashData()
		{
			int selectedValue = comboBoxDataSelect.SelectedIndex;

			// Make up some data arrays based on the Sine function
			PointPairList list = new PointPairList();
			for (int i = 0; i < DataSet.GetLength(1); i++)
			{
				list.Add(i, DataSet[selectedValue, i]);
			}

			for (int i = 0; i < myPane.CurveList.Count; i++)
			{
				myPane.CurveList[i].Clear();
			}
			
			// Generate a blue curve with circle
			// symbols, and "Piper" in the legend
			LineItem myCurve = myPane.AddCurve(String.Empty, list, Color.Blue, SymbolType.Circle);

			myPane.AxisChange();
			Refresh();
		}
		#endregion
	}
}
