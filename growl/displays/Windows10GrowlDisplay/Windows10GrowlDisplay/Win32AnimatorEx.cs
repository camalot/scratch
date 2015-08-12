
namespace Growl.Display.Windows10 {
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;
	using DisplayStyle;

	public sealed class Win32AnimatorEx : IAnimator, IDisposable {
		private const int AW_HIDE = 65536;

		private const int AW_ACTIVATE = 131072;

		private const int DEFAULT_DURATION = 500;

		public AnimationDirection DirectionIn { get; set; }
		public AnimationDirection DirectionOut { get; set; }

		public bool Disabled { get; set; }

		public int Duration { get; set; } = DEFAULT_DURATION;
		public Form Form { get; private set; }

		public AnimationMethod Method { get; set; } = AnimationMethod.Slide;

		public Win32AnimatorEx ( Form form ) : this ( form, AnimationMethod.Slide, AnimationDirection.Left, AnimationDirection.Right, DEFAULT_DURATION ) {
		}

		public Win32AnimatorEx ( Form form, AnimationMethod method, int duration ) : this ( form, method, AnimationDirection.Left, AnimationDirection.Right, duration ) {
		}

		public Win32AnimatorEx ( Form form, AnimationMethod method, AnimationDirection directionIn, AnimationDirection directionOut, int duration ) {
			this.Form = form;
			this.Form.VisibleChanged += new EventHandler ( this.Form_VisibleChanged );
			this.Form.Closing += new CancelEventHandler ( this.Form_Closing );
			this.Duration = duration;
			this.DirectionIn = directionIn;
			this.DirectionOut = directionOut;
		}

		[DllImport ( "user32", CharSet = CharSet.None, ExactSpelling = false )]
		private static extern bool AnimateWindow ( IntPtr hWnd, int dwTime, int dwFlags );

		public void Dispose ( ) {
			this.Dispose ( true );
			GC.SuppressFinalize ( this );
		}

		private void Dispose ( bool disposing ) {
			if ( disposing && this.Form != null ) {
				this.Form.Dispose ( );
			}
		}

		private void Form_Closing ( object sender, CancelEventArgs e ) {
			if ( !e.Cancel && !this.Disabled && ( this.Form.MdiParent == null || this.Method != Win32AnimatorEx.AnimationMethod.Blend ) ) {
				AnimateWindow ( this.Form.Handle, this.Duration, AW_HIDE | (int)this.Method | (int)this.DirectionOut );
			}
		}

		private void Form_VisibleChanged ( object sender, EventArgs e ) {
			if ( !this.Disabled && this.Form.MdiParent == null ) {
				var numOut = (int)this.Method | (int)this.DirectionOut;
				var numIn = (int)this.Method | (int)this.DirectionIn;
				var num = ( !this.Form.Visible ? numOut | AW_HIDE : numIn | AW_ACTIVATE );
				AnimateWindow ( this.Form.Handle, this.Duration, num );
			}
		}

		public void CancelClosing ( ) {
			
		}

		[Flags]
		public enum AnimationDirection {
			Right = 1,
			Left = 2,
			Down = 4,
			Up = 8
		}

		public enum AnimationMethod {
			Roll = 0,
			Centre = 16,
			Slide = 262144,
			Blend = 524288
		}
	}
}
