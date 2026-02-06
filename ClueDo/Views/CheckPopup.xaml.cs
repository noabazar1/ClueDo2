using CommunityToolkit.Maui.Views;
namespace ClueDo.Views;

public partial class CheckPopup : Popup
{
	public CheckPopup(bool roomCorrect, bool weaponCorrect, bool suspectCorrect)
	{
		InitializeComponent();
		SetMark(RoomMark, roomCorrect);
		SetMark(WeaponMark, weaponCorrect);
		SetMark(SuspectMark, suspectCorrect);
	}

	private void SetMark(Label label, bool isCorrect)
	{
		label.FontFamily = null;
		if (isCorrect)
		{
			label.Text = "\u2714";
			label.TextColor = Colors.Green;
		}
		else
		{
			label.Text = "\u2716";
			label.TextColor = Colors.Red;
		}
	}
    private void OnOkClicked(object sender, EventArgs e)
    {
		Close();
    }
}