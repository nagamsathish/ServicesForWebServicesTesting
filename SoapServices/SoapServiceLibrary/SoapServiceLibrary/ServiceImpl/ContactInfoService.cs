﻿using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using SoapServices.DAL.EducationalInstitution;

namespace SoapServiceLibrary.ServiceImpl
{
    /// <summary>
    /// Contact Info Service 
    /// </summary>
    public class ContactInfoService : IContactInfoService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ContactInfoService));

        /// <summary>
        /// This method will be used to add the contact information
        /// </summary>
        /// <param name="contactInformation"></param>
        /// <returns></returns>
        public bool AddContactInfo(Definitions.ContactInformation contactInformation)
        {
            Log.DebugFormat("Recived Request to AddContactInfo with ContactInformation:{0}", contactInformation);

            using (EducationalInstitutionDbContext context = new EducationalInstitutionDbContext())
            {
                context.ContactInformations.Add(new ContactInformation
                {
                    EmailId = contactInformation.EmailId,
                    FacebookId = contactInformation.FacebookId,
                    LinkedInId = contactInformation.LinkedInId,
                    PrimaryContactNumber = contactInformation.PrimaryContactNumber,
                    SecondaryContactNumber = contactInformation.SecondaryContactNumber,
                    SkypeId = contactInformation.SkypeId
                });
                context.SaveChanges();
            }
            Log.Debug("Addition Succesful & sending response as true");
            return true;
            
        }
        /// <summary>
        /// Deletes Contact By email id
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns>number of records deleted</returns>
        public int DeleteContactsByEmailId(string emailId)
        {
            Log.DebugFormat(" Received Request to DeleteContactsByEmailId with emailid: {0}", emailId);
            int count = 0;
            using (EducationalInstitutionDbContext context = new EducationalInstitutionDbContext())
            {
                var items=context.ContactInformations.Where(item => item.EmailId.Equals(emailId, StringComparison.OrdinalIgnoreCase));
                Parallel.ForEach(items, item =>
                {
                    Log.DebugFormat("Removing record with contactInformation {0}", item);
                });
                count = items.Count();
                context.ContactInformations.RemoveRange(items);
                context.SaveChanges();
            }
            Log.Debug("Processed Request DeleteContactsByEmailId");
            return count;
        }

        /// <summary>
        /// This method returns all the contact information
        /// </summary>
        /// <returns></returns>
        public Definitions.ContactInformation[] GetAllContactInformations()
        {
            Log.Debug("Received Request for the GetAllContactInformations");
            using (EducationalInstitutionDbContext context = new EducationalInstitutionDbContext())
            {
                Definitions.ContactInformation[] contacts=  context.ContactInformations.Select(item => new Definitions.ContactInformation
                {
                    EmailId = item.EmailId,
                    FacebookId = item.FacebookId,
                    LinkedInId = item.LinkedInId,
                    PrimaryContactNumber = item.FacebookId,
                    SecondaryContactNumber = item.SecondaryContactNumber,
                    SkypeId = item.SkypeId
                }).ToArray();
                //Logging the contacts
                Parallel.ForEach(contacts, item =>{Log.Debug(item);});
                Log.Debug("Processed Request for the GetAllContactInformations");
                return contacts;
            }
        }

        /// <summary>
        /// Gets the Contact Information By Mail
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public Definitions.ContactInformation[] GetContactInformationByMail(string emailId)
        {
            Log.DebugFormat("Received Request for GetContactInformationByMail with email id {0}",emailId);
            Definitions.ContactInformation[] contactInformations;
            using (EducationalInstitutionDbContext context = new EducationalInstitutionDbContext())
            {
                contactInformations = context.ContactInformations.Where(
                    item => item.EmailId.Equals(emailId, StringComparison.OrdinalIgnoreCase)).Select(item=> new Definitions.ContactInformation
                    {
                        EmailId = item.EmailId,
                        FacebookId = item.FacebookId,
                        LinkedInId = item.LinkedInId,
                        PrimaryContactNumber = item.PrimaryContactNumber,
                        SecondaryContactNumber = item.SecondaryContactNumber,
                        SkypeId = item.SkypeId
                    }).ToArray();
                Parallel.ForEach(contactInformations, item => { Log.Debug(item); });
                Log.DebugFormat("Processed Request for GetContactInformationByMail with email id {0}",emailId);
                return contactInformations;
            }
        }
    }
}
