using System;
using Growl.DisplayStyle;

namespace Growl.Display.Windows10 {
	partial class WindowsGrowlNotificationWindow {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose ( );
			}
			base.Dispose ( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ( ) {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsGrowlNotificationWindow));
			this.icon = new System.Windows.Forms.PictureBox();
			this.title = new Growl.DisplayStyle.ExpandingLabel();
			this.description = new Growl.DisplayStyle.ExpandingLabel();
			((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
			this.SuspendLayout();
			// 
			// icon
			// 
			this.icon.Image = ((System.Drawing.Image)(resources.GetObject("icon.Image")));
			this.icon.Location = new System.Drawing.Point(12, 12);
			this.icon.Name = "icon";
			this.icon.Size = new System.Drawing.Size(32, 32);
			this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.icon.TabIndex = 0;
			this.icon.TabStop = false;
			// 
			// title
			// 
			this.title.AutoEllipsis = true;
			this.title.BackColor = System.Drawing.Color.Transparent;
			this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.title.ForeColor = System.Drawing.Color.White;
			this.title.Location = new System.Drawing.Point(64, 12);
			this.title.Name = "title";
			this.title.Size = new System.Drawing.Size(284, 24);
			this.title.TabIndex = 1;
			this.title.Text = "label1";
			this.title.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.title.UseMnemonic = false;
			this.title.LabelHeightChanged += new Growl.DisplayStyle.ExpandingLabel.LabelHeightChangedEventHandler(this.title_LabelHeightChanged);
			// 
			// description
			// 
			this.description.AutoEllipsis = true;
			this.description.BackColor = System.Drawing.Color.Transparent;
			this.description.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.description.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(165)))), ((int)(((byte)(165)))));
			this.description.Location = new System.Drawing.Point(64, 48);
			this.description.Name = "description";
			this.description.Size = new System.Drawing.Size(284, 42);
			this.description.TabIndex = 2;
			this.description.Text = "label2";
			this.description.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.description.UseMnemonic = false;
			this.description.LabelHeightChanged += new Growl.DisplayStyle.ExpandingLabel.LabelHeightChangedEventHandler(this.description_LabelHeightChanged);
			// 
			// WindowsGrowlNotificationWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(360, 100);
			this.Controls.Add(this.description);
			this.Controls.Add(this.title);
			this.Controls.Add(this.icon);
			this.Name = "WindowsGrowlNotificationWindow";
			this.Text = "WindowsGrowlNotificationWindow";
			((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox icon;
		private ExpandingLabel title;
		private ExpandingLabel description;
	}
}