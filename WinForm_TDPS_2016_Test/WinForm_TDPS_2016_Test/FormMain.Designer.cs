﻿namespace WinForm_TDPS_2016_Test
{
	partial class FormMain
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonBeginEnd = new System.Windows.Forms.Button();
			this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
			this.comboBoxCaptureDevice = new System.Windows.Forms.ComboBox();
			this.comboBoxCaptureResolution = new System.Windows.Forms.ComboBox();
			this.pictureBox = new AForge.Controls.PictureBox();
			this.buttonSample = new System.Windows.Forms.Button();
			this.buttonDebug = new System.Windows.Forms.Button();
			this.buttonDebug2 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonBeginEnd
			// 
			this.buttonBeginEnd.Location = new System.Drawing.Point(411, 69);
			this.buttonBeginEnd.Name = "buttonBeginEnd";
			this.buttonBeginEnd.Size = new System.Drawing.Size(75, 25);
			this.buttonBeginEnd.TabIndex = 0;
			this.buttonBeginEnd.Text = "Start";
			this.buttonBeginEnd.UseVisualStyleBackColor = true;
			this.buttonBeginEnd.Click += new System.EventHandler(this.buttonBeginEnd_Click);
			// 
			// videoSourcePlayer
			// 
			this.videoSourcePlayer.Location = new System.Drawing.Point(12, 13);
			this.videoSourcePlayer.Name = "videoSourcePlayer";
			this.videoSourcePlayer.Size = new System.Drawing.Size(393, 315);
			this.videoSourcePlayer.TabIndex = 3;
			this.videoSourcePlayer.Text = "videoSourcePlayer1";
			this.videoSourcePlayer.VideoSource = null;
			// 
			// comboBoxCaptureDevice
			// 
			this.comboBoxCaptureDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCaptureDevice.FormattingEnabled = true;
			this.comboBoxCaptureDevice.Location = new System.Drawing.Point(411, 13);
			this.comboBoxCaptureDevice.Name = "comboBoxCaptureDevice";
			this.comboBoxCaptureDevice.Size = new System.Drawing.Size(174, 21);
			this.comboBoxCaptureDevice.TabIndex = 4;
			this.comboBoxCaptureDevice.TextChanged += new System.EventHandler(this.comboBoxCaptureDevice_TextChanged);
			// 
			// comboBoxCaptureResolution
			// 
			this.comboBoxCaptureResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCaptureResolution.FormattingEnabled = true;
			this.comboBoxCaptureResolution.Location = new System.Drawing.Point(411, 41);
			this.comboBoxCaptureResolution.Name = "comboBoxCaptureResolution";
			this.comboBoxCaptureResolution.Size = new System.Drawing.Size(174, 21);
			this.comboBoxCaptureResolution.TabIndex = 5;
			// 
			// pictureBox
			// 
			this.pictureBox.Image = null;
			this.pictureBox.Location = new System.Drawing.Point(591, 13);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(393, 315);
			this.pictureBox.TabIndex = 6;
			this.pictureBox.TabStop = false;
			// 
			// buttonSample
			// 
			this.buttonSample.Location = new System.Drawing.Point(492, 69);
			this.buttonSample.Name = "buttonSample";
			this.buttonSample.Size = new System.Drawing.Size(93, 25);
			this.buttonSample.TabIndex = 7;
			this.buttonSample.Text = "Sample";
			this.buttonSample.UseVisualStyleBackColor = true;
			this.buttonSample.Click += new System.EventHandler(this.buttonSample_Click);
			// 
			// buttonDebug
			// 
			this.buttonDebug.Location = new System.Drawing.Point(411, 303);
			this.buttonDebug.Name = "buttonDebug";
			this.buttonDebug.Size = new System.Drawing.Size(75, 25);
			this.buttonDebug.TabIndex = 8;
			this.buttonDebug.Text = "Debug";
			this.buttonDebug.UseVisualStyleBackColor = true;
			this.buttonDebug.Click += new System.EventHandler(this.buttonDebug_Click);
			// 
			// buttonDebug2
			// 
			this.buttonDebug2.Location = new System.Drawing.Point(492, 303);
			this.buttonDebug2.Name = "buttonDebug2";
			this.buttonDebug2.Size = new System.Drawing.Size(75, 25);
			this.buttonDebug2.TabIndex = 9;
			this.buttonDebug2.Text = "Debug2";
			this.buttonDebug2.UseVisualStyleBackColor = true;
			this.buttonDebug2.Click += new System.EventHandler(this.buttonDebug2_Click);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1153, 407);
			this.Controls.Add(this.buttonDebug2);
			this.Controls.Add(this.buttonDebug);
			this.Controls.Add(this.buttonSample);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.comboBoxCaptureResolution);
			this.Controls.Add(this.comboBoxCaptureDevice);
			this.Controls.Add(this.videoSourcePlayer);
			this.Controls.Add(this.buttonBeginEnd);
			this.Name = "FormMain";
			this.Text = "Meepo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
			this.Load += new System.EventHandler(this.FormMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonBeginEnd;
		private AForge.Controls.VideoSourcePlayer videoSourcePlayer;
		private System.Windows.Forms.ComboBox comboBoxCaptureDevice;
		private System.Windows.Forms.ComboBox comboBoxCaptureResolution;
		private AForge.Controls.PictureBox pictureBox;
		private System.Windows.Forms.Button buttonSample;
		private System.Windows.Forms.Button buttonDebug;
		private System.Windows.Forms.Button buttonDebug2;
	}
}

