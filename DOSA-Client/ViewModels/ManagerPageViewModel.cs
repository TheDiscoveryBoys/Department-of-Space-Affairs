using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.lib.Constants;
using DOSA_Client.lib;
using DOSA_Client.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace DOSA_Client.ViewModels
{
    class ManagerPageViewModel : ScreenViewModelBase
    {
        public string Title => "Role Management";
        public PageManager PageManager { get; set; }
        private Role _selectedRole;
        public Role SelectedRole {
            get => _selectedRole;
            set 
            {
                if (value != null)
                {
                    IsUpdateButtonEnabled = true;
;                }
                _selectedRole = value;
                OnPropertyChanged(nameof(IsUpdateButtonEnabled));
            }
        }
        public string UserEmail { get; set; }

        private List<Role> _roles;
        public List<Role> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }
        public bool IsUpdateButtonEnabled { get; set; }

        public ManagerPageViewModel(PageManager pageManager)
        {
            PageManager = pageManager;

            OnUpdateUserRoles = new RelayCommand(updateRoles);
            IsUpdateButtonEnabled = false;

            Task.Run(async () =>
            {
                Roles = await ApiClient.GetAllRoles();
            });
        }

        public ICommand OnUpdateUserRoles { get; }
        public async void updateRoles()
        {
            // get user id by email
            User? user = await ApiClient.GetUserByEmail(UserEmail);

            if (user == null)
            {
                System.Windows.MessageBox.Show(
                    "No user found with that email address.",
                    "User Not Found",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return;
            }

            // delete old roles?

            // add new role
            var success = await ApiClient.AddUserRole(new UserRole(null, user.google_id, SelectedRole.id));

            if (success)
            {
                System.Windows.MessageBox.Show("User role updated successfully.", "Success");
                // reset form
                UserEmail = string.Empty;
                SelectedRole = null;
                OnPropertyChanged(nameof(UserEmail));
                OnPropertyChanged(nameof(SelectedRole));
            }
            else
            {
                System.Windows.MessageBox.Show("Unable to assign user this role.", "Error");
            }
        }
    }
}

