using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandidateManagement_NgoQuangMinh
{
    /// <summary>
    /// Interaction logic for SidebarHeaderWindow.xaml
    /// </summary>
    public partial class SidebarHeaderWindow : UserControl
    {
        public SidebarHeaderWindow()
        {
            InitializeComponent();
        }

        public int? RoleID { get; set; } 

        private void CandidateProfile_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Hide();
            CandidateProfileManager candidateProfileManager = new CandidateProfileManager(RoleID);
            candidateProfileManager.Show();
        }

        private void JobPosting_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Hide();
            JobPostingWindow jobPostingWindow = new JobPostingWindow(RoleID);
            jobPostingWindow.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
