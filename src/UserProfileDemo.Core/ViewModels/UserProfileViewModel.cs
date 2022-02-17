using System.Threading.Tasks;
using System.Windows.Input;
using CouchbaseLabs.MVVM;
using CouchbaseLabs.MVVM.Input;
using CouchbaseLabs.MVVM.Services;
using UserProfileDemo.Core.Respositories;
using UserProfileDemo.Core.Services;
using UserProfileDemo.Models;

namespace UserProfileDemo.Core.ViewModels
{
    public class UserProfileViewModel : BaseNavigationViewModel
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IAlertService _alertService;
        private readonly IMediaService _mediaService;

        string UserProfileDocId 
        {
            get
            {
                if (AppInstance.User != null)
                {
                    return $"user::{AppInstance.User.Username}";
                }
                else
                {
                    return $"user::";
                }
            }
        }

        string _name;
        public string Name
        {
            get => _name;
            set => SetPropertyChanged(ref _name, value);
        }

        string _email;
        public string Email
        {
            get => _email;
            set => SetPropertyChanged(ref _email, value);
        }

        string _address;
        public string Address
        {
            get => _address;
            set => SetPropertyChanged(ref _address, value);
        }

        byte[] _imageData;
        public byte[] ImageData
        {
            get => _imageData;
            set => SetPropertyChanged(ref _imageData, value);
        }

        string _university;
        public string University
        {
            get => _university ?? "Select University";
            set => SetPropertyChanged(ref _university, value);
        }

        ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(async() => await Save());
                }

                return _saveCommand;
            }
        }

        ICommand _selectImageCommand;
        public ICommand SelectImageCommand
        {
            get
            {
                if (_selectImageCommand == null)
                {
                    _selectImageCommand = new Command(async () => await SelectImage());
                }

                return _selectImageCommand;
            }
        }

        ICommand _selectUniversityCommand;
        public ICommand SelectUniversityCommand
        {
            get
            {
                if (_selectUniversityCommand == null)
                {
                    _selectUniversityCommand = new Command(async () => await NavigateToUniversities());
                }

                return _selectUniversityCommand;
            }
        }

        ICommand _logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                if (_logoutCommand == null)
                {
                    _logoutCommand = new Command(Logout);
                }

                return _logoutCommand;
            }
        }

        public UserProfileViewModel(INavigationService navigationService, 
                                    IUserProfileRepository userProfileRepoiory,
                                    IAlertService alertService,
                                    IMediaService mediaService) : base(navigationService)
        {
            _userProfileRepository = userProfileRepoiory;
            _alertService = alertService;
            _mediaService = mediaService;
        }

        public override async Task LoadAsync(bool refresh)
        {
            await LoadUserProfile();
        }

        private async Task LoadUserProfile()
        {
            IsBusy = true;

            if (string.IsNullOrEmpty(Email))
            {
                var userProfile = await _userProfileRepository?.GetAsync(UserProfileDocId);

                if (userProfile == null)
                {
                    userProfile = new UserProfile
                    {
                        Id = UserProfileDocId,
                        Email = AppInstance.User.Username
                    };
                }

                Name = userProfile.Name;
                Email = userProfile.Email;
                Address = userProfile.Address;
                ImageData = userProfile.ImageData;
                University = userProfile.University;
            }

            IsBusy = false;
        }

        private async Task Save()
        {
            var userProfile = new UserProfile
            {
                Id = UserProfileDocId,
                Name = Name,
                Email = Email,
                Address = Address, 
                ImageData = ImageData,  
                University = University
            };
   
            var success = await _userProfileRepository.SaveAsync(userProfile).ConfigureAwait(false);

            if (success)
            {
                await _alertService.ShowMessage(null, "Successfully updated profile!", "OK");
            }
            else
            {
                await _alertService.ShowMessage(null, "Error updating profile!", "OK");
            }
        }

        private async Task SelectImage()
        {
            var imageData = await _mediaService.PickPhotoAsync();

            if (imageData != null)
            {
                ImageData = imageData;
            }
        }

        private Task NavigateToUniversities()
        {
            var vm = ServiceContainer.GetInstance<UniversitiesViewModel>();

            vm.UniversitySelected = UniversitySelected;

            return Navigation.PushAsync(vm);
        }

        private void UniversitySelected(string name) => University = name;

        private void Logout()
        {
            _userProfileRepository.Dispose();

            AppInstance.User = null;

            Navigation.ReplaceRoot<LoginViewModel>(false);
        }
    }
}
