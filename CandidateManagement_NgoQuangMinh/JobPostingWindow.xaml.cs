using Candidate_BusinessObject;
using Candidate_Service;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CandidateManagement_NgoQuangMinh
{
    /// <summary>
    /// Interaction logic for JobPostingWindow.xaml
    /// </summary>
    public partial class JobPostingWindow : Window
    {
        private readonly IJobPostingService jobService;
        private readonly int? roleID;
        public JobPostingWindow(int? roleID)
        {
            InitializeComponent();
            this.jobService = new JobPostingService();
            LoadList();
            this.roleID = roleID;
            SidebarHeaderWindow headerWindow = (SidebarHeaderWindow)this.FindName("headerWindowControl");
            if (headerWindow != null)
            {
                headerWindow.RoleID = roleID;
            }
        }

        private void LoadList()
        {
            try
            {
                this.cmbPostID.ItemsSource = jobService.GetJobPostings(); // Ensure this returns data
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job postings: {ex.Message}");
            }
        }

        private void dtgJobPosting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedItem != null)
            {
                JobPosting jobPosting = dataGrid.SelectedItem as JobPosting;
                if (jobPosting != null)
                {
                    txtPostingID.Text = jobPosting.PostingId;
                    txtJobPostingTitle.Text = jobPosting.JobPostingTitle;
                    txtDescription.Text = jobPosting.Description;
                    PostedDate.SelectedDate = jobPosting.PostedDate; // Use SelectedDate for DatePicker
                }
            }
        }


        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {

                if (cmbPostID.SelectedItem == null)
                {
                    MessageBox.Show("Please select a candidate profile to delete.");
                    return;
                }
                DataGridRow row = (DataGridRow)cmbPostID.ItemContainerGenerator.ContainerFromItem(cmbPostID.SelectedItem);
                if (row == null)
                {
                    MessageBox.Show("Error: Could not retrieve the selected candidate row.");
                    return;
                }
                DataGridCell RowColumn = cmbPostID.Columns[0].GetCellContent(row).Parent as DataGridCell;
                if (RowColumn == null)
                {
                    MessageBox.Show("RowColumn == null.");
                }
                string postingId = ((TextBlock)RowColumn.Content).Text;
                if (postingId == null)
                {
                    MessageBox.Show("postingId == null.");
                }
                bool isDelete = jobService.deleteJobPosting(postingId);
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

                this.LoadList();
            }
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!string.IsNullOrWhiteSpace(txtPostingID.Text))
                {
                    IJobPostingService jobPostingService = new JobPostingService();

                    JobPosting existingPosting = jobPostingService.GetJobPosting(txtPostingID.Text);

                    if (existingPosting != null)
                    {
                        existingPosting.JobPostingTitle = txtJobPostingTitle.Text;
                        existingPosting.Description = txtDescription.Text;
                        existingPosting.PostedDate = PostedDate.SelectedDate;

                        bool isUpdated = jobPostingService.updateJobPosting(existingPosting);
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
            JobPosting jobPosting = new JobPosting();
            jobPosting.PostingId = txtPostingID.Text;
            jobPosting.JobPostingTitle = txtJobPostingTitle.Text;
            jobPosting.Description = txtDescription.Text;
            jobPosting.PostedDate = PostedDate.SelectedDate;
            bool isAdded = jobService.addJobPosting(jobPosting);
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
    }
}
