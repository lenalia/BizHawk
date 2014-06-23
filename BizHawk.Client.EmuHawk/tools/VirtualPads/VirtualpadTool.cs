﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using BizHawk.Client.Common;

namespace BizHawk.Client.EmuHawk
{
	public partial class VirtualpadTool : Form, IToolForm
	{
		private int _defaultWidth;
		private int _defaultHeight;

		private List<VirtualPad> Pads
		{
			get
			{
				return ControllerBox.Controls
					.OfType<VirtualPad>()
					.ToList();
			}
		}

		public VirtualpadTool()
		{
			InitializeComponent();
			Closing += (o, e) => SaveConfigSettings();
			TopMost = Global.Config.VirtualPadSettings.TopMost;
		}

		private void VirtualpadTool_Load(object sender, EventArgs e)
		{
			_defaultWidth = Size.Width;
			_defaultHeight = Size.Height;

			StickyBox.Checked = Global.Config.VirtualPadSticky;

			if (Global.Config.VirtualPadSettings.UseWindowPosition)
			{
				Location = Global.Config.VirtualPadSettings.WindowPosition;
			}

			if (Global.Config.VirtualPadSettings.UseWindowPosition)
			{
				Size = Global.Config.VirtualPadSettings.WindowSize;
			}

			CreatePads();
		}

		public void ClearVirtualPadHolds()
		{
			ControllerBox.Controls
				.OfType<IVirtualPad>()
				.ToList()
				.ForEach(pad => pad.Clear());
		}

		public void BumpAnalogValue(int? dx, int? dy) // TODO: multi-player
		{

		}

		private void CreatePads()
		{
			ControllerBox.Controls.Clear();

			// TODO: be more clever than this
			switch(Global.Emulator.SystemId)
			{
				case "NES":
					ControllerBox.Controls.AddRange(new NesSchema().GetPads().ToArray());
					break;
				case "N64":
					ControllerBox.Controls.Add(new VirtualPad(
						N64Schema.StandardController(1))
					{
						Location = new Point(15, 15)
					});
					break;
				case "SMS":
				case "SG":
				case "GG": // TODO: test if all 3 of these are needed
					ControllerBox.Controls.Add(new VirtualPad(
						SmsSchema.StandardController(1))
					{
						Location = new Point(15, 15)
					});
					break;
				case "PCE":
					ControllerBox.Controls.Add(new VirtualPad(
						PceSchema.StandardController(1))
					{
						Location = new Point(15, 15)
					});
					break;
				case "SNES":
					ControllerBox.Controls.Add(new VirtualPad(
						SnesSchema.StandardController(1))
					{
						Location = new Point(15, 15)
					});
					break;
			}
		}

		private void SaveConfigSettings()
		{
			Global.Config.VirtualPadSettings.Wndx = Location.X;
			Global.Config.VirtualPadSettings.Wndy = Location.Y;
			Global.Config.VirtualPadSettings.Width = Right - Left;
			Global.Config.VirtualPadSettings.Height = Bottom - Top;

			Global.Config.VirtualPadSticky = StickyBox.Checked;
		}

		private void RefreshFloatingWindowControl()
		{
			Owner = Global.Config.VirtualPadSettings.FloatingWindow ? null : GlobalWin.MainForm;
		}

		#region IToolForm Implementation

		public bool AskSave() { return true; }
		public bool UpdateBefore { get { return false; } }

		public void Restart()
		{
			if (!IsHandleCreated || IsDisposed)
			{
				return;
			}

			CreatePads();
		}

		public void UpdateValues()
		{
			if (!IsHandleCreated || IsDisposed)
			{
				return;
			}

			Pads.ForEach(p => p.Refresh());

			if (!StickyBox.Checked)
			{
				Pads.ForEach(pad => pad.Clear());
			}
		}

		#endregion

		#region Events

		#region Menu

		private void OptionsSubMenu_DropDownOpened(object sender, EventArgs e)
		{
			AutoloadMenuItem.Checked = Global.Config.AutoloadVirtualPad;
			SaveWindowPositionMenuItem.Checked = Global.Config.VirtualPadSettings.SaveWindowPosition;
			AlwaysOnTopMenuItem.Checked = Global.Config.VirtualPadSettings.TopMost;
			FloatingWindowMenuItem.Checked = Global.Config.VirtualPadSettings.FloatingWindow;
		}

		private void AutoloadMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.AutoloadVirtualPad ^= true;
		}

		private void SaveWindowPositionMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.VirtualPadSettings.SaveWindowPosition ^= true;
		}

		private void AlwaysOnTopMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.VirtualPadSettings.TopMost ^= true;
			TopMost = Global.Config.VirtualPadSettings.TopMost;
		}

		private void FloatingWindowMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.VirtualPadSettings.FloatingWindow ^= true;
			RefreshFloatingWindowControl();
		}

		private void RestoreDefaultSettingsMenuItem_Click(object sender, EventArgs e)
		{
			Size = new Size(_defaultWidth, _defaultHeight);

			Global.Config.VirtualPadSettings.SaveWindowPosition = true;
			Global.Config.VirtualPadSettings.TopMost = TopMost = false;
			Global.Config.VirtualPadSettings.FloatingWindow = false;
			Global.Config.VirtualPadMultiplayerMode = false;
		}

		private void ExitMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void PadsSubMenu_DropDownOpened(object sender, EventArgs e)
		{
			StickyMenuItem.Checked = StickyBox.Checked;
		}

		private void ClearAllMenuItem_Click(object sender, EventArgs e)
		{
			ClearVirtualPadHolds();
		}

		private void StickyMenuItem_Click(object sender, EventArgs e)
		{
			StickyBox.Checked ^= true;
		}

		#endregion

		#endregion
	}
}