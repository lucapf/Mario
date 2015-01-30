using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace mediatori.Models
{

    public class ChangeEmailModel
    {
        string currentEmail { get; set; }
        string newEmail { get; set; }
        string newEmailConfirm { get; set; }
    }

    public class ManageModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Nome { get; set; }
        public string Cognome { get; set; }

        public DateTime? datePreviousLogin { get; set; }

        public string NomeCognome
        {
            get
            {
                string temp;

                temp = Nome;

                if (String.IsNullOrEmpty(temp))
                {
                    temp = Cognome;
                }
                else
                {
                    temp += " " + Cognome;
                }

                return temp;
            }
        }

        //Credenziali per CreditoLab
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
       
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "Nome utente")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password corrente")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nuova password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Conferma nuova password")]
        [Compare("NewPassword", ErrorMessage = "La nuova password e la password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }

        public string user { get; set; }


    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "Nome utente")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Memorizza account")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Nome utente")]
        public string UserName { get; set; }

       
        [Required]
        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Conferma password")]
        [Compare("Password", ErrorMessage = "La Password e la Password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Profilo")]
        [Required]
        public string[] roles { get; set; }

        public List<MyUsers.Models.MyProfile> ProfiliDisponibili { get; set; }

    }

    public class RolesManagement
    {
        [Required]
        [Display(Name = "Nome utente")]
        public String username { get; set; }
        [Required]
        [Display(Name = "Ruoli utente")]
        public String[] ruoli { get; set; }
    }


    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
