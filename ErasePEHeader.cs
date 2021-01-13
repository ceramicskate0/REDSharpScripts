private static int ErasePEHeader()
{
	SYSTEM_INFO sys_info = new SYSTEM_INFO();
	GetSystemInfo(out sys_info);
	UIntPtr proc_min_address = sys_info.minimumApplicationAddress;
	UIntPtr proc_max_address = sys_info.maximumApplicationAddress;
	ulong proc_min_address_l = (ulong)proc_min_address;
	ulong proc_max_address_l = (ulong)proc_max_address;
	Process currentProcess = Process.GetCurrentProcess();
	MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
	VirtualQueryEx(currentProcess.Handle, proc_min_address, out mem_basic_info, Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
	proc_min_address_l += mem_basic_info.RegionSize;
	proc_min_address = new UIntPtr(proc_min_address_l);
	VirtualQueryEx(currentProcess.Handle, proc_min_address, out mem_basic_info, Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
	Console.WriteLine("Base Address: 0x{0}", (mem_basic_info.BaseAddress).ToString("X"));
	bool result = VirtualProtect((UIntPtr)mem_basic_info.BaseAddress, (UIntPtr)4096, (uint)MemoryProtectionConsts.READWRITE, out uint oldProtect);
	FillMemory((UIntPtr)mem_basic_info.BaseAddress, 132, 0);
	Console.WriteLine("PE Header overwritten at 0x{0}", (mem_basic_info.BaseAddress).ToString("X"));
	return 0;
}
