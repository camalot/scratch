using Growl.DisplayStyle;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Growl.Display.Windows10 {
	public class WindowsGrowlDisplaySettings : SettingsPanelBase {
		//private IContainer components;

		private Panel panel1;

		private RadioButton radioUpperLeft;

		private RadioButton radioLowerRight;

		private RadioButton radioLowerLeft;

		private RadioButton radioUpperRight;
		private PictureBox bgColorPicker;
		private Label label1;
		private Label label2;
		private PictureBox titleColorPicker;
		private Label label3;
		private PictureBox descColorPicker;
		private Panel panel2;
		private Label label4;
		private NumericUpDown annimationSpeed;
		private Button reset;
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.radioLowerRight = new System.Windows.Forms.RadioButton();
			this.radioLowerLeft = new System.Windows.Forms.RadioButton();
			this.radioUpperRight = new System.Windows.Forms.RadioButton();
			this.radioUpperLeft = new System.Windows.Forms.RadioButton();
			this.lblScreenCorners = new System.Windows.Forms.Label();
			this.bgColorPicker = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.titleColorPicker = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.descColorPicker = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.annimationSpeed = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.reset = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bgColorPicker)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.titleColorPicker)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.descColorPicker)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.annimationSpeed)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.radioLowerRight);
			this.panel1.Controls.Add(this.radioLowerLeft);
			this.panel1.Controls.Add(this.radioUpperRight);
			this.panel1.Controls.Add(this.radioUpperLeft);
			this.panel1.Location = new System.Drawing.Point(7, 7);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(77, 67);
			this.panel1.TabIndex = 2;
			// 
			// radioLowerRight
			// 
			this.radioLowerRight.AutoSize = true;
			this.radioLowerRight.Checked = true;
			this.radioLowerRight.Location = new System.Drawing.Point(58, 49);
			this.radioLowerRight.Name = "radioLowerRight";
			this.radioLowerRight.Size = new System.Drawing.Size(14, 13);
			this.radioLowerRight.TabIndex = 3;
			this.radioLowerRight.TabStop = true;
			this.radioLowerRight.UseVisualStyleBackColor = true;
			this.radioLowerRight.CheckedChanged += new System.EventHandler(this.radioScreenCorners_CheckedChanged);
			// 
			// radioLowerLeft
			// 
			this.radioLowerLeft.AutoSize = true;
			this.radioLowerLeft.Location = new System.Drawing.Point(3, 49);
			this.radioLowerLeft.Name = "radioLowerLeft";
			this.radioLowerLeft.Size = new System.Drawing.Size(14, 13);
			this.radioLowerLeft.TabIndex = 2;
			this.radioLowerLeft.UseVisualStyleBackColor = true;
			this.radioLowerLeft.CheckedChanged += new System.EventHandler(this.radioScreenCorners_CheckedChanged);
			// 
			// radioUpperRight
			// 
			this.radioUpperRight.AutoSize = true;
			this.radioUpperRight.Location = new System.Drawing.Point(58, 3);
			this.radioUpperRight.Name = "radioUpperRight";
			this.radioUpperRight.Size = new System.Drawing.Size(14, 13);
			this.radioUpperRight.TabIndex = 1;
			this.radioUpperRight.UseVisualStyleBackColor = true;
			this.radioUpperRight.CheckedChanged += new System.EventHandler(this.radioScreenCorners_CheckedChanged);
			// 
			// radioUpperLeft
			// 
			this.radioUpperLeft.AutoSize = true;
			this.radioUpperLeft.Location = new System.Drawing.Point(3, 3);
			this.radioUpperLeft.Name = "radioUpperLeft";
			this.radioUpperLeft.Size = new System.Drawing.Size(14, 13);
			this.radioUpperLeft.TabIndex = 0;
			this.radioUpperLeft.UseVisualStyleBackColor = true;
			this.radioUpperLeft.CheckedChanged += new System.EventHandler(this.radioScreenCorners_CheckedChanged);
			// 
			// lblScreenCorners
			// 
			this.lblScreenCorners.AutoSize = true;
			this.lblScreenCorners.Location = new System.Drawing.Point(90, 7);
			this.lblScreenCorners.Name = "lblScreenCorners";
			this.lblScreenCorners.Size = new System.Drawing.Size(141, 13);
			this.lblScreenCorners.TabIndex = 3;
			this.lblScreenCorners.Text = "Notification Screen Location";
			// 
			// bgColorPicker
			// 
			this.bgColorPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.bgColorPicker.Location = new System.Drawing.Point(7, 83);
			this.bgColorPicker.Name = "bgColorPicker";
			this.bgColorPicker.Size = new System.Drawing.Size(24, 24);
			this.bgColorPicker.TabIndex = 4;
			this.bgColorPicker.TabStop = false;
			this.bgColorPicker.Click += new System.EventHandler(this.bgColorPicker_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(41, 89);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Notification Background Color";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(41, 118);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(134, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Notification Title Text Color";
			// 
			// titleColorPicker
			// 
			this.titleColorPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.titleColorPicker.Location = new System.Drawing.Point(7, 112);
			this.titleColorPicker.Name = "titleColorPicker";
			this.titleColorPicker.Size = new System.Drawing.Size(24, 24);
			this.titleColorPicker.TabIndex = 6;
			this.titleColorPicker.TabStop = false;
			this.titleColorPicker.Click += new System.EventHandler(this.titleColorPicker_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(41, 147);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(167, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Notification Description Text Color";
			// 
			// descColorPicker
			// 
			this.descColorPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.descColorPicker.Location = new System.Drawing.Point(7, 141);
			this.descColorPicker.Name = "descColorPicker";
			this.descColorPicker.Size = new System.Drawing.Size(24, 24);
			this.descColorPicker.TabIndex = 8;
			this.descColorPicker.TabStop = false;
			this.descColorPicker.Click += new System.EventHandler(this.descColorPicker_Click);
			// 
			// panel2
			// 
			this.panel2.AutoScroll = true;
			this.panel2.Controls.Add(this.reset);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.annimationSpeed);
			this.panel2.Controls.Add(this.panel1);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.lblScreenCorners);
			this.panel2.Controls.Add(this.descColorPicker);
			this.panel2.Controls.Add(this.bgColorPicker);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.titleColorPicker);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(666, 318);
			this.panel2.TabIndex = 10;
			// 
			// annimationSpeed
			// 
			this.annimationSpeed.Location = new System.Drawing.Point(7, 194);
			this.annimationSpeed.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			this.annimationSpeed.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.annimationSpeed.Name = "annimationSpeed";
			this.annimationSpeed.Size = new System.Drawing.Size(224, 20);
			this.annimationSpeed.TabIndex = 10;
			this.annimationSpeed.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.annimationSpeed.Visible = false;
			this.annimationSpeed.ValueChanged += new System.EventHandler(this.annimationSpeed_ValueChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 178);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(153, 13);
			this.label4.TabIndex = 11;
			this.label4.Text = "Animation Speed (Milliseconds)";
			this.label4.Visible = false;
			// 
			// reset
			// 
			this.reset.Location = new System.Drawing.Point(255, 89);
			this.reset.Name = "reset";
			this.reset.Size = new System.Drawing.Size(101, 23);
			this.reset.TabIndex = 12;
			this.reset.Text = "&Reset";
			this.reset.UseVisualStyleBackColor = true;
			this.reset.Click += new System.EventHandler(this.reset_Click);
			// 
			// WindowsGrowlDisplaySettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel2);
			this.Name = "WindowsGrowlDisplaySettings";
			this.Size = new System.Drawing.Size(666, 318);
			this.Load += new System.EventHandler(this.WindowsGrowlDisplaySettings_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.bgColorPicker)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.titleColorPicker)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.descColorPicker)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.annimationSpeed)).EndInit();
			this.ResumeLayout(false);

		}

		private void WindowsGrowlDisplaySettings_Load ( object sender, EventArgs e ) {

			var settings = base.GetSettings ( );
			if ( settings.ContainsKey ( SettingsHelper.SETTING_DISPLAYLOCATION ) ) {
				try {
					switch ( Convert.ToInt32 ( settings[SettingsHelper.SETTING_DISPLAYLOCATION] ) ) {
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

				this.bgColorPicker.BackColor = SettingsHelper.GetBackColorFromSetting ( this );
				this.titleColorPicker.BackColor = SettingsHelper.GetTitleColorFromSetting ( this );
				this.descColorPicker.BackColor = SettingsHelper.GetDescColorFromSetting ( this );
				this.annimationSpeed.Value = SettingsHelper.GetSpeedFromSetting ( this );
			}
		}

		private void radioScreenCorners_CheckedChanged ( object sender, EventArgs e ) {
			if ( sender is RadioButton ) {
				string name = ( (RadioButton)sender ).Name;
				if ( name == "radioUpperLeft" ) {
					base.SaveSetting ( SettingsHelper.SETTING_DISPLAYLOCATION, WindowsGrowlVisualDisplayLocation.TopLeft );
					return;
				}
				if ( name == "radioUpperRight" ) {
					base.SaveSetting ( SettingsHelper.SETTING_DISPLAYLOCATION, WindowsGrowlVisualDisplayLocation.TopRight );
					return;
				}
				if ( name == "radioLowerLeft" ) {
					base.SaveSetting ( SettingsHelper.SETTING_DISPLAYLOCATION, WindowsGrowlVisualDisplayLocation.BottomLeft );
					return;
				}
				base.SaveSetting ( SettingsHelper.SETTING_DISPLAYLOCATION, WindowsGrowlVisualDisplayLocation.BottomRight );
			}
		}

		private void bgColorPicker_Click ( object sender, EventArgs e ) {
			var defaultColor = SettingsHelper.GetBackColorFromSetting ( this );
			var colorDialog = new ColorDialog {
				AllowFullOpen = true,
				AnyColor = true,
				Color = defaultColor,
				SolidColorOnly = true,
				ShowHelp = false,
				FullOpen = true,
				CustomColors = new int[] { ColorTranslator.ToWin32( SettingsHelper.DEFAULT_BGCOLOR ), ColorTranslator.ToWin32 ( defaultColor ) }
			};
			var result = colorDialog.ShowDialog ( this.ParentForm );
			if(result == DialogResult.OK) {
				var selected = colorDialog.Color;
				base.SaveSetting ( SettingsHelper.SETTING_BGCOLOR, selected );
				this.bgColorPicker.BackColor = selected;
			}
		}

		private void titleColorPicker_Click ( object sender, EventArgs e ) {
			var defaultColor = SettingsHelper.GetTitleColorFromSetting ( this );
			var colorDialog = new ColorDialog {
				AllowFullOpen = true,
				AnyColor = true,
				Color = defaultColor,
				SolidColorOnly = true,
				ShowHelp = false,
				FullOpen = true,
				CustomColors = new int[] { ColorTranslator.ToWin32 ( SettingsHelper.DEFAULT_TITLECOLOR ), ColorTranslator.ToWin32 ( defaultColor ) }
			};
			var result = colorDialog.ShowDialog ( this.ParentForm );
			if ( result == DialogResult.OK ) {
				var selected = colorDialog.Color;
				base.SaveSetting ( SettingsHelper.SETTING_TITLECOLOR, selected );
				this.titleColorPicker.BackColor = selected;
			}
		}

		private void descColorPicker_Click ( object sender, EventArgs e ) {
			var defaultColor = SettingsHelper.GetDescColorFromSetting ( this );
			var colorDialog = new ColorDialog {
				AllowFullOpen = true,
				AnyColor = true,
				Color = defaultColor,
				SolidColorOnly = true,
				ShowHelp = false,
				FullOpen = true,
				CustomColors = new int[] { ColorTranslator.ToWin32 ( SettingsHelper.DEFAULT_DESCCOLOR ), ColorTranslator.ToWin32 ( defaultColor ) }
			};
			var result = colorDialog.ShowDialog ( this.ParentForm );
			if ( result == DialogResult.OK ) {
				var selected = colorDialog.Color;
				base.SaveSetting ( SettingsHelper.SETTING_DESCCOLOR, selected );
				this.descColorPicker.BackColor = selected;
			}
		}

		private void annimationSpeed_ValueChanged ( object sender, EventArgs e ) {
			base.SaveSetting ( SettingsHelper.SETTING_SPEED, this.annimationSpeed.Value );
		}

		private void reset_Click ( object sender, EventArgs e ) {

			base.SaveSetting ( SettingsHelper.SETTING_DESCCOLOR, SettingsHelper.DEFAULT_DESCCOLOR );
			this.descColorPicker.BackColor = SettingsHelper.DEFAULT_DESCCOLOR;
			base.SaveSetting ( SettingsHelper.SETTING_TITLECOLOR, SettingsHelper.DEFAULT_TITLECOLOR );
			this.titleColorPicker.BackColor = SettingsHelper.DEFAULT_TITLECOLOR;
			base.SaveSetting ( SettingsHelper.SETTING_BGCOLOR, SettingsHelper.DEFAULT_BGCOLOR );
			this.bgColorPicker.BackColor = SettingsHelper.DEFAULT_BGCOLOR;

			this.annimationSpeed.Value = SettingsHelper.DEFAULT_SPEED;
			base.SaveSetting ( SettingsHelper.SETTING_SPEED, SettingsHelper.DEFAULT_SPEED );

		}
	}
}
