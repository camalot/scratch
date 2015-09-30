using System.Collections.Generic;
using Growl.Connector;
using Growl.Destinations;

namespace Growl.Forwarder.Windows10 {
	public class NotificationCenterDestination : ForwardDestination {
		//NotifyIcon notify;
		public NotificationCenterDestination ( string name ) : base(name, true ) {
			//notify = new NotifyIcon ( );
		}
		public override string AddressDisplay {
			get {
				return "Windows 10 Notification Center";
			}
		}
		public override bool Available {
			get {
				return true;
				//return Environment.OSVersion.Version.Major >= 10;
			}

			protected set {
				
			}
		}

		public override DestinationBase Clone ( ) {
			return new NotificationCenterDestination ( this.Description );
		}

		public override void ForwardNotification ( Notification notification, CallbackContext callbackContext, RequestInfo requestInfo, bool isIdle, ForwardedNotificationCallbackHandler callbackFunction ) {
			//notify.ShowBalloonTip ( 4000, notification.Title, notification.Text, ToolTipIcon.Info );
		}

		public override void ForwardRegistration ( Connector.Application application, List<NotificationType> notificationTypes, RequestInfo requestInfo, bool isIdle ) {
			// do nothing
		}

		public override System.Drawing.Image GetIcon ( ) {
			return NotificationCenterForwardHandler.GetIcon ( );
		}
	}
}
