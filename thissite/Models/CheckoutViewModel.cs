using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Braintree;

namespace thissite.Models
{
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string ContactEmail { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string ContactPhoneNumber { get; set; }

        public Cart Cart { get; set; }

        [Display(Name = "Address")]
        public string ShippingAddressLine1 { get; set; }

        [Display(Name = "Apartment/Unit/Suite")]
        public string ShippingAddressLine2 { get; set; }

        [Display(Name = "City")]
        public string ShippingLocale { get; set; }



        [Display(Name = "State")]
        public string ShippingRegion { get; set; }

        [Display(Name = "Country")]
        public string ShippingCountry { get; set; }

        [Display(Name = "Zip Code")]
        public string ShippingPostalCode { get; set; }

        [Required]
        [Display(Name = "Name on Card")]
        public string BillingNameOnCard { get; set; }


        //[CreditCard]  -- A little buggy!
        [Required]
        [Display(Name = "Credit Card Number")]
        [MaxLength(16)]
        public string BillingCardNumber { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        [Range(1, 12)]
        public int BillingCardExpirationMonth { get; set; }

        [Required]
        public int BillingCardExpirationYear { get; set; }

        [Required]
        [Display(Name = "CVV/CVV2")]
        public string BillingCardVerificationValue { get; set; }

        [Display(Name = "Save Credit Card")]
        public bool SaveBillingCard { get; set; }

        [Display(Name = "Save Shipping Address")]
        public bool SaveShippingAddress { get; set; }
        public CreditCard[] CreditCards { get; set; }
        public Address[] Addresses { get; set; }

        public string SavedAddressId { get; set; }
    }
}