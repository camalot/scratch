using System;
using System.Collections.Generic;
using System.Text;
using Growl.Destinations;

namespace Growl.Forwarder.Windows10 {
	public class NotificationCenterSettingsPanel : DestinationSettingsPanel {
		private System.Windows.Forms.Label labelName;
		private Growl.Destinations.HighlightTextBox textBoxName;

		public NotificationCenterSettingsPanel ( ) {
			InitializeComponent ( );
		}

		private void InitializeComponent ( ) {
			this.textBoxName = new Growl.Destinations.HighlightTextBox ( );
			this.labelName = new System.Windows.Forms.Label ( );
			this.panelDetails.SuspendLayout ( );
			this.SuspendLayout ( );
			// 
			// panelDetails
			// 
			this.panelDetails.Controls.Add ( this.labelName );
			this.panelDetails.Controls.Add ( this.textBoxName );

			// 
			// textBoxName
			// 
			this.textBoxName.HighlightColor = System.Drawing.Color.FromArgb ( ( (int)( ( (byte)( 254 ) ) ) ), ( (int)( ( (byte)( 250 ) ) ) ), ( (int)( ( (byte)( 184 ) ) ) ) );
			this.textBoxName.Location = new System.Drawing.Point ( 63, 16 );
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size ( 227, 20 );
			this.textBoxName.TabIndex = 1;
			this.textBoxName.TextChanged += new System.EventHandler ( this.textBoxName_TextChanged );

			// 
			// labelName
			// 
			this.labelName.AutoSize = true;
			this.labelName.Location = new System.Drawing.Point ( 19, 19 );
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size ( 38, 13 );
			this.labelName.TabIndex = 3;
			this.labelName.Text = "Name:";
			// 
			// NotificationCenterSettingsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
			this.Name = "NotificationCenterSettingsPanel";
			this.panelDetails.ResumeLayout ( false );
			this.panelDetails.PerformLayout ( );
			this.ResumeLayout ( false );
		}

		public override void Initialize ( bool isSubscription, DestinationListItem dli, DestinationBase db ) { }

		public override DestinationBase Create ( ) {
			return new NotificationCenterDestination ( textBoxName.Text );
		}

		public override void Update ( DestinationBase db ) {
			NotificationCenterDestination whd = db as NotificationCenterDestination;
			whd.Description = this.textBoxName.Text;
		}

		private void ValidateInputs ( ) {
			bool valid = true;

			// name
			if ( String.IsNullOrEmpty ( this.textBoxName.Text ) ) {
				this.textBoxName.Highlight ( );
				valid = false;
			} else {
				this.textBoxName.Unhighlight ( );
			}

			OnValidChanged ( valid );
		}

		private void textBoxName_TextChanged ( object sender, EventArgs e ) {
			ValidateInputs ( );
		}
	}
}
