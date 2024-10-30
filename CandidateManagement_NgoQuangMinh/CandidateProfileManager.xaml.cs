using Candidate_BusinessObject;
using Candidate_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandidateManagement_NgoQuangMinh
{
    /// <summary>
    /// Interaction logic for CandidateProfileManager.xaml
    /// </summary>
    public partial class CandidateProfileManager : Window
    {
        private readonly ICandidateProfileService profileService;
        private readonly IJobPostingService jobService;
        private readonly int? roleID;

        public CandidateProfileManager()
        {
            InitializeComponent();
            this.profileService = new CandidateProfileService();
            this.jobService = new JobPostingService();
          
        }
        public CandidateProfileManager(int? roleID)
        {
            InitializeComponent();
            this.profileService = new CandidateProfileService();
            this.jobService = new JobPostingService();
            this.roleID = roleID;
            SidebarHeaderWindow headerWindow = (SidebarHeaderWindow)this.FindName("headerWindowControl");
            if (headerWindow != null)
            {
                headerWindow.RoleID = roleID;
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (roleID)
            {
                case 1:
                    //Admin
                    this.BtnAdd.IsEnabled = false;
                    break;
                case 2:                  
                    break;
                case 3:
                    //Staff
                    this.BtnAdd.IsEnabled = false;

                    break;
                default:

                    break;
            }
            LoadList();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {

                if (dtgCandidateProfile.SelectedItem == null)
                {
                    MessageBox.Show("Please select a candidate profile to delete.");
                    return;
                }
                DataGridRow row = (DataGridRow)dtgCandidateProfile.ItemContainerGenerator.ContainerFromItem(dtgCandidateProfile.SelectedItem);
                if (row == null)
                {
                    MessageBox.Show("Error: Could not retrieve the selected candidate row.");
                    return;
                }
                DataGridCell RowColumn = dtgCandidateProfile.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string candidateId = ((TextBlock)RowColumn.Content).Text;
                bool isDelete = profileService.deleteCandidateProfile(candidateId);
                if (isDelete)
                {
                    MessageBox.Show("Candidate profile deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to delete candidate profile.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {

                LoadList();
            }
        }


        private void Button_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtID.Text))
                {
                    ICandidateProfileService candidateProfileService = new CandidateProfileService();

                    CandidateProfile existingProfile = candidateProfileService.GetCandidateProfile(txtID.Text);

                    if (existingProfile != null)
                    {
                        existingProfile.Fullname = txtName.Text;
                        existingProfile.ProfileShortDescription = txtDescript.Text;
                        existingProfile.Birthday = BirthDay.SelectedDate;
                        existingProfile.ProfileUrl = img.Text;
                        existingProfile.PostingId = cmbPostID.SelectedValue?.ToString();

                        bool isUpdated = candidateProfileService.updateCandidateProfile(existingProfile);
                        if (isUpdated)
                        {
                            MessageBox.Show("Candidate profile updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to update candidate profile.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Candidate not found.");
                    }
                }
                else
                {
                    MessageBox.Show("You must select a candidate!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                LoadList();
            }
        }



        private void Button_Add(object sender, RoutedEventArgs e)
        {

            CandidateProfile candidateProfile = new CandidateProfile();
            candidateProfile.Fullname = txtName.Text;
            candidateProfile.CandidateId = txtID.Text;
            candidateProfile.ProfileShortDescription = txtDescript.Text;
            candidateProfile.Birthday = BirthDay.SelectedDate;
            candidateProfile.ProfileUrl = img.Text;
            candidateProfile.PostingId = cmbPostID.SelectedValue.ToString();
            bool isAdded = profileService.addCandidateProfile(candidateProfile);
            if (isAdded)
            {
                MessageBox.Show("Candidate profile added successfully.");
                this.LoadList();
            }
            else
            {
                MessageBox.Show("Failed to add candidate profile.");
            }
        }




        private void LoadList()
        {
            this.dtgCandidateProfile.ItemsSource = profileService.GetCandidates().Select(a => new { a.CandidateId, a.Fullname, a.Posting.JobPostingTitle });
            this.cmbPostID.ItemsSource = jobService.GetJobPostings();
            this.cmbPostID.DisplayMemberPath = "JobPostingTitle";
            this.cmbPostID.SelectedValuePath = "PostingId";
        }

        private void dtgCandidateProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
            if (row != null)
            {
                DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string id = ((TextBlock)RowColumn.Content).Text;
                CandidateProfile candidateProfile = profileService.GetCandidateProfile(id);
                if (candidateProfile != null)
                {
                    txtID.Text = candidateProfile.CandidateId.ToString();
                    txtName.Text = candidateProfile.Fullname;
                    txtDescript.Text = candidateProfile.ProfileShortDescription;
                    BirthDay.Text = candidateProfile.Birthday.ToString();
                    img.Text = candidateProfile.ProfileUrl;
                    cmbPostID.SelectedValue = candidateProfile.PostingId;
                }
            }
        }

    }
}
