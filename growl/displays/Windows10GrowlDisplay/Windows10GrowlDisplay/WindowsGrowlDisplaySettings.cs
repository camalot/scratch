using Growl.DisplayStyle;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Growl.Display.Windows10 {
	public class WindowsGrowlDisplaySettings : SettingsPanelBase {
		public const string SETTING_DISPLAYLOCATION = "DisplayLocation";

		//private IContainer components;

		private Panel panel1;

		private RadioButton radioUpperLeft;

		private RadioButton radioLowerRight;

		private RadioButton radioLowerLeft;

		private RadioButton radioUpperRight;

		private Label lblScreenCorners;

		public WindowsGrowlDisplaySettings ( ) {
			this.InitializeComponent ( );
		}

		protected override void Dispose ( bool disposing ) {
			//if ( disposing && this.components != null ) {
			//	this.components.Dispose ( );
			//}
			base.Dispose ( disposing );
		}

		private void InitializeComponent ( ) {
			this.panel1 = new Panel ( );
			this.radioLowerRight = new RadioButton ( );
			this.radioLowerLeft = new RadioButton ( );
			this.radioUpperRight = new RadioButton ( );
			this.radioUpperLeft = new RadioButton ( );
			this.lblScreenCorners = new Label ( );
			this.panel1.SuspendLayout ( );
			base.SuspendLayout ( );
			this.panel1.BorderStyle = BorderStyle.FixedSingle;
			this.panel1.Controls.Add ( this.radioLowerRight );
			this.panel1.Controls.Add ( this.radioLowerLeft );
			this.panel1.Controls.Add ( this.radioUpperRight );
			this.panel1.Controls.Add ( this.radioUpperLeft );
			this.panel1.Location = new Point ( 18, 10 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size ( 77, 67 );
			this.panel1.TabIndex = 2;
			this.radioLowerRight.AutoSize = true;
			this.radioLowerRight.Checked = true;
			this.radioLowerRight.Location = new Point ( 58, 49 );
			this.radioLowerRight.Name = "radioLowerRight";
			this.radioLowerRight.Size = new Size ( 14, 13 );
			this.radioLowerRight.TabIndex = 3;
			this.radioLowerRight.TabStop = true;
			this.radioLowerRight.UseVisualStyleBackColor = true;
			this.radioLowerRight.CheckedChanged += new EventHandler ( this.radioScreenCorners_CheckedChanged );
			this.radioLowerLeft.AutoSize = true;
			this.radioLowerLeft.Location = new Point ( 3, 49 );
			this.radioLowerLeft.Name = "radioLowerLeft";
			this.radioLowerLeft.Size = new Size ( 14, 13 );
			this.radioLowerLeft.TabIndex = 2;
			this.radioLowerLeft.UseVisualStyleBackColor = true;
			this.radioLowerLeft.CheckedChanged += new EventHandler ( this.radioScreenCorners_CheckedChanged );
			this.radioUpperRight.AutoSize = true;
			this.radioUpperRight.Location = new Point ( 58, 3 );
			this.radioUpperRight.Name = "radioUpperRight";
			this.radioUpperRight.Size = new Size ( 14, 13 );
			this.radioUpperRight.TabIndex = 1;
			this.radioUpperRight.UseVisualStyleBackColor = true;
			this.radioUpperRight.CheckedChanged += new EventHandler ( this.radioScreenCorners_CheckedChanged );
			this.radioUpperLeft.AutoSize = true;
			this.radioUpperLeft.Location = new Point ( 3, 3 );
			this.radioUpperLeft.Name = "radioUpperLeft";
			this.radioUpperLeft.Size = new Size ( 14, 13 );
			this.radioUpperLeft.TabIndex = 0;
			this.radioUpperLeft.UseVisualStyleBackColor = true;
			this.radioUpperLeft.CheckedChanged += new EventHandler ( this.radioScreenCorners_CheckedChanged );
			this.lblScreenCorners.AutoSize = true;
			this.lblScreenCorners.Location = new Point ( 104, 14 );
			this.lblScreenCorners.MaximumSize = new Size ( 130, 0 );
			this.lblScreenCorners.Name = "lblScreenCorners";
			this.lblScreenCorners.Size = new Size ( 112, 52 );
			this.lblScreenCorners.TabIndex = 3;
			this.lblScreenCorners.Text = "Which corner of the screen would you like your notifications to appear?";
			base.AutoScaleDimensions = new SizeF ( 6f, 13f );
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add ( this.lblScreenCorners );
			base.Controls.Add ( this.panel1 );
			this.Load += WindowsGrowlDisplaySettings_Load;
			base.Name = "WindowsGrowlDisplaySettings";
			this.panel1.ResumeLayout ( false );
			this.panel1.PerformLayout ( );
			base.ResumeLayout ( false );
			base.PerformLayout ( );
		}

		private void WindowsGrowlDisplaySettings_Load ( object sender, EventArgs e ) {

			var settings = base.GetSettings ( );
			if ( settings.ContainsKey ( SETTING_DISPLAYLOCATION ) ) {
				try {
					switch ( Convert.ToInt32 ( settings[SETTING_DISPLAYLOCATION] ) ) {
						case 1:
							{
								this.radioUpperLeft.Checked = true;
								break;
							}
						case 2:
							{
								this.radioUpperRight.Checked = true;
								break;
							}
						case 3:
							{
								this.radioLowerLeft.Checked = true;
								break;
							}
						default:
							{
								this.radioLowerRight.Checked = true;
								break;
							}
					}
				} catch {
				}
			}
		}

		private void radioScreenCorners_CheckedChanged ( object sender, EventArgs e ) {
			if ( sender is RadioButton ) {
				string name = ( (RadioButton)sender ).Name;
				if ( name == "radioUpperLeft" ) {
					base.SaveSetting ( SETTING_DISPLAYLOCATION, WindowsGrowlVisualDisplayLocation.TopLeft );
					return;
				}
				if ( name == "radioUpperRight" ) {
					base.SaveSetting ( SETTING_DISPLAYLOCATION, WindowsGrowlVisualDisplayLocation.TopRight );
					return;
				}
				if ( name == "radioLowerLeft" ) {
					base.SaveSetting ( SETTING_DISPLAYLOCATION, WindowsGrowlVisualDisplayLocation.BottomLeft );
					return;
				}
				base.SaveSetting ( SETTING_DISPLAYLOCATION, WindowsGrowlVisualDisplayLocation.BottomRight );
			}
		}
	}
}
