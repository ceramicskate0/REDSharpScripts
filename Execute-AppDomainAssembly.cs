    public static string Execute(string Arguments = "")
    {

        string output = "";
        TextWriter realStdOut = Console.Out;
        TextWriter realStdErr = Console.Error;
        TextWriter stdOutWriter = new StringWriter();
        TextWriter stdErrWriter = new StringWriter();
        Console.SetOut(stdOutWriter);
        Console.SetError(stdErrWriter);

        AppDomain.CurrentDomain.AssemblyResolve += (sender, args2) =>
        {
            String resourceName = new AssemblyName(args2.Name).Name + ".dll";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                Byte[] assemblyData = new Byte[stream.Length];

                stream.Read(assemblyData, 0, assemblyData.Length);

                var loadedAssembly = Assembly.Load(assemblyData);

                return loadedAssembly;
            }
        };


        string[] args = Arguments.Split(' ');

		// For some reason SharpZipLib cannot get this constant as it defaults to 1 in my tests
        // So therefore it needs to be hardcoded to 850 (https://en.wikipedia.org/wiki/Code_page_850)
		// We need to invoke FixCodePage because we need it to load the assembly the correct way
        typeof(Task).GetMethod("FixCodePage", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);

        typeof(Sharphound).GetMethod("Main", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { args });


        Console.Out.Flush();
        Console.Error.Flush();
        Console.SetOut(realStdOut);
        Console.SetError(realStdErr);

        output += stdOutWriter.ToString();
        output += stdErrWriter.ToString();
        return output;
    }

    public static void FixCodePage()
    {
        ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = 850;
    }
