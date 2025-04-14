using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DOSA_Client.lib;
using DOSA_Client.lib.Constants;
using DOSA_Client.Models;
public class MainWindowViewModel : INotifyPropertyChanged
{
    private bool _showContainer;
    public bool ShowContainer
    {
        get => _showContainer;
        set
        {
            _showContainer = value;
            OnPropertyChanged(nameof(ShowContainer));
        }
    }

    private bool _showLogin;
    public bool ShowLogin
    {
        get => _showLogin;
        set
        {
            _showLogin = value;
            OnPropertyChanged(nameof(ShowLogin));
        }
    }

    public bool Show { get; set; }

    public ICommand toggleLoginVisibility { get; }

    public ICommand LoginCommand { get; }

    public String LoginViewDescription { get; set; } = "Where the system is never down...";

    public MainWindowViewModel()
    {
        ShowContainer = false;
        ShowLogin = true;
        toggleLoginVisibility = new RelayCommand(toggleLogin);
        LoginCommand = new RelayCommand(OnLoginAsync);
    }

    public async void OnLoginAsync()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:3000/");
        listener.Start();

        Process.Start(new ProcessStartInfo
        {
            FileName = Constants.googleAuthURI, 
            UseShellExecute = true
        });

        var context = await listener.GetContextAsync();
        var request = context.Request;

        string authCode = request.QueryString["code"] ?? throw new Exception("Auth Code could not be read from the request");
        

        string responseString = "<html><body>You have successfully logged in, you may close this window now :).</body></html>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();
        listener.Stop();
        Console.WriteLine($"Got auth code: {authCode}");

        // now that we have the auth code we should do the exhange for the jwt, then extract the google_id, write that to context
        // then we get the user profile for that google id and then write that to context, then we toggle the visibility of the other tabs
        // lekka

        // throw the exception deeper in the api itself rather?
        string jwt = await ApiClient.ExchangeAuthCodeForJWT(authCode) ?? throw new Exception("Failed to exchange the jwt");
        // set the jwt on the client
        ApiClient.Jwt = jwt;
        RestClient.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiClient.Jwt);

        
        string googleID = claimsDict["sub"].ToString() ?? throw new Exception("Google ID not found in the jwt");
        
        // Add the user to context
        Context.Add(ContextKeys.USER, await ApiClient.GetUserProfile(googleID));
        Console.WriteLine(jwt);
        
        // now hide this page
        toggleLogin();
    }

    public void toggleLogin()
    {
        Console.WriteLine("Toggling the things");
        ShowLogin = !ShowLogin;
        ShowContainer = !ShowContainer;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
