// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Coursework
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSPopUpButton interpolationMethod { get; set; }

		[Outlet]
		AppKit.NSTextField polynomialTextField { get; set; }

		[Outlet]
		AppKit.NSTextField practicalComplexityTextField { get; set; }

		[Outlet]
		AppKit.NSPopUpButton systemSize { get; set; }

		[Outlet]
		AppKit.NSTextField x1 { get; set; }

		[Outlet]
		AppKit.NSTextField x10 { get; set; }

		[Outlet]
		AppKit.NSTextField x2 { get; set; }

		[Outlet]
		AppKit.NSTextField x3 { get; set; }

		[Outlet]
		AppKit.NSTextField x4 { get; set; }

		[Outlet]
		AppKit.NSTextField x5 { get; set; }

		[Outlet]
		AppKit.NSTextField x6 { get; set; }

		[Outlet]
		AppKit.NSTextField x7 { get; set; }

		[Outlet]
		AppKit.NSTextField x8 { get; set; }

		[Outlet]
		AppKit.NSTextField x9 { get; set; }

		[Outlet]
		AppKit.NSTextField y1 { get; set; }

		[Outlet]
		AppKit.NSTextField y10 { get; set; }

		[Outlet]
		AppKit.NSTextField y2 { get; set; }

		[Outlet]
		AppKit.NSTextField y3 { get; set; }

		[Outlet]
		AppKit.NSTextField y4 { get; set; }

		[Outlet]
		AppKit.NSTextField y5 { get; set; }

		[Outlet]
		AppKit.NSTextField y6 { get; set; }

		[Outlet]
		AppKit.NSTextField y7 { get; set; }

		[Outlet]
		AppKit.NSTextField y8 { get; set; }

		[Outlet]
		AppKit.NSTextField y9 { get; set; }

		[Action ("calculateButton:")]
		partial void calculateButton (Foundation.NSObject sender);

		[Action ("clearFieldsButton:")]
		partial void clearFieldsButton (Foundation.NSObject sender);

		[Action ("generateSystemButton:")]
		partial void generateSystemButton (Foundation.NSObject sender);

		[Action ("saveToFileButton:")]
		partial void saveToFileButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (interpolationMethod != null) {
				interpolationMethod.Dispose ();
				interpolationMethod = null;
			}

			if (polynomialTextField != null) {
				polynomialTextField.Dispose ();
				polynomialTextField = null;
			}

			if (practicalComplexityTextField != null) {
				practicalComplexityTextField.Dispose ();
				practicalComplexityTextField = null;
			}

			if (systemSize != null) {
				systemSize.Dispose ();
				systemSize = null;
			}

			if (x1 != null) {
				x1.Dispose ();
				x1 = null;
			}

			if (x10 != null) {
				x10.Dispose ();
				x10 = null;
			}

			if (x2 != null) {
				x2.Dispose ();
				x2 = null;
			}

			if (x3 != null) {
				x3.Dispose ();
				x3 = null;
			}

			if (x4 != null) {
				x4.Dispose ();
				x4 = null;
			}

			if (x5 != null) {
				x5.Dispose ();
				x5 = null;
			}

			if (x6 != null) {
				x6.Dispose ();
				x6 = null;
			}

			if (x7 != null) {
				x7.Dispose ();
				x7 = null;
			}

			if (x8 != null) {
				x8.Dispose ();
				x8 = null;
			}

			if (x9 != null) {
				x9.Dispose ();
				x9 = null;
			}

			if (y1 != null) {
				y1.Dispose ();
				y1 = null;
			}

			if (y10 != null) {
				y10.Dispose ();
				y10 = null;
			}

			if (y2 != null) {
				y2.Dispose ();
				y2 = null;
			}

			if (y3 != null) {
				y3.Dispose ();
				y3 = null;
			}

			if (y4 != null) {
				y4.Dispose ();
				y4 = null;
			}

			if (y5 != null) {
				y5.Dispose ();
				y5 = null;
			}

			if (y6 != null) {
				y6.Dispose ();
				y6 = null;
			}

			if (y7 != null) {
				y7.Dispose ();
				y7 = null;
			}

			if (y8 != null) {
				y8.Dispose ();
				y8 = null;
			}

			if (y9 != null) {
				y9.Dispose ();
				y9 = null;
			}
		}
	}
}
