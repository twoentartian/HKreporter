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
		private enum StorageState
		{
			Null, ArrayDouble, ListDouble
		};

		private StorageState nowState;
		private double[,] dataSet;
		private List<double[]> dataList;
		private GraphPane myPane;

		public ZedGraphForm(double[,] argData)
		{
			InitializeComponent();

			nowState = StorageState.ArrayDouble;
			for (int i = 0; i < argData.GetLength(0); i++)
			{
				comboBoxDataSelect.Items.Add(string.Format("Data Set: {0:D}", i));
			}
			dataSet = argData;
			myPane = zedGraphTable.GraphPane;
			myPane.Title.Text = "X - Y";
			myPane.XAxis.Title.Text = "Point";
			myPane.YAxis.Title.Text = "Value";
			if (comboBoxDataSelect.Items.Count == 1)
			{
				comboBoxDataSelect.SelectedIndex = 0;
			}
		}

		public ZedGraphForm(List<double[]> argData)
		{
			InitializeComponent();

			nowState = StorageState.ListDouble;
			for (int i = 0; i < argData.Count; i++)
			{
				comboBoxDataSelect.Items.Add(string.Format("Data Set: {0:D}", i));
			}
			dataList = argData;
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

		private void ClearGraph()
		{
			for (int i = 0; i < myPane.CurveList.Count; i++)
			{
				myPane.CurveList[i].Clear();
			}
		}

		private void FlashData()
		{
			PointPairList list = new PointPairList();
			if (nowState == StorageState.ArrayDouble)
			{
				int selectedValue = comboBoxDataSelect.SelectedIndex;
				for (int i = 0; i < dataSet.GetLength(1); i++)
				{
					list.Add(i, dataSet[selectedValue, i]);
				}
			}
			else if (nowState == StorageState.ListDouble)
			{
				int selectedValue = comboBoxDataSelect.SelectedIndex;
				double[] value = dataList[selectedValue];
				for (int i = 0; i < value.Length; i++)
				{
					list.Add(i, value[i]);
				}
			}
			else
			{
				
			}
			ClearGraph();
			LineItem myCurve = myPane.AddCurve(String.Empty, list, Color.Blue, SymbolType.Circle);
			myPane.AxisChange();
			Refresh();
		}
		#endregion
	}
}
