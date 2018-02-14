﻿using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Computers.AppleII
{
	public partial class AppleII : IEmulator
	{
		public IEmulatorServiceProvider ServiceProvider { get; }

		public ControllerDefinition ControllerDefinition => AppleIIController;

		public IController Controller { get; set; }

		public int Frame { get; private set; }

		public string SystemId => "AppleII";

		public bool DeterministicEmulation => true;

		public void FrameAdvance(bool render, bool rendersound)
		{
			FrameAdv(render, rendersound);
		}

		public string BoardName => null;

		public void ResetCounters()
		{
			Frame = 0;
			LagCount = 0;
			IsLagFrame = false;
		}

		public CoreComm CoreComm { get; }

		public void Dispose()
		{
			_machine.Dispose();
		}
	}
}
