﻿using System;
using OKHOSTING.UI.Controls;
using NativeControl = System.Web.UI.Control;

namespace OKHOSTING.UI.Net4.WebForms.Controls
{
	/// <summary>
	/// It is a control that represents a calendar
	/// <para xml:lang="es">Es un control que representa un calendario</para>
	/// </summary>
	public class DatePicker : TextBoxBase, IDatePicker, IWebInputControl
	{
		public DatePicker()
		{
			Attributes["pattern"] = @"(0[1-9]|1[0-9]|2[0-9]|3[01]).(0[1-9]|1[012]).[0-9]{4}";
			Attributes["placeholder"] = @"dd/mm/yyyy";
		}

		#region IInputControl

		/// <summary>
		/// Gets or sets the OKHOSTING.UI.Controls.IInputControl<System.DateTime?>. value.
		/// </summary>
		DateTime? IInputControl<DateTime?>.Value
		{
			get
			{
				var values = base.Text?.Split('/');

				try
				{
					return new DateTime(int.Parse(values[2]), int.Parse(values[1]), int.Parse(values[0]));
				}
				catch
				{
					return null;
				}
			}
			set
			{
				if (value != null)
				{
					base.Text = value.Value.ToString("dd/MM/yyyy");
				}
				else
				{
					base.Text = null;
				}
			}
		}

		/// <summary>
		/// Occurs when value changed.
		/// </summary>
		public event EventHandler<DateTime?> ValueChanged;

		/// <summary>
		/// Raises the value changed.
		/// </summary>
		/// <returns>The value changed.</returns>
		protected internal void RaiseValueChanged()
		{
			ValueChanged?.Invoke(this, ((IInputControl<DateTime?>) this).Value);
		}

		#endregion

		#region IWebInputControl

		bool IWebInputControl.HandlePostBack()
		{
			DateTime date = DateTime.MinValue;
			string postedValue = Page.Request.Form[ID];

			if (base.Text != postedValue)
			{
				base.Text = postedValue;
				return true;
			}
			else
			{
				return false;
			}
		}

		void IWebInputControl.RaiseValueChanged()
		{
			ValueChanged?.Invoke(this, ((IDatePicker) this).Value);
		}

		#endregion

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			string js = string.Format
			(
				@"
				<script type='text/javascript'>
					$(document).ready
					(
						function()
						{{
							$(""#{0}"").datepicker($.datepicker.regional[""es""]);
						}}
					);
				</script>"
			, ClientID
			);

			Page.ClientScript.RegisterStartupScript(GetType(), "datepicker_" + base.ClientID, js);
		}
	}
}