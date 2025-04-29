
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using Syddjurs.Models;

namespace Syddjurs.Pages;

public partial class MyLoansPage : ContentPage
{

    private readonly HttpClient _httpClient;
    public ObservableCollection<LoanForLIstDto> Loans { get; set; }

    private LoanForLIstDto _selectedLoan;

    public LoanForLIstDto SelectedLoan
    {
        get => _selectedLoan;
        set
        {
            _selectedLoan = value;
            OnLoanSelected(value);
            OnPropertyChanged(nameof(SelectedLoan));
        }
    }
    public MyLoansPage()
	{
		InitializeComponent();
        _httpClient = new HttpClient();
        Loans = new ObservableCollection<LoanForLIstDto>();
        Loaded += LoanListPage_Loaded;
        BindingContext = this;
    }

    private void LoanListPage_Loaded(object? sender, EventArgs e)
    {
        GetLoansForList();
    }

    private void OnLoanSelected(LoanForLIstDto selectedLoan)
    {
        if (selectedLoan == null) return;

        foreach (var loan in Loans)
        {
            if (loan.Equals(selectedLoan))
            {
                loan.IsSelected = true;
            }
            else
            {
                loan.IsSelected = false;
            }
        }      
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (SelectedLoan == null) return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "LoanToEdit", SelectedLoan }
                };

        await Shell.Current.GoToAsync("LoanPage", navigationParameter);
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (SelectedLoan == null) return;

        // Confirm and remove
        //  ImageUploads.Remove(SelectedStamp);
        SelectedLoan = null;
    }


    private async void GetLoansForList()
    {
        try
        {
            var userName = Preferences.Get("UserName", "");



            var response = await _httpClient.GetStringAsync("http://10.110.240.4:5000/Home/loansforlist?userName=" + userName);

            var loans = JsonSerializer.Deserialize<List<LoanListDto>>(response);


            Loans.Clear();
            foreach (var loan in loans)
            {
                var loanDto = new LoanForLIstDto();

                loanDto.LoanDate = loan.LoanDate;
                loanDto.Lender = loan.Lender;
                loanDto.Id = loan.Id;
                Loans.Add(loanDto);
            }
        }
        catch (Exception ex)
        {
            // Handle any errors (e.g., API failure, deserialization issues)
            Console.WriteLine($"Error loading images: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}